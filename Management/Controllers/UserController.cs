using Common;
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
    [Route("api/admin/User")]
    public class UserController : Controller
    {
        private Helper help;

        private readonly WorkplaceReservationContext db;

        public UserController(WorkplaceReservationContext context, IConfiguration iConfig)
        {
            this.db = context;
            help = new Helper(iConfig, context);
        }
        public partial class BodyObject
        {
            public long? Id { get; set; }
            public short? PortId { get; set; }
            public string Name { get; set; }
            public string LoginName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfimPassword { get; set; }
            public string Phone { get; set; }
            public short UserType { get; set; }
            public string ImageName { get; set; }
            public string fileBase64 { get; set; }
            public string ImageType { get; set; }
            
        }

        public class UserInfo
        {
            public string Password { set; get; }
            public string NewPassword { set; get; }
        }


        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize,int UserType, short PortId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Count = db.Users.Where(x => x.Status != 9
                    //&& (user.UserType != 1 ? x.PortId == user.PortId : true)
                    && (UserType > 0 && user.UserType == 1 ? x.UserType == UserType : true)
                    //&& (PortId > 0 && user.UserType == 1 ? x.PortId == PortId : true)
                ).Count();

                var Info = db.Users.Where(x => x.Status != 9
                    //&& (user.UserType != 1 ? x.PortId == user.PortId : true)
                    && (UserType > 0 && user.UserType == 1 ? x.UserType == UserType : true)
                     //&& (PortId > 0 && user.UserType == 1 ? x.PortId == PortId : true)
                ).Select(x => new
                {
                    x.Id,
                    //x.PortId,
                    //Port=db.Ports.Where(k=>k.Id==x.PortId).SingleOrDefault().Name,
                    x.Name,
                    x.LoginName,
                    x.Email,
                    x.Phone,
                    x.Image,
                    x.UserType,
                    x.Status,
                    x.LastLoginOn,
                    x.CreatedOn,
                    CreatedBy=db.Users.Where(k=>k.Id==x.CreatedBy).SingleOrDefault().Name,
                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { info = Info, count = Count });

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] BodyObject bodyObject)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if(bodyObject==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);


                if (string.IsNullOrWhiteSpace(bodyObject.LoginName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Password))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

                if (string.IsNullOrWhiteSpace(bodyObject.ConfimPassword))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

                if (bodyObject.UserType <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PermissioneEmpty);

                if (bodyObject.UserType == 2)
                {
                    if (bodyObject.PortId <= 0 || bodyObject.PortId==null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.KidneyCenterEmpty);
                }


                bodyObject.Phone = bodyObject.Phone.Substring(bodyObject.Phone.Length - 9);

                //valid input
                if (!help.IsValidEmail(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

                if (!help.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                if (!help.IsCorrectPassword(bodyObject.Password, bodyObject.ConfimPassword))
                    return StatusCode(BackMessages.StatusCode, BackMessages.ConfirmPassword);

                if (bodyObject.Password.Length < 8)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PasswordLenght);


                //Is Exist
                var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                IsExist = db.Users.Where(x => x.Status != 9 && x.Name == bodyObject.Name).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                IsExist = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

                IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);




                Users row = new Users();
                if (user.UserType == 2)
                {
                    //row.PortId = user.PortId;
                    row.UserType = 2;
                }
                else
                {
                    //row.PortId = bodyObject.PortId;
                    row.UserType = bodyObject.UserType;
                }


                row.Name = bodyObject.Name;
                row.Email = bodyObject.Email;
                row.Phone = bodyObject.Phone;
                row.LoginName = bodyObject.LoginName;
                row.Password = Security.ComputeHash(bodyObject.Password, HashAlgorithms.SHA512, null);
                row.LoginTryAttempts = 0;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Image = this.help.UploadFile("Default.jpg", ".jpg", this.help.GetDefaultImage());
                //row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.ImageType, bodyObject.fileBase64);
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.Users.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة مستخدم جديد ";
                rowTrans.Controller = "User";
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


        [HttpPost("Edit")]
        public IActionResult Edit([FromBody] BodyObject bodyObject)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Users.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);
                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(row);
                if (string.IsNullOrWhiteSpace(bodyObject.LoginName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

                if (bodyObject.UserType <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PermissioneEmpty);

                if (bodyObject.UserType == 2)
                {
                    if (bodyObject.PortId <= 0)
                        return StatusCode(BackMessages.StatusCode, BackMessages.KidneyCenterEmpty);
                }


                bodyObject.Phone = bodyObject.Phone.Substring(bodyObject.Phone.Length - 9);

                //valid input
                if (!help.IsValidEmail(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

                if (!help.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);


                //Is Exist
                var isExist = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone && x.Id != bodyObject.Id).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

                isExist = db.Users.Where(x => x.Status != 9 && x.Name == bodyObject.Name && x.Id != bodyObject.Id).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                isExist = db.Users.Where(x => x.Status != 9 && x.LoginName == bodyObject.LoginName && x.Id != bodyObject.Id).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                isExist = db.Users.Where(x => x.Status != 9 && x.Email == bodyObject.Email && x.Id != bodyObject.Id).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);

                if (bodyObject.UserType != 1 && bodyObject.UserType != 3)
                {
                    if (bodyObject.PortId <= 0 || bodyObject.PortId == null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.MuncitpitlyEmpty);
                }

                if (user.UserType == 2)
                {
                    //row.PortId = user.PortId;
                    row.UserType = 2;
                }
                else
                {
                    //row.PortId = bodyObject.PortId;
                    row.UserType = bodyObject.UserType;
                }
                row.LoginName = bodyObject.LoginName;
                row.Name = bodyObject.Name;
                row.Email = bodyObject.Email;
                row.Phone = bodyObject.Phone;


                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل بيانات مستخدم  ";
                rowTrans.Controller = "User";
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

        [HttpPost("{Id}/Delete")]
        public IActionResult Delete(long Id)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Users.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات مستخدم  ";
                rowTrans.Controller = "User";
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

        [HttpPost("{Id}/Suspend")]
        public IActionResult Suspend(long Id,string resone)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Users.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var IsExist = db.UserSuspends.Where(x => x.UserId == Id && x.Status != 9).SingleOrDefault();
                if(IsExist!=null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.UserSuspendBefore);

                UserSuspends userSuspends = new UserSuspends();
                userSuspends.UserId = Id;
                userSuspends.Resone = "";
                userSuspends.CreatedOn = DateTime.Now;
                userSuspends.CreatedBy = userId;
                userSuspends.Status = 1;
                db.UserSuspends.Add(userSuspends);

                row.Status = 2;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Suspend;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "إيقاف مستخدم    ";
                rowTrans.Controller = "User";
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

        [HttpPost("{Id}/RestePassword")]
        public IActionResult RestePassword(long Id)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Users.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                string Password = this.help.GenreatePass();

                row.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);
             

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.CahngeStatus;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تهيئة كلمة المرور   ";
                rowTrans.Controller = "User";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessResetOperations + " كلمة المرور الجديدة :" + "  " + Password);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        public class PatientAttachmentsObj
        {
            public string ImageName { get; set; }
            public string fileBase64 { get; set; }
            public string ImageType { get; set; }
        }

        [DisableRequestSizeLimit]
        [HttpPost("UploadImage")]
        public IActionResult AddAttachments([FromBody] PatientAttachmentsObj bodyObject)
        {
            try
            {

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (string.IsNullOrEmpty(bodyObject.fileBase64))
                    return StatusCode(BackMessages.StatusCode, BackMessages.ErorFile);

                row.Image = this.help.UploadFile(bodyObject.ImageName, this.help.GetAttachmentType(bodyObject.ImageName), bodyObject.fileBase64);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "  تغير صورة الملف الشخصي  ";
                rowTrans.Controller = "User";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);
                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations + "سيتم تغير الصورة عند الدخول للمنظومة مجددا");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }




        [HttpPost("EditUsersProfile")]
        public IActionResult EditUsersProfile([FromBody] BodyObject bodyObject)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if(bodyObject==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                if (string.IsNullOrEmpty(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);
                
                if(string.IsNullOrEmpty(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

                if (!this.help.IsValidEmail(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);
                
                if(!this.help.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                var row = db.Users.Where(x => x.Status != 9 && x.Id == userId).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(row);

                var isExist = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone && x.Id != userId).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

                isExist = db.Users.Where(x => x.Status != 9 && x.Name == bodyObject.Name && x.Id != userId).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                isExist = db.Users.Where(x => x.Status != 9 && x.LoginName == bodyObject.LoginName && x.Id != userId).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                isExist = db.Users.Where(x => x.Status != 9 && x.Email == bodyObject.Email && x.Id != userId).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);

                if (!string.IsNullOrEmpty(bodyObject.ImageName)
                   && !string.IsNullOrEmpty(bodyObject.ImageType)
                   && !string.IsNullOrEmpty(bodyObject.fileBase64))
                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.ImageType, bodyObject.fileBase64);

                row.Email = bodyObject.Email;
                row.Name = bodyObject.Name;
                row.LoginName = bodyObject.LoginName;
                row.Phone = bodyObject.Phone;
                db.Users.Update(row);

                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "  تعديل البيانات الشخصية   ";
                rowTrans.Controller = "User";
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

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] UserInfo bodyObject)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Users.Where(x => x.Status != 9 && x.Id==userId).SingleOrDefault();
                if(row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                if(string.IsNullOrEmpty(bodyObject.Password))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterCurrentPass);
                
                if(string.IsNullOrEmpty(bodyObject.NewPassword))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

                var areMatched = Security.VerifyHash(bodyObject.Password, row.Password, HashAlgorithms.SHA512);

                if (!areMatched)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PasswordRong);

                row.Password = Security.ComputeHash(bodyObject.NewPassword, HashAlgorithms.SHA512, null);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(row);

                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل كلمة المورو  ";
                rowTrans.Controller = "User";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();

                return Ok(BackMessages.SucessSaveOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/ChangeStatus")]
        public IActionResult ChangeStatus(long id)
        {
            try
            {
                if (id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Users.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                if (row.Status == 1)
                {
                    row.Status = 2;
                }
                else if (row.Status == 2)
                {
                    row.Status = 1;
                }

                db.SaveChanges();

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(row);

                rowTrans.Operations = TransactionsType.CahngeStatus;
                rowTrans.Descriptions = "تفير حالة المستخدم    ";
                rowTrans.Controller = "User";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);


                return Ok(BackMessages.SuccessChangeStatus);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}