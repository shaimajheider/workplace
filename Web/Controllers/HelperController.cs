using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Vue.Models;
using Web.Services;
using static Web.Services.Helper;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/Helper")]
    public class HelperController : Controller
    {
        private Helper help;

        private readonly LotiContext db;

        public HelperController(LotiContext context)
        {
            this.db = context;
            help = new Helper();
        }



        [AllowAnonymous]
        [HttpGet("Cities/GetAll")]
        public IActionResult GetAllCities()
        {
            try
            {
                var Info = db.Cities
                    .Where(x => x.Status != 9).Select(x => new
                    {
                        x.Id,
                        x.Name,
                    }).OrderByDescending(x => x.Name).ToList();
                return Ok(new { info = Info });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Facilities/GetAll")]
        public IActionResult GetAllFacilities(short CityId)
        {
            try
            {
               
                var Info = db.Facilities
                    .Where(x => x.Status != 9 && x.CityId== CityId).Select(x => new
                    {
                        x.Id,
                        x.Name,
                    }).OrderByDescending(x => x.Name).ToList();
                return Ok(new { info = Info });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }







        public class RegasterBodyObject
        {
            public short ProgramId { get; set; }
            public short CityId { get; set; }
            public short FacilityId { get; set; }
            public string FirstName { get; set; }
            public string FatherName { get; set; }
            public string GrandFatherName { get; set; }
            public string SirName { get; set; }
            public string Nid { get; set; }
            public string Phone { get; set; }
            public DateTime BirthDate { get; set; }
            public short Gender { get; set; }
            public string DoctorName { get; set; }

            public List<AttachmentsObj> Attachments { get; set; } = new List<AttachmentsObj>();

        }

        public class AttachmentsObj
        {
            public string ImageName { get; set; }
            public string ImageType { get; set; }
            public string fileBase64 { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("Regester")]
        public IActionResult Add([FromBody] RegasterBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                //validations
                if(bodyObject.ProgramId!=1 && bodyObject.ProgramId!=2 && bodyObject.ProgramId!=3 &&
                    bodyObject.ProgramId!=4 && bodyObject.ProgramId!=5 && bodyObject.ProgramId!=6)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ProgramIdEmpty);

                var City = db.Cities.Where(x => x.Id == bodyObject.CityId && x.Status != 9).SingleOrDefault();
                if (City == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.CityIdEmpty);
                        
                var Facility = db.Facilities.Where(x => x.Id == bodyObject.FacilityId && x.Status != 9).SingleOrDefault();
                if (Facility == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.FacilityEmpty);

                if (string.IsNullOrEmpty(bodyObject.FirstName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.FirstNameEmpty);

                if (string.IsNullOrEmpty(bodyObject.FatherName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.FatherNameEmpty);

                if (string.IsNullOrEmpty(bodyObject.GrandFatherName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.GrandFatherNameEmpty);
                
                if (string.IsNullOrEmpty(bodyObject.SirName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.SirNameEmpty);

                if (string.IsNullOrEmpty(bodyObject.Nid))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NIDExist);

                if(!this.help.IsValidNID(bodyObject.Nid))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NIDNotValid);

                if (string.IsNullOrEmpty(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

                if (!help.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                if (bodyObject.Gender <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.GenderEmpty);

                if (string.IsNullOrEmpty(bodyObject.DoctorName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.DoctorEmpty);

                if (!help.IsArabhicCharacters(bodyObject.FirstName)
                    || !help.IsArabhicCharacters(bodyObject.FatherName)
                    || !help.IsArabhicCharacters(bodyObject.GrandFatherName)
                    || !help.IsArabhicCharacters(bodyObject.SirName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameArabhicNotValid);

                if(bodyObject.Attachments.Count <=0)
                    return StatusCode(BackMessages.StatusCode,BackMessages.AttachnmentsEmpty);



                //isExist
                string FullName = bodyObject.FirstName + " " + bodyObject.FatherName + " " + bodyObject.GrandFatherName + " " + bodyObject.SirName;
                var IsExistStudent = db.Applications.Where(x => x.FirstName == bodyObject.FirstName && x.FatherName == bodyObject.FatherName 
                    && x.GrandFatherName == bodyObject.GrandFatherName && x.SirName == bodyObject.SirName && x.Status != 9).SingleOrDefault();
                if (IsExistStudent != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                IsExistStudent = db.Applications.Where(x => x.FullName == FullName && x.Status!=9).SingleOrDefault();
                if (IsExistStudent != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);
                
                IsExistStudent = db.Applications.Where(x => x.Nid == bodyObject.Nid && x.Status!=9).SingleOrDefault();
                if (IsExistStudent != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NIDExist);
                
                IsExistStudent = db.Applications.Where(x => x.Phone == bodyObject.Phone && x.Status!=9).SingleOrDefault();
                if (IsExistStudent != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);


                List<ApplicationsAttachments> ApplicationsAttachmentsList = new List<ApplicationsAttachments>();
                if (bodyObject.Attachments.Count > 0)
                {
                    foreach (var item1 in bodyObject.Attachments)
                    {
                        ApplicationsAttachments ApplicationsAttachmentsRow = new ApplicationsAttachments();
                        ApplicationsAttachmentsRow.Name = item1.ImageName;
                        ApplicationsAttachmentsRow.Path = this.help.UploadFile(item1.ImageName, this.help.GetAttachmentType(item1.ImageName), item1.fileBase64);
                        ApplicationsAttachmentsRow.CreatedOn = DateTime.Now;
                        ApplicationsAttachmentsRow.Status = 1;
                        ApplicationsAttachmentsList.Add(ApplicationsAttachmentsRow);
                    }
                }



                Applications row = new Applications();
                row.ApplicationsAttachments=ApplicationsAttachmentsList;
                row.FullName = FullName;
                row.ProgramId = bodyObject.ProgramId;
                row.CityId = bodyObject.CityId;
                row.FacilityId = bodyObject.FacilityId;
                row.FirstName = bodyObject.FirstName;
                row.FatherName = bodyObject.FatherName;
                row.GrandFatherName = bodyObject.GrandFatherName;
                row.SirName = bodyObject.SirName;
                row.Nid=bodyObject.Nid;
                row.Phone=bodyObject.Phone;
                row.BirthDate=bodyObject.BirthDate;
                row.Gender=bodyObject.Gender;
                row.DoctorName=bodyObject.DoctorName;
                row.Levels = 1;
                row.Status = 1;
                row.CreatedOn = DateTime.Now;
                db.Applications.Add(row);
                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }





        //******************************************** Countact Us ********************************************

        public class ContactUsBodyObject
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Mesaage { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("CountactUs")]
        public IActionResult CountactUs([FromBody] ContactUsBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                //validations
                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

                if (string.IsNullOrEmpty(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

                if (!help.IsValidEmail(bodyObject.Email))
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

                if (string.IsNullOrEmpty(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

                if (!help.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);
                
                if (string.IsNullOrEmpty(bodyObject.Mesaage))
                    return StatusCode(BackMessages.StatusCode, BackMessages.MessageEmpty);

                ContactUs row = new ContactUs();
                row.Name = bodyObject.Name;
                row.Phone = bodyObject.Phone;
                row.Email = bodyObject.Email;
                row.Mesaage = bodyObject.Mesaage;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.ContactUs.Add(row);
                db.SaveChanges();
                return Ok(BackMessages.SucessSentMessage);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



    }
}