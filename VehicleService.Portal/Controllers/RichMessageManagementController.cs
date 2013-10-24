using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleService.Portal.Data;
using VehicleService.Portal.WebServices;

namespace VehicleService.Portal.Controllers
{
    public class RichMessageManagementController : BaseController
    {
        #region General

        private JsonResult GetRichMessageList(RichMessageType type, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MobileService service = new MobileService();

                //Get data from database
                int count = service.GetRichMessagesByTypeCount((int)type);
                List<RichMessage> items = service.GetRichMessagesByTypePaging((int)type, jtStartIndex, jtPageSize, jtSorting);

                //Return result to jTable
                return Json(new { Result = "OK", Records = items, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private JsonResult CreateRichMessage(RichMessageType type, RichMessage item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                item.TimeStamp = DateTime.Now;
                item.Type = (byte)type;

                MobileService service = new MobileService();

                RichMessage newItem = service.AddRichMessage(item);
                if (newItem != null)
                {
                    return Json(new { Result = "OK", Record = newItem });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "创建新纪录失败，请检查输入并重试！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private JsonResult UpdateRichMessage(RichMessageType type, RichMessage item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                item.TimeStamp = DateTime.Now;
                item.Type = (byte)type;

                MobileService service = new MobileService();

                if (service.UpdateRichMessage(item))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法更新记录:Id = " + item.ID + ", Type  = " + type.ToString() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private JsonResult DeleteRichMessage(RichMessageType type, int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.DeleteRichMessage(id))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法删除记录:Id = " + id + ", Type = " + type.ToString() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region News

        public ActionResult NewsManagement()
        {
            ViewBag.Title = "管理发布新闻";
            ViewBag.TableHeader = "已发布的新闻列表";
            ViewBag.Type = RichMessageType.News;
            return View("~/Views/RichMessageManagement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetNewsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            return GetRichMessageList(RichMessageType.News, jtStartIndex, jtPageSize, jtSorting);
        }

        [HttpPost]
        public JsonResult CreateNews(RichMessage item)
        {
            return CreateRichMessage(RichMessageType.News, item);
        }

        [HttpPost]
        public JsonResult UpdateNews(RichMessage item)
        {
            return UpdateRichMessage(RichMessageType.News, item);
        }

        [HttpPost]
        public JsonResult DeleteNews(int id)
        {
            return DeleteRichMessage(RichMessageType.News, id);
        }

        #endregion

        #region Event

        public ActionResult EventManagement()
        {
            ViewBag.Title = "管理发布活动";
            ViewBag.TableHeader = "已发布的活动列表";
            ViewBag.Type = RichMessageType.Event;
            return View("~/Views/RichMessageManagement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetEventList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            return GetRichMessageList(RichMessageType.Event, jtStartIndex, jtPageSize, jtSorting);
        }

        [HttpPost]
        public JsonResult CreateEvent(RichMessage item)
        {
            return CreateRichMessage(RichMessageType.Event, item);
        }

        [HttpPost]
        public JsonResult UpdateEvent(RichMessage item)
        {
            return UpdateRichMessage(RichMessageType.Event, item);
        }

        [HttpPost]
        public JsonResult DeleteEvent(int id)
        {
            return DeleteRichMessage(RichMessageType.Event, id);
        }

        #endregion

        #region Promotion

        public ActionResult PromotionManagement()
        {
            ViewBag.Title = "管理发布促销";
            ViewBag.TableHeader = "已发布的促销列表";
            ViewBag.Type = RichMessageType.Promotion;
            return View("~/Views/RichMessageManagement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetPromotionList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            return GetRichMessageList(RichMessageType.Promotion, jtStartIndex, jtPageSize, jtSorting);
        }

        [HttpPost]
        public JsonResult CreatePromotion(RichMessage item)
        {
            return CreateRichMessage(RichMessageType.Promotion, item);
        }

        [HttpPost]
        public JsonResult UpdatePromotion(RichMessage item)
        {
            return UpdateRichMessage(RichMessageType.Promotion, item);
        }

        [HttpPost]
        public JsonResult DeletePromotion(int id)
        {
            return DeleteRichMessage(RichMessageType.Promotion, id);
        }

        #endregion

        #region Tips

        public ActionResult TipsManagement()
        {
            ViewBag.Title = "管理发布小贴士";
            ViewBag.TableHeader = "已发布的小贴士列表";
            ViewBag.Type = RichMessageType.Tips;
            return View("~/Views/RichMessageManagement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetTipsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            return GetRichMessageList(RichMessageType.Tips, jtStartIndex, jtPageSize, jtSorting);
        }

        [HttpPost]
        public JsonResult CreateTips(RichMessage item)
        {
            return CreateRichMessage(RichMessageType.Tips, item);
        }

        [HttpPost]
        public JsonResult UpdateTips(RichMessage item)
        {
            return UpdateRichMessage(RichMessageType.Tips, item);
        }

        [HttpPost]
        public JsonResult DeleteTips(int id)
        {
            return DeleteRichMessage(RichMessageType.Tips, id);
        }

        #endregion
    }
}
