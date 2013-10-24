using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VehicleService.Portal.Data;

namespace VehicleService.Portal.Models
{
    public class AppointmentModel
    {
        public AppointmentModel()
        {
        }

        public AppointmentModel(Appointment appointment)
        {
            this.ID = appointment.ID;
            this.CustomerId = appointment.CustomerId;
            this.VehicleNumber = appointment.VehicleNumber;
            this.CustomerName = appointment.CustomerName;
            this.PhoneNumber = appointment.PhoneNumber;
            this.Type = appointment.Type;
            this.Comment = appointment.Comment;
            this.Status = appointment.Status;
            this.TimeStamp = appointment.TimeStamp;
            this.ConfirmTime = appointment.ConfirmTime;
            this.CompleteTime = appointment.CompleteTime;
            this.DateTimeText = appointment.DateTimeText;
            this.Date = appointment.DateTimeStart.Date;
            if (appointment.DateTimeStart.TimeOfDay == new TimeSpan(9, 0, 0))
            {
                this.TimeRange = 0;
            }
            else if (appointment.DateTimeStart.TimeOfDay == new TimeSpan(12, 0, 0))
            {
                this.TimeRange = 1;
            }
            else if (appointment.DateTimeStart.TimeOfDay == new TimeSpan(14, 0, 0))
            {
                this.TimeRange = 2;
            }
            else
            {
                this.TimeRange = 3;
            }
        }

        #region Properties

        public int ID { get; set; }

        public int CustomerId { get; set; }

        public string VehicleNumber { get; set; }

        public string CustomerName { get; set; }

        public string PhoneNumber { get; set; }

        public byte Type { get; set; }

        public string Comment { get; set; }

        public DateTime TimeStamp { get; set; }

        public DateTime? ConfirmTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public string DateTimeText { get; set; }

        public DateTime Date { get; set; }

        public int TimeRange { get; set; }

        public byte Status { get; set; }

        #endregion
    }
}