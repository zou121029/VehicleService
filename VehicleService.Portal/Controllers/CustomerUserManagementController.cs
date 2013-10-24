using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleService.Portal.Data;
using VehicleService.Portal.WebServices;

namespace VehicleService.Portal.Controllers
{
    public class CustomerUserManagementController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "管理车主用户信息";
            ViewBag.TableHeader = "车主用户信息列表";
            return View();
        }

        [HttpPost]
        public JsonResult GetCustomerUserList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MobileService service = new MobileService();

                //Get data from database
                int count = service.GetCustomerUserCount();
                List<CustomerUser> items = service.GetCustomerUserPaging(jtStartIndex, jtPageSize, jtSorting);

                //Return result to jTable
                return Json(new { Result = "OK", Records = items, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //[HttpPost]
        //public JsonResult CreateCustomerUser(CustomerUser item)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
        //        }

        //        MobileService service = new MobileService();

        //        item.Status = (byte)CustomerUserStatusType.Normal;

        //        CustomerUser newItem = service.AddCustomerUser(item);
        //        if (newItem != null)
        //        {
        //            return Json(new { Result = "OK", Record = newItem });
        //        }
        //        else
        //        {
        //            return Json(new { Result = "ERROR", Message = "创建新纪录失败，请检查输入并重试！" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        [HttpPost]
        public JsonResult UpdateCustomerUser(CustomerUser item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                MobileService service = new MobileService();

                if (service.UpdateCustomerUser(item))
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
        public JsonResult DeleteCustomerUser(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.DeleteCustomerUser(id))
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

        [HttpPost]
        public JsonResult EnableCustomerUser(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.ChangeCustomerUserStatus(id, CustomerUserStatusType.Normal))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法激活用户:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DisableCustomerUser(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.ChangeCustomerUserStatus(id, CustomerUserStatusType.Disabled))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法激活用户:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
