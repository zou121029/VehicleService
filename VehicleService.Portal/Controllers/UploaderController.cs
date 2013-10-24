using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleService.Portal.WebServices;

namespace VehicleService.Portal.Controllers
{
    public class UploaderController : BaseController
    {
        #region Upload

        [HttpPost]
        public ActionResult UploadImage(int id, string type)
        {
            // Validate we have a file being posted
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No image provided." }, "text/html");
            }

            // File we want to resize and save.
            var file = Request.Files[0];

            try
            {
                var filename = UploadFile(file, id, type);

                MobileService service = new MobileService();
                if (type == "RichMessage")
                {
                    service.UpdateRichMessageImageUrl(id, filename);
                }
                else if (type == "ServiceEmployee")
                {
                    service.UpdateServiceEmployeeImageUrl(id, filename);
                }
                else if (type == "VehicleType")
                {
                    service.UpdateVehicleTypeImageUrl(id, filename);
                }

                // Return JSON
                return Json(new
                {
                    statusCode = 200,
                    status = "Image uploaded.",
                    file = filename,
                }, "text/html");
            }
            catch (Exception ex)
            {
                // Log using "NLog" NuGet package
                //Logger.ErrorException(ex.ToString(), ex);
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading image.",
                    file = string.Empty,
                    message = ex.Message,
                }, "text/html");
            }
        }

        /// <summary>
        /// Persist the file to disk.
        /// </summary>
        private string UploadFile(HttpPostedFileBase file, int id, string type)
        {
            // Build absolute path
            var relPath = "http://" + Request.ServerVariables["HTTP_HOST"] + "/UserData/" + type + "/" + id.ToString() + "/" + file.FileName;
            var absPath = Server.MapPath("~/UserData/" + type + "/") + id.ToString() + "/";
            var absFileAndPath = absPath + file.FileName;

            // Create directory as necessary and save image on server
            if (!Directory.Exists(absPath))
                Directory.CreateDirectory(absPath);
            file.SaveAs(absFileAndPath);

            // Return relative file path
            return relPath;
        }

        #endregion
    }
}
