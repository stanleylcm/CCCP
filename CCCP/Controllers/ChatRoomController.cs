using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.IO;
using CCCP.ViewModel;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.Common;
using CCCP.Controllers.WebApi;

namespace CCCP.Controllers
{
    public class ChatRoomController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();

        public ActionResult ChatRoom()
        {
            return PartialView("_ChatRoom");
        }

        [HttpPost]
        public JsonResult SaveChatRoomMessageAttachment(int chatRoomId, int senderUserId, string senderDisplayName, DateTime sendDateTime, string message, IEnumerable<HttpPostedFileBase> uploadData)
        {
            try
            {
                string saveDBDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                int messageId = ChatRoomService.SaveChatMessage(chatRoomId, senderUserId, message, Convert.ToDateTime(saveDBDateTime));

                if (messageId > 0 && uploadData != null && uploadData.Count() > 0)
                {
                    int cnt = 1;

                    // Get the uploaded file from the Files collection
                    foreach (HttpPostedFileBase file in uploadData)
                    {
                        string fileType = Path.GetExtension(file.FileName);

                        if (file != null)
                        {
                            var path = Path.Combine(Server.MapPath("~/temp"), cnt.ToString() + fileType);

                            // Save the uploaded file to "UploadedFiles" folder
                            file.SaveAs(path);
                            
                            byte[] content = { };
                            // convert / compress image and video
                            switch (fileType)
                            {
                                case ".jpg": case ".png":
                                    content = MediaService.ResizeImage(path, fileType);
                                    fileType = ".jpg"; // all image will be compressed to jpg
                                    break;
                                case ".mp4": case ".mov":
                                    content = MediaService.CompressVideo(path, fileType);
                                    fileType = ".mp4"; // all movie will be converted to mp4
                                    break;
                                default:
                                    content = System.IO.File.ReadAllBytes(path);
                                    break;
                            }

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            // save to db...
                            ChatRoomService.SaveChatAttachment(messageId, senderUserId, fileType, content);
                        }

                        cnt++;
                    }
                }

                return Json(new { result = true, id = messageId, sendDateTime = saveDBDateTime });
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string errMsg = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(errMsg, raise);
                    }
                }
                throw raise;
            }
            catch (Exception e)
            {
                return Json(new { result = false, id = 0 });
            }
        }

        [HttpPost]
        public ActionResult GetMessageAttachment(int messageId)
        {
            List<ChatRoomAttachment> attachList = new CCCPDbContext().ChatRoomAttachment.Where(m => m.ChatRoomMessageId == messageId).ToList();
            var jsonResult = Json(new { result = true, attachmentList = attachList });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}