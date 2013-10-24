using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace VehicleService.Portal
{
    public class DataSourceNotificationService
    {
        #region Singleton

        private DataSourceNotificationService() { }

        private static DataSourceNotificationService _instance = null;
        public static DataSourceNotificationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataSourceNotificationService();
                    _instance.timer = new Timer(TimerTick, null, 30 * 1000, Timeout.Infinite);
                }
                return _instance;
            }
        }

        #endregion

        #region Timer

        private Timer timer;

        private DateTime timestamp = DateTime.Now;

        private static void TimerTick(object state)
        {
            // Stop timer
            _instance.timer.Dispose();

            // Get timestamp
            _instance.timestamp = DateTime.Now;

            //  Get changes during this period

            // Restart timer
            _instance.timer = new Timer(TimerTick, null, 30 * 1000, Timeout.Infinite);
        }

        #endregion
    }
}