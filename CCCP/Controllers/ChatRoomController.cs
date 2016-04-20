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
                int messageId = ChatRoomService.SaveChatMessage(chatRoomId, senderUserId, message, Convert.ToDateTime(sendDateTime));

                if (uploadData != null && uploadData.Count() > 0)
                {
                    // Get the uploaded file from the Files collection
                    foreach (var file in uploadData)
                    {
                        /*
                        if (httpPostedFile != null)
                        {
                            var path = Path.Combine(Server.MapPath("~/temp"), DateTime.Now.ToString("yyyyMMdd-hhmmss")) + ".jpg";

                            // Save the uploaded file to "UploadedFiles" folder
                            httpPostedFile.SaveAs(path);

                            Uri folder = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Views\Home");
                            Uri file = new Uri(path);

                            var fileSavePath = folder.MakeRelativeUri(file).ToString();
                            var resizeFileSavePath = folder.MakeRelativeUri(new Uri(ResizeImage(path))).ToString();
                        }
                        */
                    }
                }

                return Json(new { result = true, id = messageId /* will be the messageId? */ });
            }
            catch (Exception e)
            {
                return Json(new { result = false, id = 0 });
            }
        }

        
        [HttpGet]
        public JsonResult GetMessageAttachment(int messageId)
        {
            return Json(new { result = true });
        }
    }
}