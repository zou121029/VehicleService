using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleService.Portal.Data;
using VehicleService.Portal.WebServices;

namespace VehicleService.Portal.Controllers
{
    public class ServiceEmployeeManagementController : BaseController
    {
        #region General

        private JsonResult GetServiceEmployeeList(ServiceEmployeeType type, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MobileService service = new MobileService();

                //Get data from database
                int count = service.GetServiceEmployeesByTypeCount((int)type);
                List<ServiceEmployee> items = service.GetServiceEmployeesByTypePaging((int)type, jtStartIndex, jtPageSize, jtSorting);

                //Return result to jTable
                return Json(new { Result = "OK", Records = items, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private JsonResult CreateServiceEmployee(ServiceEmployeeType type, ServiceEmployee item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                item.Type = (byte)type;

                MobileService service = new MobileService();

                ServiceEmployee newItem = service.AddServiceEmployee(item);
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

        private JsonResult UpdateServiceEmployee(ServiceEmployeeType type, ServiceEmployee item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }
                
                item.Type = (byte)type;

                MobileService service = new MobileService();

                if (service.UpdateServiceEmployee(item))
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

        private JsonResult DeleteServiceEmployee(ServiceEmployeeType type, int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.DeleteServiceEmployee(id))
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

        #region ServiceAgent

        public ActionResult ServiceAgentManagement()
        {
            ViewBag.Title = "管理服务顾问";
            ViewBag.TableHeader = "服务顾问列表";
            ViewBag.Type = ServiceEmployeeType.ServiceAgent;
            return View("~/Views/ServiceEmployeeManagement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetServiceAgentList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            return GetServiceEmployeeList(ServiceEmployeeType.ServiceAgent, jtStartIndex, jtPageSize, jtSorting);
        }

        [HttpPost]
        public JsonResult CreateServiceAgent(ServiceEmployee item)
        {
            return CreateServiceEmployee(ServiceEmployeeType.ServiceAgent, item);
        }

        [HttpPost]
        public JsonResult UpdateServiceAgent(ServiceEmployee item)
        {
            return UpdateServiceEmployee(ServiceEmployeeType.ServiceAgent, item);
        }

        [HttpPost]
        public JsonResult DeleteServiceAgent(int id)
        {
            return DeleteServiceEmployee(ServiceEmployeeType.ServiceAgent, id);
        }

        #endregion

        #region Worker

        public ActionResult WorkerManagement()
        {
            ViewBag.Title = "管理维修工人";
            ViewBag.TableHeader = "维修工人列表";
            ViewBag.Type = ServiceEmployeeType.Worker;
            return View("~/Views/ServiceEmployeeManagement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetWorkerList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            return GetServiceEmployeeList(ServiceEmployeeType.Worker, jtStartIndex, jtPageSize, jtSorting);
        }

        [HttpPost]
        public JsonResult CreateWorker(ServiceEmployee item)
        {
            return CreateServiceEmployee(ServiceEmployeeType.Worker, item);
        }

        [HttpPost]
        public JsonResult UpdateWorker(ServiceEmployee item)
        {
            return UpdateServiceEmployee(ServiceEmployeeType.Worker, item);
        }

        [HttpPost]
        public JsonResult DeleteWorker(int id)
        {
            return DeleteServiceEmployee(ServiceEmployeeType.Worker, id);
        }

        #endregion

        #region Sales

        public ActionResult SalesManagement()
        {
            ViewBag.Title = "管理销售顾问";
            ViewBag.TableHeader = "销售顾问销列表";
            ViewBag.Type = ServiceEmployeeType.Sales;
            return View("~/Views/ServiceEmployeeManagement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetSalesList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            return GetServiceEmployeeList(ServiceEmployeeType.Sales, jtStartIndex, jtPageSize, jtSorting);
        }

        [HttpPost]
        public JsonResult CreateSales(ServiceEmployee item)
        {
            return CreateServiceEmployee(ServiceEmployeeType.Sales, item);
        }

        [HttpPost]
        public JsonResult UpdateSales(ServiceEmployee item)
        {
            return UpdateServiceEmployee(ServiceEmployeeType.Sales, item);
        }

        [HttpPost]
        public JsonResult DeleteSales(int id)
        {
            return DeleteServiceEmployee(ServiceEmployeeType.Sales, id);
        }

        #endregion

        #region InsuranceAgent

        public ActionResult InsuranceAgentManagement()
        {
            ViewBag.Title = "管理保险顾问";
            ViewBag.TableHeader = "保险顾问列表";
            ViewBag.Type = ServiceEmployeeType.InsuranceAgent;
            return View("~/Views/ServiceEmployeeManagement/Index.cshtml");
        }

        [HttpPost]
        public JsonResult GetInsuranceAgentList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            return GetServiceEmployeeList(ServiceEmployeeType.InsuranceAgent, jtStartIndex, jtPageSize, jtSorting);
        }

        [HttpPost]
        public JsonResult CreateInsuranceAgent(ServiceEmployee item)
        {
            return CreateServiceEmployee(ServiceEmployeeType.InsuranceAgent, item);
        }

        [HttpPost]
        public JsonResult UpdateInsuranceAgent(ServiceEmployee item)
        {
            return UpdateServiceEmployee(ServiceEmployeeType.InsuranceAgent, item);
        }

        [HttpPost]
        public JsonResult DeleteInsuranceAgent(int id)
        {
            return DeleteServiceEmployee(ServiceEmployeeType.InsuranceAgent, id);
        }

        #endregion
    }
}
