using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.ViewModel;
using CCCP.Common;
using System.Data;
using System.Data.Entity;

namespace CCCP.Controllers.WebApi
{
    public class GeneralEnquiryApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public GeneralEnquiryModel GetNewGeneralEnquiry()
        {
            return new GeneralEnquiryModel();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public GeneralEnquiryModel GetGeneralEnquiry(int Id)
        {
            GeneralEnquiryModel result = new GeneralEnquiryModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load enquiry details
                result.Entity = db.GeneralEnquiry.Find(Id);

                // load chatroom
                result.Chatroom = new ChatRoomModel(result.Entity.ChatRoomId);

                result.Chatroom.ChatRoomMessagesEntites = (from crMessage in db.ChatRoomMessage
                                                           where crMessage.ChatRoomId.Equals(result.Chatroom.Entity.ChatRoomId)
                                                           orderby crMessage.SendDateTime
                                                           select new ChatRoomMessageModel()
                                                           {
                                                               Entity = crMessage
                                                           }).ToList<ChatRoomMessageModel>();

                foreach (ChatRoomMessageModel crMessage in result.Chatroom.ChatRoomMessagesEntites)
                {
                    crMessage.ChatRoomAttachmentsEntities = (from crAttachment in db.ChatRoomAttachment
                                                             where crAttachment.ChatRoomMessageId.Equals(crMessage.Entity.ChatRoomMessageId)
                                                             select crAttachment).ToList<ChatRoomAttachment>();
                }
            }

            // result
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public GeneralEnquiryModel GetGeneralEnquiry()
        {
            GeneralEnquiryModel result = new GeneralEnquiryModel();
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<GeneralEnquiry> GetGeneralEnquiryList()
        {
            List<GeneralEnquiry> result = new List<GeneralEnquiry>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.GeneralEnquiry.ToList();
            }
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CreateGeneralEnquiry(GeneralEnquiry enquiry)
        {
            CCCPDbContext db = new CCCPDbContext();
            GeneralEnquiryModel generalEnquiry = new GeneralEnquiryModel();
            enquiry.Status = EnquiryStatus.Pending.ToString();
            generalEnquiry.Entity = enquiry;
            generalEnquiry.PrepareSave(PrepareSaveMode.Created);
            enquiry = generalEnquiry.Entity;

            db.GeneralEnquiry.Add(enquiry);
            db.SaveChanges();
            db.usp_GeneralEnquiry_PostCreate(enquiry.GeneralEnquiryId, enquiry.CreatedBy);

            return generalEnquiry.Entity.GeneralEnquiryId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int EditGeneralEnquiry(GeneralEnquiryModel enquiry)
        {
            CCCPDbContext db = new CCCPDbContext();

            // prepare history etc. before save
            if (enquiry.Entity.Status == IncidentStatus.Closed.ToEnumString())
            {
                enquiry.PrepareSave(PrepareSaveMode.Closed);
            }
            else
            {
                enquiry.PrepareSave();
            }

            db.GeneralEnquiry.Attach(enquiry.Entity);

            db.Entry(enquiry.Entity).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                // output validation errors
                Console.WriteLine(sb.ToString());
            }

            return enquiry.Entity.GeneralEnquiryId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int CloseGeneralEnquiry(int id)
        {
            CCCPDbContext db = new CCCPDbContext();

            GeneralEnquiry GeneralEnquiry = db.GeneralEnquiry.Find(id);
            GeneralEnquiry.Status = EnquiryStatus.Closed.ToEnumString();
            db.SaveChanges();

            return GeneralEnquiry.GeneralEnquiryId;
        }
    }
}