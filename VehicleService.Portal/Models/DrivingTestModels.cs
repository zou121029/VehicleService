using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VehicleService.Portal.Data;

namespace VehicleService.Portal.Models
{
    public class DrivingTestViewModel
    {
        public DrivingTestViewModel()
        {
        }

        public DrivingTestViewModel(List<VehicleType> vehicleTypes)
        {
            VehicleTypes = vehicleTypes;
        }

        public List<VehicleType> VehicleTypes { get; private set; }
    }
}