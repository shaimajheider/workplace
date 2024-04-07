using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vue.Models;
using static Web.Services.Helper;

namespace Vue.Controllers
{
    [Produces("application/json")]
    [Route("Security")]
    public class SecurityController : Controller
    {
        private readonly WorkplaceReservationContext db;
        private IConfiguration _configuration;
        public Validation valid;
        public SecurityController(WorkplaceReservationContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            valid = new Validation();
            this.db = context;
        }


        [AllowAnonymous]
        [HttpGet("IsLoggedin")]
        public async Task<IActionResult> IsLogin(string returnUrl = null)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                return Ok(true);
            }
            else
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok(false);
            }
        }

        public class BodyObject
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("loginUser")]
        [AllowAnonymous]
        public async Task<IActionResult> loginUser([FromBody] BodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                
                if (string.IsNullOrWhiteSpace(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterEmailandUserName);

                if (string.IsNullOrWhiteSpace(bodyObject.Password))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

                var Info = (from p in db.Users
                             where (p.Email == bodyObject.Email || p.LoginName == bodyObject.Email) && p.Status != 9
                             select p).SingleOrDefault();

                if (Info == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.RongPasswordandEmail);

                if (Info.UserType != 1 && Info.UserType != 2 )
                    return StatusCode(BackMessages.StatusCode, BackMessages.DontHavePermisine);

                if (Info.Status == 2)
                {
                    if (Info.LoginTryAttemptDate != null)
                    {
                        DateTime dt = Info.LoginTryAttemptDate.Value;
                        double minuts = 30;
                        dt = dt.AddMinutes(minuts);
                        if (dt >= DateTime.Now)
                        {
                            return StatusCode(BackMessages.StatusCode, BackMessages.YourAcountisBlocked);
                        }
                        else
                        {
                            Info.Status = 1;

                            db.SaveChanges();
                        }
                    }
                    else { return StatusCode(BackMessages.StatusCode, BackMessages.YourAcountisBlocked); }
                }

                if (!Security.VerifyHash(bodyObject.Password, Info.Password, HashAlgorithms.SHA512))
                {

                    Info.LoginTryAttempts++;
                    if (Info.LoginTryAttempts >= 5 && Info.Status == 1)
                    {
                        Info.LoginTryAttemptDate = DateTime.Now;
                        Info.Status = 2;
                    }
                    db.SaveChanges();
                    return StatusCode(BackMessages.StatusCode, BackMessages.RongPasswordandEmail);
                }

                Info.LoginTryAttempts = 0;
                Info.LastLoginOn = DateTime.Now;
                db.SaveChanges();
                string OfficeName = "";

                //if (Info.UserType != 1 && Info.UserType != 3)
                //{
                //    if(Info.PortId <= 0 || Info.PortId == null)
                //        return StatusCode(BackMessages.StatusCode, BackMessages.YouDontHaveCenter);

                //    OfficeName = db.Ports.Where(x => x.Id == Info.PortId).SingleOrDefault().Name;
                //}

                var row = new
                {
                    Info.Id,
                    Info.Name,
                    Info.LoginName,
                    OfficeName,
                    Info.UserType,
                    Info.Email,
                    Info.Phone,
                    Info.Status,
                    Info.CreatedOn,
                    Info.LastLoginOn,
                    Info.Image,
                    CreatedBy=db.Users.Where(x=>x.Id==Info.Id).SingleOrDefault().Name,
                };

                const string Issuer = "http://www.hnec.ly";
                var claims = new List<Claim>();
                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2022/10/identity/claims/Id", Info.Id.ToString(), ClaimValueTypes.Integer64, Issuer));

                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2022/10/identity/claims/userType", Info.UserType.ToString(), ClaimValueTypes.Integer32, Issuer));
                var userIdentity = new ClaimsIdentity("thisisasecreteforauth");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);


                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                return Ok(row);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(BackMessages.StatusCode,BackMessages.Errorwhilelogout);
            }
        }

    }
}
