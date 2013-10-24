using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleService.Portal.Data;
using VehicleService.Portal.WebServices;

namespace VehicleService.Portal.Controllers
{
    public class VehicleTypeManagementController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "管理销售车型";
            ViewBag.TableHeader = "销售车型列表";
            return View("~/Views/VehicleTypeManagement/Index.cshtml");
        }

        #region Vehicle Type

        [HttpPost]
        public JsonResult GetVehicleTypeList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MobileService service = new MobileService();

                //Get data from database
                int count = service.GetVehicleTypeCount();
                List<VehicleType> items = service.GetVehicleTypePaging(jtStartIndex, jtPageSize, jtSorting);

                //Return result to jTable
                return Json(new { Result = "OK", Records = items, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CreateVehicleType(VehicleType item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                MobileService service = new MobileService();

                VehicleType newItem = service.AddVehicleType(item);
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

        [HttpPost]
        public JsonResult UpdateVehicleType(VehicleType item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                MobileService service = new MobileService();

                if (service.UpdateVehicleType(item))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法更新记录:Id = " + item.ID });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteVehicleType(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.DeleteVehicleType(id))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法删除记录:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Vehicle Sub Type

        [HttpPost]
        public JsonResult GetVehicleSubTypeList(int vehicleTypeId)
        {
            try
            {
                MobileService service = new MobileService();

                //Get data from database
                List<VehicleSubType> items = service.GetVehicleSubTypesByTypeId(vehicleTypeId);

                //Return result to jTable
                return Json(new { Result = "OK", Records = items });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CreateVehicleSubType(VehicleSubType item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                MobileService service = new MobileService();

                VehicleSubType newItem = service.AddVehicleSubType(item);
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

        [HttpPost]
        public JsonResult UpdateVehicleSubType(VehicleSubType item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                MobileService service = new MobileService();

                if (service.UpdateVehicleSubType(item))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法更新记录:Id = " + item.ID });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteVehicleSubType(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.DeleteVehicleSubType(id))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法删除记录:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion
    }
}
