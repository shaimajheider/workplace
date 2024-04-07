using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using Vue.Models;
using Web.Services;
using static Web.Services.Helper;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/Dictionaries")]
    public class DictionariesController : Controller
    {
        private Helper help;

        private readonly WorkplaceReservationContext db;

        public DictionariesController(WorkplaceReservationContext context, IConfiguration iConfig)
        {
            this.db = context;
            help = new Helper(iConfig, context);
        }



        public partial class BodyObjct
        {
            public long? Id { get; set; }
            public string Name { get; set; }
        }


        //PaymentMethods
        [HttpPost("PaymentMethods/Add")]
        public IActionResult AddPaymentMethods([FromBody] BodyObjct bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (string.IsNullOrWhiteSpace(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


                var isExist = db.PaymentMethods.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                PaymentMethods row = new PaymentMethods();
                row.Name = bodyObject.Name;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.PaymentMethods.Add(row);


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "PaymentMethods/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("PaymentMethods/Edit")]
        public IActionResult EditPaymentMethods([FromBody] BodyObjct bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                TransactionsObject rowTrans = new TransactionsObject();
                
                var row = db.PaymentMethods.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);
                
                rowTrans.OldObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                var isExist = db.PaymentMethods.Where(x => x.Name == bodyObject.Name && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                row.Name = bodyObject.Name;

                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تعديل بيانات   ";
                rowTrans.Controller = "PaymentMethods/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();

                return Ok(BackMessages.SucessEditOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("PaymentMethods/Get")]
        public IActionResult GetPaymentMethods(int pageNo, int pageSize)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                int Count = db.PaymentMethods
                    .Where(x => x.Status != 9
                    ).Count();
                var Info = db.PaymentMethods
                    .Where(x => x.Status != 9
                ).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Status,
                    x.CreatedOn,
                    CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("PaymentMethods/GetAll")]
        public IActionResult GetAllPaymentMethods()
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);
                
                var Info = db.PaymentMethods
                    .Where(x => x.Status != 9
                ).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Status,
                    x.CreatedOn,
                    CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                }).OrderByDescending(x => x.Name).ToList();


                return Ok(new { info = Info});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/PaymentMethods/Delete")]
        public IActionResult DeletePaymentMethods(long id)
        {
            try
            {
                if (id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var row = db.PaymentMethods.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "PaymentMethods/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }












        [HttpGet("ContactUs/Get")]
        public IActionResult GetContactUs(int pageNo, int pageSize)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                int Count = db.ContactUs.Where(x => x.Status != 9).Count();
                var Info = db.ContactUs.Where(x => x.Status != 9).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Email,
                    x.Phone,
                    x.Mesaage,
                    x.CreatedOn,
                    x.Status,
                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { info = Info, count = Count });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        //[HttpGet("GetDashboardInfo")]
        //public IActionResult GetDashboardInfo()
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();

        //        int Inside = db.PaymentMethods.Where(x => x.Type==1 && x.Status!=9).Count();
        //        int OutSide = db.PaymentMethods.Where(x => x.Type==2 && x.Status!=9).Count();


        //        int Users = db.Users.Where(x => (user.UserType != 1 ? x.OfficeId == user.OfficeId : true) && x.Status != 9).Count();
        //        int UserAdmin = db.Users.Where(x => (user.UserType != 1 ? x.OfficeId == user.OfficeId : true) && x.UserType == 1 && x.Status != 9).Count();
        //        int UserEmp = db.Users.Where(x => (user.UserType != 1 ? x.OfficeId == user.OfficeId : true) && x.UserType == 1 && x.Status != 9).Count();

        //        int ActiveAcount = db.Users.Where(x => x.UserType == 1
        //        && x.Status != 9
        //        && x.LastLoginOn >= DateTime.Now.AddMinutes(-30) && x.LastLoginOn <= DateTime.Now.AddMinutes(30)
        //        && (user.UserType != 1 ? x.OfficeId == user.OfficeId : true)).Count();
        //        int time = DateTime.Now.Day;


        //        int AllMessage = db.Messages.Where(x => x.Status != 9 &&( x.SendTo == user.OfficeId || x.OfficeId==user.OfficeId)).Count();

        //        int InboxMessage = db.Messages.Where(x => x.Status!=9 && x.SendToNavigation.Type==1
        //            && x.SendTo == user.OfficeId).Count();
        //        int InboxOutMessage = db.Messages.Where(x => x.Status!=9 && x.SendToNavigation.Type==2
        //            && x.SendTo == user.OfficeId).Count();

        //        int SenderMessage = db.Messages.Where(x => x.Status!=9 && x.Office.Type==1
        //            && x.OfficeId == user.OfficeId).Count();
        //        int SenderOutMessage = db.Messages.Where(x => x.Status!=9 && x.Office.Type==2
        //            && x.OfficeId == user.OfficeId).Count();


        //        var Info = new
        //        {
        //            Inside,
        //            OutSide,
        //            AllMessage,
        //            InboxMessage,
        //            InboxOutMessage,
        //            SenderMessage,
        //            SenderOutMessage,
        //            Users,
        //            UserAdmin,
        //            UserEmp,
        //            ActiveAcount,
        //            time
        //        };


        //        return Ok(new { info = Info });


        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}







    }
}