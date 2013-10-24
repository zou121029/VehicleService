using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleService.Portal.Models;
using VehicleService.Portal.Data;
using VehicleService.Portal.WebServices;

namespace VehicleService.Portal.Controllers
{
    public class MobileRequestManagementController : BaseController
    {
        #region Appointment

        public ActionResult Appointment()
        {
            ViewBag.Title = "管理响应客户预约维修保养";
            ViewBag.TableHeader = "未完成的客户预约维修保养列表";
            return View();
        }

        [HttpPost]
        public JsonResult GetAppointmentList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MobileService service = new MobileService();

                //Get data from database
                int count = service.GetAppointmentsCount();
                List<Appointment> items = service.GetAppointmentsPaging(jtStartIndex, jtPageSize, jtSorting);

                List<AppointmentModel> modelList = new List<AppointmentModel>();
                foreach (var item in items)
                {
                    modelList.Add(new AppointmentModel(item));
                }

                //Return result to jTable
                return Json(new { Result = "OK", Records = modelList, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateAppointment(AppointmentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                DateTime start, end;
                string dateTimeText;
                if (model.TimeRange == 0)
                {
                    start = model.Date.AddHours(9);
                    end = model.Date.AddHours(12);
                    dateTimeText = model.Date.ToString("yyyy-MM-dd") + " 09:00 - 12:00";
                }
                else if (model.TimeRange == 1)
                {
                    start = model.Date.AddHours(12);
                    end = model.Date.AddHours(14);
                    dateTimeText = model.Date.ToString("yyyy-MM-dd") + " 12:00 - 14:00";
                }
                else if (model.TimeRange == 2)
                {
                    start = model.Date.AddHours(14);
                    end = model.Date.AddHours(16);
                    dateTimeText = model.Date.ToString("yyyy-MM-dd") + " 14:00 - 16:00";
                }
                else
                {
                    start = model.Date.AddHours(16);
                    end = model.Date.AddHours(18);
                    dateTimeText = model.Date.ToString("yyyy-MM-dd") + " 16:00 - 18:00";
                }

                Appointment item = new Appointment()
                {
                    ID = model.ID,
                    CustomerId = model.CustomerId,
                    VehicleNumber = model.VehicleNumber,
                    CustomerName = model.CustomerName,
                    PhoneNumber = model.PhoneNumber,
                    Type = model.Type,
                    Comment = model.Comment,
                    TimeStamp = model.TimeStamp,
                    ConfirmTime = DateTime.Now,
                    CompleteTime = model.CompleteTime,
                    Status = (byte)CustomerRequestStatusType.Confirmed,
                    DateTimeStart = start,
                    DateTimeEnd = end,
                    DateTimeText = dateTimeText,
                };

                MobileService service = new MobileService();

                if (service.UpdateAppointment(item))
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
        public JsonResult FinishAppointment(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.UpdateAppointmentStatus(id, CustomerRequestStatusType.Finished))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法完成关闭预约:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CancelAppointment(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.UpdateAppointmentStatus(id, CustomerRequestStatusType.Canceled))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法完成取消预约:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Rescue

        public ActionResult RescueRequest()
        {
            ViewBag.Title = "管理响应客户救援请求";
            ViewBag.TableHeader = "未完成的客户救援请求列表";
            return View();
        }

        [HttpPost]
        public JsonResult GetRescueRequestList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MobileService service = new MobileService();

                //Get data from database
                int count = service.GetRescueRequestsCount();
                List<RescueRequestView> items = service.GetRescueRequestsPaging(jtStartIndex, jtPageSize, jtSorting);

                //Return result to jTable
                return Json(new { Result = "OK", Records = items, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateRescueRequest(RescueRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                RescueRequest item = new RescueRequest()
                {
                    ID = model.ID,
                    CustomerId = model.CustomerId,
                    Time = model.Time,
                    Comment = model.Comment,
                    ConfirmTime = DateTime.Now,
                    CompleteTime = model.CompleteTime,
                    Status = (byte)CustomerRequestStatusType.Confirmed,
                };

                MobileService service = new MobileService();

                if (service.UpdateRescueRequest(item))
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
        public JsonResult FinishRescueRequest(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.UpdateRescueRequestStatus(id, CustomerRequestStatusType.Finished))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法完成关闭预约:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CancelRescueRequest(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.UpdateRescueRequestStatus(id, CustomerRequestStatusType.Canceled))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法完成取消预约:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region DrivingTest

        public ActionResult DrivingTest()
        {
            ViewBag.Title = "管理响应客户预约试乘试驾";
            ViewBag.TableHeader = "未完成的客户预约试乘试驾列表";

            MobileService service = new MobileService();
            DrivingTestViewModel model = new DrivingTestViewModel(service.GetVehicleTypes());

            return View(model);
        }

        [HttpPost]
        public JsonResult GetDrivingTestList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MobileService service = new MobileService();

                //Get data from database
                int count = service.GetDrivingTestsCount();
                List<DrivingTestView> items = service.GetDrivingTestsPaging(jtStartIndex, jtPageSize, jtSorting);

                //Return result to jTable
                return Json(new { Result = "OK", Records = items, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateDrivingTest(DrivingTestView model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "表单数据错误，请检查输入并重试！" });
                }

                DrivingTest item = new DrivingTest()
                {
                    ID = model.ID,
                    CustomerId = model.CustomerId,
                    VehicleTypeId = model.VehicleTypeId,
                    Date = model.Date,
                    CustomerName = model.CustomerName,
                    PhoneNumber = model.PhoneNumber,
                    Comment = model.Comment,
                    TimeStamp = model.TimeStamp,
                    ConfirmTime = DateTime.Now,
                    CompleteTime = model.CompleteTime,
                    Status = (byte)CustomerRequestStatusType.Confirmed,
                };

                MobileService service = new MobileService();

                if (service.UpdateDrivingTest(item))
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
        public JsonResult FinishDrivingTest(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.UpdateDrivingTestStatus(id, CustomerRequestStatusType.Finished))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法完成关闭预约:Id = " + id });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CancelDrivingTest(int id)
        {
            try
            {
                MobileService service = new MobileService();

                if (service.UpdateDrivingTestStatus(id, CustomerRequestStatusType.Canceled))
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "无法完成取消预约:Id = " + id });
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
