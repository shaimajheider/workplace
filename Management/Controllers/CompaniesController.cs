using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Vue.Models;
using Web.Services;
using static Web.Services.Helper;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/Companies")]
    public class CompaniesController : Controller
    {
        private Helper help;

        private readonly WorkplaceReservationContext db;

        public CompaniesController(WorkplaceReservationContext context, IConfiguration iConfig)
        {
            this.db = context;
            help = new Helper(iConfig, context);
        }
        public partial class BodyObject
        {
            public long? Id { get; set; }
            public string Name { get; set; }
            public string LoginName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfimPassword { get; set; }
            public string Phone { get; set; }

            //Companies Table
            public string OwnerName { get; set; }
            public string OwnerPhone { get; set; }
            public string LocationDescriptions { get; set; }
            public string LocationLink { get; set; }
            public int FloorCount { get; set; }
            public int ClassRoomCount { get; set; }
            public int MeetingRoomCount { get; set; }
            public int TraningRoomCount { get; set; }
            public int PrivateRoomCount { get; set; }
            public int OfficeCount { get; set; }

            public List<AttachmentsObj> Attachments { get; set; } = new List<AttachmentsObj>();
        }

        public class AttachmentsObj
        {
            public string ImageName { get; set; }
            public string ImageType { get; set; }
            public string fileBase64 { get; set; }
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize,short level, long ById,DateTime? byDate)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Count = db.Companies.Where(x => x.Status != 9 && x.User.Level == level && x.User.Status != 9
                    //&& (user.UserType != 1 ? x.PortId == user.PortId : true)
                    //&& (UserType > 0 && user.UserType == 1 ? x.UserType == UserType : true)
                    && (ById > 0 ? x.UserId == ById : true)
                ).Count();

                var Info = db.Companies
                    .Include(x => x.User)
                    .Where(x => x.Status != 9 && x.User.Level == level && x.User.Status != 9
                     //&& (user.UserType != 1 ? x.PortId == user.PortId : true)
                     //&& (UserType > 0 && user.UserType == 1 ? x.UserType == UserType : true)
                     && (ById > 0 ? x.UserId == ById : true)
                ).Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.OwnerName,
                    x.OwnerPhone,
                    x.LocationDescriptions,
                    x.LocationLink,
                    x.FloorCount,
                    x.ClassRoomCount,
                    x.MeetingRoomCount,
                    x.TraningRoomCount,
                    x.PrivateRoomCount,
                    x.OfficeCount,
                    x.User.Name,
                    x.User.LoginName,
                    x.User.Email,
                    x.User.Phone,
                    x.User.Image,
                    x.User.Level,
                    x.User.Status,
                    x.User.LastLoginOn,
                    x.User.CreatedOn,
                    WalletValue=db.Wallet.Where(k=>k.Status!=9 && k.UserId==x.UserId).SingleOrDefault().Value,
                    CreatedBy = db.Users.Where(k => k.Id == x.User.CreatedBy).SingleOrDefault().Name,
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
                
                if (string.IsNullOrEmpty(bodyObject.OwnerName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.OwnerNameEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);
                
                if (string.IsNullOrWhiteSpace(bodyObject.OwnerPhone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.OwnerPhoneEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

                //if (string.IsNullOrWhiteSpace(bodyObject.Password))
                //    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

                //if (string.IsNullOrWhiteSpace(bodyObject.ConfimPassword))
                //    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);
                
                if (string.IsNullOrWhiteSpace(bodyObject.LocationDescriptions))
                    return StatusCode(BackMessages.StatusCode, BackMessages.RecordEmpty+ " وصف الموقع");
                
                if (string.IsNullOrWhiteSpace(bodyObject.LocationLink))
                    return StatusCode(BackMessages.StatusCode, BackMessages.RecordEmpty+ " الرابط الخاص بالموقع");

                if (bodyObject.Attachments.Count <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.AttachmentEmpty);


                bodyObject.Phone = bodyObject.Phone.Substring(bodyObject.Phone.Length - 9);
                bodyObject.OwnerPhone = bodyObject.Phone.Substring(bodyObject.Phone.Length - 9);

                //valid input
                if (!help.IsValidEmail(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

                if (!help.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);
                
                if (!help.IsValidPhone(bodyObject.OwnerPhone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                //if (!help.IsCorrectPassword(bodyObject.Password, bodyObject.ConfimPassword))
                //    return StatusCode(BackMessages.StatusCode, BackMessages.ConfirmPassword);

                //if (bodyObject.Password.Length < 8)
                //    return StatusCode(BackMessages.StatusCode, BackMessages.PasswordLenght);



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

                var IsExistCompany=db.Companies.Where(x => x.OwnerPhone == bodyObject.OwnerPhone && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist1);
                
                IsExistCompany=db.Companies.Where(x => x.OwnerPhone == bodyObject.OwnerPhone && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist1);


                List<CompainesAttachmenets> CompainesAttachmenetsList = new List<CompainesAttachmenets>();
                if (bodyObject.Attachments.Count > 0)
                {
                    foreach (var item1 in bodyObject.Attachments)
                    {
                        CompainesAttachmenets CompainesAttachmenetsRow = new CompainesAttachmenets();
                        CompainesAttachmenetsRow.Name = item1.ImageName;
                        CompainesAttachmenetsRow.Path = this.help.UploadFile(item1.ImageName, this.help.GetAttachmentType(item1.ImageName), item1.fileBase64);
                        CompainesAttachmenetsRow.CreatedBy = userId;
                        CompainesAttachmenetsRow.CreatedOn = DateTime.Now;
                        CompainesAttachmenetsRow.Status = 1;
                        CompainesAttachmenetsList.Add(CompainesAttachmenetsRow);
                    }
                }

                List<Wallet> WalletList = new List<Wallet>();
                Wallet wallet = new Wallet();
                wallet.Value = 0;
                wallet.CreatedBy = userId;
                wallet.CreatedOn = DateTime.Now;
                wallet.Status = 1;
                WalletList.Add(wallet);

                Users UserOjbect = new Users();
                UserOjbect.Name = bodyObject.Name;
                UserOjbect.LoginName = bodyObject.LoginName;
                UserOjbect.Phone = bodyObject.Phone;
                UserOjbect.Email = bodyObject.Email;
                //UserOjbect.Password = Security.ComputeHash(bodyObject.Password, HashAlgorithms.SHA512, null);
                UserOjbect.Password = Security.ComputeHash("12345", HashAlgorithms.SHA512, null);
                UserOjbect.UserType = 2;
                UserOjbect.Image= this.help.UploadFile("Default.jpg", ".jpg", this.help.GetDefaultImage());
                UserOjbect.LoginTryAttempts = 0;
                UserOjbect.CreatedBy = userId;
                UserOjbect.CreatedOn = DateTime.Now;
                UserOjbect.Status = 1;
                UserOjbect.Level = 2;
                UserOjbect.Wallet = WalletList;


                Companies row = new Companies();
                row.OwnerName=bodyObject.OwnerName;
                row.OwnerPhone=bodyObject.OwnerPhone;
                row.LocationDescriptions=bodyObject.LocationDescriptions;
                row.LocationLink=bodyObject.LocationLink;
                row.FloorCount=bodyObject.FloorCount;
                row.ClassRoomCount=bodyObject.ClassRoomCount;
                row.MeetingRoomCount=bodyObject.MeetingRoomCount;
                row.TraningRoomCount = bodyObject.TraningRoomCount;
                row.PrivateRoomCount = bodyObject.PrivateRoomCount;
                row.OfficeCount = bodyObject.OfficeCount;
                row.CreatedOn = DateTime.Now;
                row.CreatedBy = userId;
                row.Status = 1;
                row.User = UserOjbect;
                if (CompainesAttachmenetsList.Count > 0)
                    row.CompainesAttachmenets = CompainesAttachmenetsList;
                db.Companies.Add(row);
                

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة شركة جديد ";
                rowTrans.Controller = "Companies";
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

                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);


                if (string.IsNullOrWhiteSpace(bodyObject.LoginName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if (string.IsNullOrEmpty(bodyObject.OwnerName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.OwnerNameEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.OwnerPhone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.OwnerPhoneEmpty);

                if (string.IsNullOrWhiteSpace(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

                //if (string.IsNullOrWhiteSpace(bodyObject.Password))
                //    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

                //if (string.IsNullOrWhiteSpace(bodyObject.ConfimPassword))
                //    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

                if (string.IsNullOrWhiteSpace(bodyObject.LocationDescriptions))
                    return StatusCode(BackMessages.StatusCode, BackMessages.RecordEmpty + " وصف الموقع");

                if (string.IsNullOrWhiteSpace(bodyObject.LocationLink))
                    return StatusCode(BackMessages.StatusCode, BackMessages.RecordEmpty + " الرابط الخاص بالموقع");


                bodyObject.Phone = bodyObject.Phone.Substring(bodyObject.Phone.Length - 9);
                bodyObject.OwnerPhone = bodyObject.Phone.Substring(bodyObject.Phone.Length - 9);

                //valid input
                if (!help.IsValidEmail(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

                if (!help.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                if (!help.IsValidPhone(bodyObject.OwnerPhone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                //if (!help.IsCorrectPassword(bodyObject.Password, bodyObject.ConfimPassword))
                //    return StatusCode(BackMessages.StatusCode, BackMessages.ConfirmPassword);

                //if (bodyObject.Password.Length < 8)
                //    return StatusCode(BackMessages.StatusCode, BackMessages.PasswordLenght);


                var row = db.Companies.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.Status != 9 && x.User.Status != 9).SingleOrDefault();
                if(row==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var UserOjbect = db.Users.Where(x => x.Id == row.UserId && x.Status != 9).SingleOrDefault();
                if(UserOjbect==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.ItemId = bodyObject.Id;
                rowTrans.OldObject= JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });



                //Is Exist
                var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9 && x.Id!=row.UserId).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                IsExist = db.Users.Where(x => x.Status != 9 && x.Name == bodyObject.Name && x.Id != row.UserId).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                IsExist = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone && x.Id != row.UserId).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

                IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9 && x.Id != row.UserId).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);

                var IsExistCompany = db.Companies.Where(x => x.OwnerPhone == bodyObject.OwnerPhone && x.Status != 9 && x.Id != row.Id).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist1);

                IsExistCompany = db.Companies.Where(x => x.OwnerPhone == bodyObject.OwnerPhone && x.Status != 9 && x.Id != row.Id).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist1);




                UserOjbect.Name = bodyObject.Name;
                UserOjbect.LoginName = bodyObject.LoginName;
                UserOjbect.Phone = bodyObject.Phone;
                UserOjbect.Email = bodyObject.Email;

                row.OwnerName = bodyObject.OwnerName;
                row.OwnerPhone = bodyObject.OwnerPhone;
                row.LocationDescriptions = bodyObject.LocationDescriptions;
                row.LocationLink = bodyObject.LocationLink;
                row.FloorCount = bodyObject.FloorCount;
                row.ClassRoomCount = bodyObject.ClassRoomCount;
                row.MeetingRoomCount = bodyObject.MeetingRoomCount;
                row.TraningRoomCount = bodyObject.TraningRoomCount;
                row.PrivateRoomCount = bodyObject.PrivateRoomCount;
                row.OfficeCount = bodyObject.OfficeCount;

                
                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل  بيانات  ";
                rowTrans.Controller = "Companies";
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

                var Companies = db.Companies.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (Companies == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var row = db.Users.Where(x => x.Id == Companies.UserId && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
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

                var Companies = db.Companies.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (Companies == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var row = db.Users.Where(x => x.Id == Companies.UserId && x.Status != 9).SingleOrDefault();
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

        [HttpPost("{Id}/Activate")]
        public IActionResult Activate(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Companies = db.Companies.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (Companies == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var row = db.Users.Where(x => x.Id == Companies.UserId && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                Wallet wallet = new Wallet();
                wallet.UserId = row.Id;
                wallet.Value = 0;
                wallet.CreatedBy = userId;
                wallet.CreatedOn = DateTime.Now;
                wallet.Status = 1;
                db.Wallet.Add(wallet);


                row.Level = 2;

                db.SaveChanges();

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.CahngeStatus;
                rowTrans.Descriptions = "الموافقة علي شركة    ";
                rowTrans.Controller = "Comapnies";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);


                return Ok(BackMessages.SucessActiveOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/Deactivate")]
        public IActionResult Deactivate(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Companies = db.Companies.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (Companies == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var row = db.Users.Where(x => x.Id == Companies.UserId && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Level = 3;

                db.SaveChanges();

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.CahngeStatus;
                rowTrans.Descriptions = "رفض  شركة    ";
                rowTrans.Controller = "Comapnies";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);


                return Ok(BackMessages.SucessRejectRequest);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/ChangeStatus")]
        public IActionResult ChangeStatus(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Companies = db.Companies.Where(x => x.Status != 9 && x.Id == Id).SingleOrDefault();
                if (Companies == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var row = db.Users.Where(x => x.Id == Companies.UserId && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                if (row.Status == 1)
                {
                    row.Status = 2;
                }
                else if (row.Status == 2)
                {
                    row.Status = 1;
                }

                db.SaveChanges();


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












        //Wallet
        [HttpGet("Wallet/Get")]
        public IActionResult GetWallet(long Id)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Companies = db.Companies.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if(Companies==null)
                    return StatusCode(BackMessages.StatusCode,BackMessages.NotFound);

                var Wallet = db.Wallet.Where(x => x.UserId == Companies.UserId && x.Status != 9).SingleOrDefault();
                if(Wallet==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var Info = db.WalletTransactions
                    .Include(x=>x.PaymentMethod)
                    .Where(x => x.Status != 9 && x.WalletId == Wallet.Id).Select(x => new
                {
                    x.Value,
                    x.PaymentMethod.Name,
                    x.ProcessType,
                    x.CreatedOn,
                    CreatedBy=db.Users.Where(k=>k.Id==x.CreatedBy).SingleOrDefault().Name,
                }).OrderByDescending(x=>x.CreatedOn).ToList();
                
                var WalletPurchases = db.WalletPurchases
                    .Include(x=>x.Subscriptions)
                    .Include(x=>x.Subscriptions.Offer)
                    .Where(x => x.Status != 9 && x.WalletId == Wallet.Id).Select(x => new
                {
                    x.Value,
                    x.Subscriptions.Offer.Name,
                    x.Subscriptions.Start,
                    x.Subscriptions.End,
                    x.SubscriptionsPrice,
                    x.CreatedOn,
                    CreatedBy=db.Users.Where(k=>k.Id==x.CreatedBy).SingleOrDefault().Name,
                }).OrderByDescending(x=>x.CreatedOn).ToList();


                return Ok(new { info = Info, WalletPurchases = WalletPurchases });

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        public class StudentWalletBodyObject
        {
            public long? Id { get; set; }
            public long CompaniesId { get; set; }
            public short PaymentMethodId { get; set; }
            public short ProcessType { get; set; }
            public int Value { get; set; }
        }

        [HttpPost("RechargeWallet")]
        public IActionResult RechargeWallet([FromBody] StudentWalletBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.Value <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ValueEmpty);

                var Companies = db.Companies.Where(x => x.Id == bodyObject.CompaniesId && x.Status != 9).SingleOrDefault();
                if (Companies == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var Wallet = db.Wallet.Where(x => x.UserId == Companies.UserId && x.Status != 9).SingleOrDefault();
                if (Wallet == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


                WalletTransactions row = new WalletTransactions();
                row.WalletId = Wallet.Id;
                row.PaymentMethodId = bodyObject.PaymentMethodId;
                row.Value = bodyObject.Value;
                row.ProcessType = bodyObject.ProcessType;
                row.CreatedOn = DateTime.Now;
                row.CreatedBy = userId;
                row.Status = 1;
                db.WalletTransactions.Add(row);
                Wallet.Value += bodyObject.Value;


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Pay;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "  شحن محفظة   ";
                rowTrans.Controller = "Companies/RechargeWallet";
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

        [HttpPost("{Id}/DeletetWalletTransacitons")]
        public IActionResult DeletetWalletTransacitons(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Info = db.WalletTransactions.Include(x => x.Wallet).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (Info == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var Wallet = db.Wallet.Where(x => x.Id == Info.WalletId && x.Status != 9).SingleOrDefault();
                if (Wallet == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                Info.Status = 9;
                Info.CreatedBy = userId;
                Info.CreatedOn = DateTime.Now;
                Wallet.Value -= Info.Value;

                WalletTransactions row = new WalletTransactions();
                row.WalletId = Wallet.Id;
                row.PaymentMethodId = 1;
                row.Value = Info.Value;
                row.ProcessType = 2;
                row.CreatedOn = DateTime.Now;
                row.CreatedBy = userId;
                row.Status = 1;
                db.WalletTransactions.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "  حذف عملية من حركات الحافظة  ";
                rowTrans.Controller = "Companies/StudentWalletTransactions";
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













        //Subscriptions
        [HttpGet("Subscriptions/Get")]
        public IActionResult GetSubscriptions(int pageNo, int pageSize)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Info = db.Subscriptions
                    .Include(x => x.Offer)
                    .Include(x => x.User)
                    .Where(x => x.Status != 9
                     && (user.UserType != 1 ? x.UserId == userId : true)
                    ).Select(x => new
                    {
                        x.Id,
                        x.User.Name,
                        OfferName=x.Offer.Name,
                        x.Start,
                        x.End,
                        x.PaiedValue,
                        x.RemindValue,
                        x.LastPaymentDate,
                        x.Level,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info });

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }









        //Offers
        public class OfferBodyObject
        {
            public long? Id { get; set; }
            public long CompaniesRoomId { get; set; }
            public string Name { get; set; }
            public string Descriptions { get; set; }
            public short Target { get; set; }
            public short LenthType { get; set; }
            public short MaxLenth { get; set; }
            public short LessLenth { get; set; }
            public int Price { get; set; }
            public int InitialPaymentPrice { get; set; }
        }

        [HttpPost("Offers/Add")]
        public IActionResult AddOffers([FromBody] OfferBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if(string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode,BackMessages.NameEmpty);

                var isExist = db.Offers.Where(x => x.Name == bodyObject.Name && x.CompaniesRoomId == bodyObject.CompaniesRoomId && x.Status != 9).SingleOrDefault();
                if(isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                Offers row = new Offers();
                row.CompaniesRoomId = bodyObject.CompaniesRoomId;
                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                row.Target = bodyObject.Target;
                row.LenthType = bodyObject.LenthType;
                row.MaxLenth = bodyObject.MaxLenth;
                row.LessLenth = bodyObject.LessLenth;
                row.Price = bodyObject.Price;
                row.InitialPaymentPrice = bodyObject.InitialPaymentPrice;
                row.CreatedOn = DateTime.Now;
                row.CreatedBy = userId;
                row.Status = 1;
                db.Offers.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Pay;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "   تقديم عرض   ";
                rowTrans.Controller = "Companies/Offers";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessRequestOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("Offers/Get")]
        public IActionResult GetOffers(int pageNo, int pageSize)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Info = db.Offers
                    .Include(x => x.Subscriptions)
                    .Include(x => x.CompaniesRoom)
                    .Include(x => x.CompaniesRoom.Company)
                    .Include(x => x.CompaniesRoom.Company.User)
                    .Where(x => x.Status != 9
                     && (user.UserType!=1 ? x.CompaniesRoom.Company.UserId == userId : true)
                    ).Select(x => new
                    {
                        x.Id,
                        x.CompaniesRoom.Type,
                        x.CompaniesRoom.Discriptions,
                        CompaniesName= x.CompaniesRoom.Company.User.Name,
                        CompaniesPhone= x.CompaniesRoom.Company.User.Phone,
                        x.Name,
                        x.Descriptions,
                        x.Target,
                        x.LenthType,
                        x.LessLenth,
                        x.MaxLenth,
                        x.Price,
                        x.BookingValue,
                        x.InitialPaymentPrice,
                        x.LastPaymentBefore,
                        x.AcceptedBy,
                        x.AcceptedOn,
                        x.RejectedOn,
                        x.RejectedResone,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info });

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Offers/GetRequest")]
        public IActionResult GetOffersRequest(int pageNo, int pageSize)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Info = db.Offers
                    .Include(x => x.Subscriptions)
                    .Include(x => x.CompaniesRoom)
                    .Include(x => x.CompaniesRoom.Company)
                    .Include(x => x.CompaniesRoom.Company.User)
                    .Where(x => x.Status ==1).Select(x => new
                    {
                        x.Id,
                        x.CompaniesRoom.Type,
                        x.CompaniesRoom.Discriptions,
                        CompaniesName = x.CompaniesRoom.Company.User.Name,
                        CompaniesPhone = x.CompaniesRoom.Company.User.Phone,
                        x.Name,
                        x.Descriptions,
                        x.Target,
                        x.LenthType,
                        x.LessLenth,
                        x.MaxLenth,
                        x.Price,
                        x.BookingValue,
                        x.InitialPaymentPrice,
                        x.LastPaymentBefore,
                        x.AcceptedBy,
                        x.AcceptedOn,
                        x.RejectedOn,
                        x.RejectedResone,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info });

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{id}/Offers/Delete")]
        public IActionResult DeletetOffers(long id)
        {
            try
            {
                if (id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Offers.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "  حذف سجل  ";
                rowTrans.Controller = "Companies/Offers/Delete";
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
        
        [HttpPost("{id}/Offers/Deactivate")]
        public IActionResult DeactivateOffers(long id)
        {
            try
            {
                if (id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Offers.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 4;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "   ايقاف عرض  ";
                rowTrans.Controller = "Companies/Offers/Deactivate";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessBlockOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{id}/Offers/Active")]
        public IActionResult ActiveOffers(long id)
        {
            try
            {
                if (id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Offers.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 2;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "   تفعيل عرض  ";
                rowTrans.Controller = "Companies/Offers/Active";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessActiveOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{id}/Offers/Accept")]
        public IActionResult AcceptOffers(long id)
        {
            try
            {
                if (id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Offers.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.AcceptedOn = DateTime.Now;
                row.AcceptedBy = userId;
                row.Status = 2;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Accept;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "   الموافقة علي عرض  ";
                rowTrans.Controller = "Companies/Offers/Active";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessAcceptedRequest);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{id}/Offers/Reject")]
        public IActionResult RejectOffers(long id)
        {
            try
            {
                if (id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Offers.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.RejectedOn = DateTime.Now;
                row.RejectedBy = userId;
                row.Status = 3;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Reject;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "   رفض  العرض  ";
                rowTrans.Controller = "Companies/Offers/Active";
                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                rowTrans.CreatedBy = userId;
                this.help.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessRejectRequest);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }











        public class CompaniesRoomsObj
        {
            public short Type { get; set; }
            public string Discriptions { get; set; }
            public string Notes { get; set; }
            public List<AttachmentsObj> Attachments { get; set; } = new List<AttachmentsObj>();
        }

        [HttpPost("CompaniesRooms/Add")]
        public IActionResult AddCompaniesRooms([FromBody] CompaniesRoomsObj bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Companies = db.Companies.Where(x => x.UserId == userId && x.Status != 9).SingleOrDefault();
                if(Companies == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                if (bodyObject.Attachments.Count <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.AttachmentEmpty);

                List<CompainesRoomAttachments> ataachmentList = new List<CompainesRoomAttachments>();
                if (bodyObject.Attachments.Count > 0)
                {
                    foreach (var item1 in bodyObject.Attachments)
                    {
                        CompainesRoomAttachments roomAttahcmnet = new CompainesRoomAttachments();
                        roomAttahcmnet.Name = item1.ImageName;
                        roomAttahcmnet.Path = this.help.UploadFile(item1.ImageName, this.help.GetAttachmentType(item1.ImageName), item1.fileBase64);
                        roomAttahcmnet.CreatedBy = userId;
                        roomAttahcmnet.CreatedOn = DateTime.Now;
                        roomAttahcmnet.Status = 1;
                        ataachmentList.Add(roomAttahcmnet);
                    }
                }


                CompaniesRooms row = new CompaniesRooms();
                row.Type = bodyObject.Type;
                row.Discriptions = bodyObject.Discriptions;
                row.Notes = bodyObject.Notes;
                row.CompanyId = Companies.Id;
                row.CompainesRoomAttachments = ataachmentList;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.CompaniesRooms.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "  اضافة غرفة  ";
                rowTrans.Controller = "CompaniesRooms/Add";
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

        [HttpGet("CompaniesRooms/Get")]
        public IActionResult GetCompaniesRooms()
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);
                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Compny = db.Companies.Where(x => x.UserId == userId).SingleOrDefault();
                if (Compny == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                int Count = db.CompaniesRooms
                    .Include(x => x.Company)
                     .Where(x => x.Status != 9
                       && (user.UserType!=1 ? x.CompanyId == Compny.Id : true)
                     ).Count();

                var Info = db.CompaniesRooms
                     .Include(x => x.Company)
                     .Where(x => x.Status != 9
                       && (user.UserType != 1 ? x.CompanyId == Compny.Id : true)
                     ).Select(x => new
                     {
                         x.Id,
                         x.Type,
                         x.Discriptions,
                         x.Notes,
                         MessageAttachments = x.CompainesRoomAttachments.Where(k => k.Status != 9).Select(k => new
                         {
                             k.Id,
                             url = k.Path
                         }).ToList(),
                         x.CreatedOn
                     }).OrderByDescending(x => x.CreatedOn).ToList();


                return Ok(new { info = Info, count = Count });


            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/CompaniesRooms/Delete")]
        public IActionResult CompaniesRoomsDelete(long Id)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.CompaniesRooms.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "CompaniesRooms";
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

                //if (!string.IsNullOrEmpty(bodyObject.ImageName)
                //   && !string.IsNullOrEmpty(bodyObject.ImageType)
                //   && !string.IsNullOrEmpty(bodyObject.fileBase64))
                //    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.ImageType, bodyObject.fileBase64);

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

        public class UserInfo
        {
            public string Password { set; get; }
            public string NewPassword { set; get; }
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

    }
}