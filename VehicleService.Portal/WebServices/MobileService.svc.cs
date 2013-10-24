using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VehicleService.Portal.Data;

namespace VehicleService.Portal.WebServices
{
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MobileService : IMobileService
    {
        #region Generic methods

        public List<T> GetAllItems<T>()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.GetTable(typeof(T)).Cast<T>().ToList<T>();
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool DeleteItem<T>(T item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    db.GetTable(typeof(T)).Attach(item);
                    db.GetTable(typeof(T)).DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool InsertItem<T>(T item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    db.GetTable(typeof(T)).InsertOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateItem<T>(T newItem, T orgItem)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    db.GetTable(typeof(T)).Attach(newItem, orgItem);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #region Account Management

        public CustomerUser GetCustomerUserByAccountName(string accountName)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    if (!string.IsNullOrEmpty(accountName))
                    {
                        return Queryable.Single<CustomerUser>(db.CustomerUsers, u => u.AccountName == accountName);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public CustomerUser CustomerUserSignin(string accountName, string password)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    if (!string.IsNullOrEmpty(accountName) && !string.IsNullOrWhiteSpace(password))
                    {
                        return Queryable.Single<CustomerUser>(db.CustomerUsers, u => u.AccountName == accountName && u.Password == password
                            && u.Status == (byte)CustomerUserStatusType.Normal);        // Only active user can sign-in
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public WcfResultType NewCustomerUser(string accountName, string password, string customerName,
            string vehicleNumber, string phoneNumber, bool? gender, string birthday, string address, string email)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    // Input validation
                    if (string.IsNullOrWhiteSpace(accountName) ||
                        string.IsNullOrWhiteSpace(password))
                    {
                        return WcfResultType.InputValidationError;
                    }

                    if (this.GetCustomerUserByAccountName(accountName) == null)
                    {
                        CustomerUser user = new CustomerUser()
                        {
                            AccountName = accountName,
                            Password = password,
                            CustomerName = customerName,
                            VehicleNumber = vehicleNumber,
                            PhoneNumber = phoneNumber,
                            Gender = gender,
                            Address = address,
                            Email = email,
                            Status = (byte)CustomerUserStatusType.Normal,
                        };

                        // Parse birthday
                        DateTime datetimeVal;
                        if (!string.IsNullOrWhiteSpace(birthday))
                        {
                            if (!DateTime.TryParse(birthday, out datetimeVal))
                            {
                                return WcfResultType.InputValidationError;
                            }
                            else
                            {
                                user.Birthday = datetimeVal;
                            }
                        }
                        db.CustomerUsers.InsertOnSubmit(user);
                        db.SubmitChanges();
                        return WcfResultType.Success;
                    }
                    else
                    {
                        return WcfResultType.DuplicationError;
                    }
                }
                catch
                {
                    return WcfResultType.GeneralError;
                }
            }
        }

        public WcfResultType UpdateCustomerUser(string accountName, string password, string customerName,
            string vehicleNumber, string phoneNumber, bool? gender, string birthday, string address, string email)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    // Input validation
                    if (string.IsNullOrWhiteSpace(accountName))
                    {
                        return WcfResultType.InputValidationError;
                    }

                    CustomerUser user = null;
                    try
                    {
                        user = db.CustomerUsers.Single(u => u.AccountName == accountName);
                    }
                    catch { }
                    if (user == null)
                    {
                        return WcfResultType.NotExistError;
                    }
                    else
                    {
                        // If password is not empty, update the password
                        // The old password verification should be done by mobile client
                        if (!string.IsNullOrWhiteSpace(password))
                        {
                            user.Password = password;
                        }

                        // Parse birthday
                        DateTime datetimeVal;
                        if (!string.IsNullOrWhiteSpace(birthday))
                        {
                            if (!DateTime.TryParse(birthday, out datetimeVal))
                            {
                                return WcfResultType.InputValidationError;
                            }
                            else
                            {
                                user.Birthday = datetimeVal;
                            }
                        }
                        else
                        {
                            // Clear the value
                            user.Birthday = null;
                        }

                        // Other properties
                        user.CustomerName = customerName;
                        user.PhoneNumber = phoneNumber;
                        user.VehicleNumber = vehicleNumber;
                        user.Gender = gender;
                        user.Address = address;
                        user.Email = email;
                        
                        // Submit
                        db.SubmitChanges();
                        return WcfResultType.Success;
                    }
                }
                catch
                {
                    return WcfResultType.GeneralError;
                }
            }
        }

        #region Internal

        internal int GetCustomerUserCount()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.CustomerUsers.Count();
                }
                catch
                {
                    return 0;
                }
            }
        }

        internal List<CustomerUser> GetCustomerUserPaging(int startIndex, int count, string sorting)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    IEnumerable<CustomerUser> query = db.CustomerUsers;

                    //Sorting
                    //This ugly code is used just for demonstration.
                    //Normally, Incoming sorting text can be directly appended to an SQL query.
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("ID ASC"))
                    {
                        query = query.OrderBy(p => p.ID);
                    }
                    else if (sorting.Equals("ID DESC"))
                    {
                        query = query.OrderByDescending(p => p.ID);
                    }
                    else if (sorting.Equals("AccountName ASC"))
                    {
                        query = query.OrderBy(p => p.AccountName);
                    }
                    else if (sorting.Equals("AccountName DESC"))
                    {
                        query = query.OrderByDescending(p => p.AccountName);
                    }
                    else if (sorting.Equals("CustomerName ASC"))
                    {
                        query = query.OrderBy(p => p.CustomerName);
                    }
                    else if (sorting.Equals("CustomerName DESC"))
                    {
                        query = query.OrderByDescending(p => p.CustomerName);
                    }
                    else if (sorting.Equals("VehicleNumber ASC"))
                    {
                        query = query.OrderBy(p => p.VehicleNumber);
                    }
                    else if (sorting.Equals("VehicleNumber DESC"))
                    {
                        query = query.OrderByDescending(p => p.VehicleNumber);
                    }
                    else if (sorting.Equals("PhoneNumber ASC"))
                    {
                        query = query.OrderBy(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("PhoneNumber DESC"))
                    {
                        query = query.OrderByDescending(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("Gender ASC"))
                    {
                        query = query.OrderBy(p => p.Gender);
                    }
                    else if (sorting.Equals("Gender DESC"))
                    {
                        query = query.OrderByDescending(p => p.Gender);
                    }
                    else if (sorting.Equals("Birthday ASC"))
                    {
                        query = query.OrderBy(p => p.Birthday);
                    }
                    else if (sorting.Equals("Birthday DESC"))
                    {
                        query = query.OrderByDescending(p => p.Birthday);
                    }
                    else if (sorting.Equals("Address ASC"))
                    {
                        query = query.OrderBy(p => p.Address);
                    }
                    else if (sorting.Equals("Address DESC"))
                    {
                        query = query.OrderByDescending(p => p.Address);
                    }
                    else if (sorting.Equals("Email ASC"))
                    {
                        query = query.OrderBy(p => p.Email);
                    }
                    else if (sorting.Equals("Email DESC"))
                    {
                        query = query.OrderByDescending(p => p.Email);
                    }
                    else if (sorting.Equals("Status ASC"))
                    {
                        query = query.OrderBy(p => p.Status);
                    }
                    else if (sorting.Equals("Status DESC"))
                    {
                        query = query.OrderByDescending(p => p.Status);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.ID); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList() //Paging
                               : query.ToList(); //No paging
                }
                catch
                {
                    return null;
                }
            }
        }

        //internal CustomerUser AddCustomerUser(CustomerUser item)
        //{
        //    using (DataClassesDataContext db = new DataClassesDataContext())
        //    {
        //        try
        //        {
        //            db.CustomerUsers.InsertOnSubmit(item);
        //            db.SubmitChanges();
        //            return item;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //}

        internal bool UpdateCustomerUser(CustomerUser item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    // Copy properties except Password, Status
                    CustomerUser old = db.CustomerUsers.Single(v => v.ID == item.ID);
                    old.AccountName = item.AccountName;
                    old.Address = item.Address;
                    old.Birthday = item.Birthday;
                    old.CustomerName = item.CustomerName;
                    old.Email = item.Email;
                    old.Gender = item.Gender;
                    old.PhoneNumber = item.PhoneNumber;
                    old.VehicleNumber = item.VehicleNumber;

                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal bool DeleteCustomerUser(int id)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    CustomerUser item = db.CustomerUsers.Single(v => v.ID == id);
                    db.CustomerUsers.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // Enable or Disable user
        internal bool ChangeCustomerUserStatus(int id, CustomerUserStatusType status)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    CustomerUser item = db.CustomerUsers.Single(v => v.ID == id);
                    item.Status = (byte)status;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #endregion

        #region Appointment

        /// <summary>
        /// New appointment
        /// </summary>
        /// <param name="vehicleNumber"></param>
        /// <param name="customerName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="dateString"></param>
        /// <param name="timeRange"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public WcfResultType NewAppointment(
            string vehicleNumber, string customerName, string phoneNumber,
            string date, string timeRange, int type, int customerId)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(vehicleNumber) ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(date) ||
                string.IsNullOrWhiteSpace(timeRange) ||
                type < 0 || type > 1)
            {
                return WcfResultType.InputValidationError;
            }

            // Parse date
            DateTime dateVal;
            if (!DateTime.TryParse(date, out dateVal))
            {
                return WcfResultType.InputValidationError;
            }

            // Parse time range
            var times = timeRange.Split('-');
            if (times.Count() < 2)
            {
                return WcfResultType.InputValidationError;
            }
            TimeSpan start;
            if (!TimeSpan.TryParse(times.ElementAt(0), out start))
            {
                return WcfResultType.InputValidationError;
            }
            TimeSpan end;
            if (!TimeSpan.TryParse(times.ElementAt(1), out end))
            {
                return WcfResultType.InputValidationError;
            }
            DateTime dateTimeStart = dateVal.Date.Add(start);
            DateTime dateTimeEnd = dateVal.Date.Add(end);

            // Insert new item
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    Appointment item = new Appointment()
                    {
                        VehicleNumber = vehicleNumber,
                        CustomerName = customerName,
                        PhoneNumber = phoneNumber,
                        DateTimeStart = dateTimeStart,
                        DateTimeEnd = dateTimeEnd,
                        Type = (byte)type,
                        Status = (byte)CustomerRequestStatusType.Normal,
                        TimeStamp = DateTime.Now,
                        CustomerId = customerId,
                        DateTimeText = date + " " + timeRange,
                    };
                    db.Appointments.InsertOnSubmit(item);
                    db.SubmitChanges();
                    return WcfResultType.Success;
                }
                catch
                {
                    return WcfResultType.GeneralError;
                }
            }
        }

        public List<Appointment> GetValidAppointments(int customerId)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.Appointments.Where(v => v.CustomerId == customerId &&
                        v.Status <= (byte)CustomerRequestStatusType.Confirmed).ToList();
                }
                catch
                {
                    return null;
                }
            }
        }

        #region Internal

        internal int GetAppointmentsCount()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.Appointments.Count(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed);
                }
                catch
                {
                    return 0;
                }
            }
        }

        internal IEnumerable<Appointment> GetAppointmentsNotification(DateTime timestamp)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.Appointments.Where(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed && v.TimeStamp > timestamp);
                }
                catch
                {
                    return null;
                }
            }
        }

        internal List<Appointment> GetAppointmentsPaging(int startIndex, int count, string sorting)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    IEnumerable<Appointment> query = db.Appointments.Where(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed);

                    //Sorting
                    //This ugly code is used just for demonstration.
                    //Normally, Incoming sorting text can be directly appended to an SQL query.
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("ID ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.ID);
                    }
                    else if (sorting.Equals("ID DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(v => v.Status).ThenByDescending(p => p.ID);
                    }
                    else if (sorting.Equals("VehicleNumber ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.VehicleNumber);
                    }
                    else if (sorting.Equals("VehicleNumber DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.VehicleNumber);
                    }
                    else if (sorting.Equals("CustomerName ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.CustomerName);
                    }
                    else if (sorting.Equals("CustomerName DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.CustomerName);
                    }
                    else if (sorting.Equals("PhoneNumber ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("PhoneNumber DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("Type ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.Type);
                    }
                    else if (sorting.Equals("Type DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.Type);
                    }
                    else
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.ID); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList() //Paging
                               : query.ToList(); //No paging
                }
                catch
                {
                    return null;
                }
            }
        }

        internal bool UpdateAppointment(Appointment item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    Appointment old = db.Appointments.Single(v => v.ID == item.ID);
                    old.CustomerName = item.CustomerName;
                    old.PhoneNumber = item.PhoneNumber;
                    old.VehicleNumber = item.VehicleNumber;
                    old.Comment = item.Comment;
                    old.ConfirmTime = item.ConfirmTime;
                    old.DateTimeText = item.DateTimeText;
                    old.DateTimeEnd = item.DateTimeEnd;
                    old.DateTimeStart = item.DateTimeStart;
                    old.Status = item.Status;
                    old.Type = item.Type;

                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal bool UpdateAppointmentStatus(int id, CustomerRequestStatusType status)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    Appointment item = db.Appointments.Single(v => v.ID == id);
                    item.Status = (byte)status;
                    item.CompleteTime = DateTime.Now;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #endregion

        #region Rescue

        public WcfResultType NewRescueRequest(double latitude, double longitude, int customerId)
        {
            // Insert new item
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    RescueRequest item = new RescueRequest()
                    {
                        Time = DateTime.Now,
                        Latitude = latitude,
                        Longitude = longitude,
                        Status = (byte)CustomerRequestStatusType.Normal,
                        CustomerId = customerId,
                    };
                    db.RescueRequests.InsertOnSubmit(item);
                    db.SubmitChanges();
                    return WcfResultType.Success;
                }
                catch
                {
                    return WcfResultType.GeneralError;
                }
            }
        }

        #region Internal

        internal int GetRescueRequestsCount()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.RescueRequests.Count(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed);
                }
                catch
                {
                    return 0;
                }
            }
        }

        internal IEnumerable<RescueRequestView> GetRescueRequestsNotification(DateTime timestamp)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.RescueRequestViews.Where(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed && v.Time > timestamp);
                }
                catch
                {
                    return null;
                }
            }
        }

        internal List<RescueRequestView> GetRescueRequestsPaging(int startIndex, int count, string sorting)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    IEnumerable<RescueRequestView> query = db.RescueRequestViews.Where(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed);

                    //Sorting
                    //This ugly code is used just for demonstration.
                    //Normally, Incoming sorting text can be directly appended to an SQL query.
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("ID ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.ID);
                    }
                    else if (sorting.Equals("ID DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(v => v.Status).ThenByDescending(p => p.ID);
                    }
                    else if (sorting.Equals("CustomerName ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.CustomerName);
                    }
                    else if (sorting.Equals("CustomerName DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.CustomerName);
                    }
                    else if (sorting.Equals("PhoneNumber ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("PhoneNumber DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("Time ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.Time);
                    }
                    else if (sorting.Equals("Time DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.Time);
                    }
                    else
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.ID); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList() //Paging
                               : query.ToList(); //No paging
                }
                catch
                {
                    return null;
                }
            }
        }

        internal bool UpdateRescueRequest(RescueRequest item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    RescueRequest old = db.RescueRequests.Single(v => v.ID == item.ID);
                    old.Comment = item.Comment;
                    old.ConfirmTime = item.ConfirmTime;
                    old.Status = item.Status;

                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal bool UpdateRescueRequestStatus(int id, CustomerRequestStatusType status)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    RescueRequest item = db.RescueRequests.Single(v => v.ID == id);
                    item.Status = (byte)status;
                    item.CompleteTime = DateTime.Now;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #endregion

        #region Service Employees

        public List<ServiceEmployee> GetServiceEmployees()
        {
            return this.GetAllItems<ServiceEmployee>();
        }

        public List<ServiceEmployee> GetServiceEmployeesByType(int type)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.ServiceEmployees.Where(v => v.Type == type).ToList();
                }
                catch
                {
                    return null;
                }
            }
        }

        #region Internal

        internal int GetServiceEmployeesByTypeCount(int type)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.ServiceEmployees.Count(v => v.Type == type);
                }
                catch
                {
                    return 0;
                }
            }
        }

        internal List<ServiceEmployee> GetServiceEmployeesByTypePaging(int type, int startIndex, int count, string sorting)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    IEnumerable<ServiceEmployee> query = db.ServiceEmployees.Where(v => v.Type == type);

                    //Sorting
                    //This ugly code is used just for demonstration.
                    //Normally, Incoming sorting text can be directly appended to an SQL query.
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("ID ASC"))
                    {
                        query = query.OrderBy(p => p.ID);
                    }
                    else if (sorting.Equals("ID DESC"))
                    {
                        query = query.OrderByDescending(p => p.ID);
                    }
                    else if (sorting.Equals("Name ASC"))
                    {
                        query = query.OrderBy(p => p.Name);
                    }
                    else if (sorting.Equals("Name DESC"))
                    {
                        query = query.OrderByDescending(p => p.Name);
                    }
                    else if (sorting.Equals("WorkNumber ASC"))
                    {
                        query = query.OrderBy(p => p.WorkNumber);
                    }
                    else if (sorting.Equals("WorkNumber DESC"))
                    {
                        query = query.OrderByDescending(p => p.WorkNumber);
                    }
                    else if (sorting.Equals("PhoneNumber ASC"))
                    {
                        query = query.OrderBy(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("PhoneNumber DESC"))
                    {
                        query = query.OrderByDescending(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("Description ASC"))
                    {
                        query = query.OrderBy(p => p.Description);
                    }
                    else if (sorting.Equals("Description DESC"))
                    {
                        query = query.OrderByDescending(p => p.Description);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.ID); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList() //Paging
                               : query.ToList(); //No paging
                }
                catch
                {
                    return null;
                }
            }
        }

        internal ServiceEmployee AddServiceEmployee(ServiceEmployee item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    db.ServiceEmployees.InsertOnSubmit(item);
                    db.SubmitChanges();
                    return item;
                }
                catch
                {
                    return null;
                }
            }
        }

        internal bool UpdateServiceEmployee(ServiceEmployee item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    ServiceEmployee old = db.ServiceEmployees.Single(v => v.ID == item.ID);
                    old.Name = item.Name;
                    old.WorkNumber = item.WorkNumber;
                    old.PhoneNumber = item.PhoneNumber;
                    old.Description = item.Description;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal bool UpdateServiceEmployeeImageUrl(int id, string url)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    ServiceEmployee item = db.ServiceEmployees.Single(v => v.ID == id);
                    item.PictureUrl = url;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    // Just throw
                    throw;
                }
            }
        }

        internal bool DeleteServiceEmployee(int id)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    ServiceEmployee item = db.ServiceEmployees.Single(v => v.ID == id);
                    db.ServiceEmployees.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #endregion

        #region Rich Messages

        public List<RichMessage> GetRichMessages()
        {
            return this.GetAllItems<RichMessage>();
        }

        public List<RichMessage> GetRichMessagesByType(int type)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.RichMessages.Where(v => v.Type == type).ToList();
                }
                catch
                {
                    return null;
                }
            }
        }

        #region Internal

        internal int GetRichMessagesByTypeCount(int type)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.RichMessages.Count(v => v.Type == type);
                }
                catch
                {
                    return 0;
                }
            }
        }

        internal List<RichMessage> GetRichMessagesByTypePaging(int type, int startIndex, int count, string sorting)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    IEnumerable<RichMessage> query = db.RichMessages.Where(v => v.Type == type);

                    //Sorting
                    //This ugly code is used just for demonstration.
                    //Normally, Incoming sorting text can be directly appended to an SQL query.
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("ID ASC"))
                    {
                        query = query.OrderBy(p => p.ID);
                    }
                    else if (sorting.Equals("ID DESC"))
                    {
                        query = query.OrderByDescending(p => p.ID);
                    }
                    else if (sorting.Equals("Title ASC"))
                    {
                        query = query.OrderBy(p => p.Title);
                    }
                    else if (sorting.Equals("Title DESC"))
                    {
                        query = query.OrderByDescending(p => p.Title);
                    }
                    else if (sorting.Equals("Content ASC"))
                    {
                        query = query.OrderBy(p => p.Content);
                    }
                    else if (sorting.Equals("Content DESC"))
                    {
                        query = query.OrderByDescending(p => p.Content);
                    }
                    else if (sorting.Equals("TimeStamp ASC"))
                    {
                        query = query.OrderBy(p => p.TimeStamp);
                    }
                    else if (sorting.Equals("TimeStamp DESC"))
                    {
                        query = query.OrderByDescending(p => p.TimeStamp);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.ID); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList() //Paging
                               : query.ToList(); //No paging
                }
                catch
                {
                    return null;
                }
            }
        }

        internal RichMessage AddRichMessage(RichMessage item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    db.RichMessages.InsertOnSubmit(item);
                    db.SubmitChanges();
                    return item;
                }
                catch
                {
                    return null;
                }
            }
        }

        internal bool UpdateRichMessage(RichMessage item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    RichMessage old = db.RichMessages.Single(v => v.ID == item.ID);
                    old.Title = item.Title;
                    old.Content = item.Content;
                    old.TimeStamp = DateTime.Now;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal bool UpdateRichMessageImageUrl(int id, string url)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    RichMessage item = db.RichMessages.Single(v => v.ID == id);
                    item.PictureUrl = url;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    // Just throw
                    throw;
                }
            }
        }

        internal bool DeleteRichMessage(int id)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    RichMessage item = db.RichMessages.Single(v => v.ID == id);
                    db.RichMessages.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #endregion

        #region Vehicle Types

        public List<VehicleType> GetVehicleTypes()
        {
            return this.GetAllItems<VehicleType>();
        }

        public List<VehicleSubType> GetVehicleSubTypesByTypeId(int typeID)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.VehicleSubTypes.Where(v => v.VehicleTypeID == typeID).ToList();
                }
                catch
                {
                    return null;
                }
            }
        }

        #region Internal

        internal int GetVehicleTypeCount()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.VehicleTypes.Count();
                }
                catch
                {
                    return 0;
                }
            }
        }

        internal List<VehicleType> GetVehicleTypePaging(int startIndex, int count, string sorting)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    IEnumerable<VehicleType> query = db.VehicleTypes;

                    //Sorting
                    //This ugly code is used just for demonstration.
                    //Normally, Incoming sorting text can be directly appended to an SQL query.
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("ID ASC"))
                    {
                        query = query.OrderBy(p => p.ID);
                    }
                    else if (sorting.Equals("ID DESC"))
                    {
                        query = query.OrderByDescending(p => p.ID);
                    }
                    else if (sorting.Equals("Name ASC"))
                    {
                        query = query.OrderBy(p => p.Name);
                    }
                    else if (sorting.Equals("Name DESC"))
                    {
                        query = query.OrderByDescending(p => p.Name);
                    }
                    else if (sorting.Equals("Description ASC"))
                    {
                        query = query.OrderBy(p => p.Description);
                    }
                    else if (sorting.Equals("Description DESC"))
                    {
                        query = query.OrderByDescending(p => p.Description);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.ID); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList() //Paging
                               : query.ToList(); //No paging
                }
                catch
                {
                    return null;
                }
            }
        }

        internal VehicleType AddVehicleType(VehicleType item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    db.VehicleTypes.InsertOnSubmit(item);
                    db.SubmitChanges();
                    return item;
                }
                catch
                {
                    return null;
                }
            }
        }

        internal bool UpdateVehicleType(VehicleType item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    VehicleType old = db.VehicleTypes.Single(v => v.ID == item.ID);
                    old.Name = item.Name;
                    old.Description = item.Description;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal bool UpdateVehicleTypeImageUrl(int id, string url)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    VehicleType item = db.VehicleTypes.Single(v => v.ID == id);
                    item.PictureUrl = url;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    // Just throw
                    throw;
                }
            }
        }

        internal bool DeleteVehicleType(int id)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    VehicleType item = db.VehicleTypes.Single(v => v.ID == id);
                    db.VehicleTypes.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal VehicleSubType AddVehicleSubType(VehicleSubType item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    db.VehicleSubTypes.InsertOnSubmit(item);
                    db.SubmitChanges();
                    return item;
                }
                catch
                {
                    return null;
                }
            }
        }

        internal bool UpdateVehicleSubType(VehicleSubType item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    db.VehicleSubTypes.Attach(item);
                    db.Refresh(RefreshMode.KeepCurrentValues, item);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal bool DeleteVehicleSubType(int id)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    VehicleSubType item = db.VehicleSubTypes.Single(v => v.ID == id);
                    db.VehicleSubTypes.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #endregion

        #region Driving Test

        public WcfResultType NewDrivingTest(string customerName, string phoneNumber,
            string date, int vehicleTypeId, string comment, int customerId)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(date))
            {
                return WcfResultType.InputValidationError;
            }

            // Parse date
            DateTime dateVal;
            if (!DateTime.TryParse(date, out dateVal))
            {
                return WcfResultType.InputValidationError;
            }

            // Insert new item
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    DrivingTest item = new DrivingTest()
                    {
                        CustomerName = customerName,
                        PhoneNumber = phoneNumber,
                        Date = dateVal,
                        VehicleTypeId = vehicleTypeId,
                        Comment = comment,
                        Status = (byte)CustomerRequestStatusType.Normal,
                        TimeStamp = DateTime.Now,
                        CustomerId = customerId,
                    };
                    db.DrivingTests.InsertOnSubmit(item);
                    db.SubmitChanges();
                    return WcfResultType.Success;
                }
                catch
                {
                    return WcfResultType.GeneralError;
                }
            }
        }

        #region Internal

        internal int GetDrivingTestsCount()
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.DrivingTests.Count(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed);
                }
                catch
                {
                    return 0;
                }
            }
        }

        internal IEnumerable<DrivingTestView> GetDrivingTestsNotification(DateTime timestamp)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    return db.DrivingTestViews.Where(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed && v.TimeStamp > timestamp);
                }
                catch
                {
                    return null;
                }
            }
        }

        internal List<DrivingTestView> GetDrivingTestsPaging(int startIndex, int count, string sorting)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    IEnumerable<DrivingTestView> query = db.DrivingTestViews.Where(v => v.Status <= (byte)CustomerRequestStatusType.Confirmed);

                    //Sorting
                    //This ugly code is used just for demonstration.
                    //Normally, Incoming sorting text can be directly appended to an SQL query.
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("ID ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.ID);
                    }
                    else if (sorting.Equals("ID DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(v => v.Status).ThenByDescending(p => p.ID);
                    }
                    else if (sorting.Equals("VehicleTypeName ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.VehicleTypeName);
                    }
                    else if (sorting.Equals("VehicleTypeName DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.VehicleTypeName);
                    }
                    else if (sorting.Equals("CustomerName ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.CustomerName);
                    }
                    else if (sorting.Equals("CustomerName DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.CustomerName);
                    }
                    else if (sorting.Equals("PhoneNumber ASC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.PhoneNumber);
                    }
                    else if (sorting.Equals("PhoneNumber DESC"))
                    {
                        query = query.OrderBy(v => v.Status).ThenByDescending(p => p.PhoneNumber);
                    }
                    else
                    {
                        query = query.OrderBy(v => v.Status).ThenBy(p => p.ID); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList() //Paging
                               : query.ToList(); //No paging
                }
                catch
                {
                    return null;
                }
            }
        }

        internal bool UpdateDrivingTest(DrivingTest item)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    DrivingTest old = db.DrivingTests.Single(v => v.ID == item.ID);
                    old.VehicleTypeId = item.VehicleTypeId;
                    old.CustomerName = item.CustomerName;
                    old.PhoneNumber = item.PhoneNumber;
                    old.Date = item.Date;
                    old.Comment = item.Comment;
                    old.ConfirmTime = item.ConfirmTime;
                    old.Status = item.Status;

                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal bool UpdateDrivingTestStatus(int id, CustomerRequestStatusType status)
        {
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                try
                {
                    DrivingTest item = db.DrivingTests.Single(v => v.ID == id);
                    item.Status = (byte)status;
                    item.CompleteTime = DateTime.Now;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #endregion
    }
}
