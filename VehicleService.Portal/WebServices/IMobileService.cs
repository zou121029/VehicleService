using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using VehicleService.Portal.Data;

namespace VehicleService.Portal.WebServices
{
    [ServiceContract]
    public interface IMobileService
    {
        #region Generic

        List<T> GetAllItems<T>();

        bool DeleteItem<T>(T item);

        bool InsertItem<T>(T item);

        bool UpdateItem<T>(T newItem, T orgItem);

        #endregion

        #region Account Management

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetCustomerUserByAccountName")]
        CustomerUser GetCustomerUserByAccountName(string accountName);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "CustomerUserSignin")]
        CustomerUser CustomerUserSignin(string accountName, string password);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NewCustomerUser")]
        WcfResultType NewCustomerUser(string accountName, string password, string customerName,
            string vehicleNumber, string phoneNumber, bool? gender, string birthday, string address, string email);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "UpdateCustomerUser")]
        WcfResultType UpdateCustomerUser(string accountName, string password, string customerName,
            string vehicleNumber, string phoneNumber, bool? gender, string birthday, string address, string email);

        #endregion

        #region Appointment

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NewAppointment")]
        WcfResultType NewAppointment(string vehicleNumber, string customerName, string phoneNumber,
            string date, string timeRange, int type, int customerId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetValidAppointments")]
        List<Appointment> GetValidAppointments(int customerId);

        #endregion

        #region Rescue

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NewRescueRequest")]
        WcfResultType NewRescueRequest(double latitude, double longitude, int customerId);

        #endregion

        #region Service Employees

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetServiceEmployees")]
        List<ServiceEmployee> GetServiceEmployees();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetServiceEmployeesByType")]
        List<ServiceEmployee> GetServiceEmployeesByType(int type);

        #endregion

        #region Rich Messages

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRichMessages")]
        List<RichMessage> GetRichMessages();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetRichMessagesByType")]
        List<RichMessage> GetRichMessagesByType(int type);

        #endregion

        #region Vehicle Types

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetVehicleTypes")]
        List<VehicleType> GetVehicleTypes();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetVehicleSubTypesByTypeId")]
        List<VehicleSubType> GetVehicleSubTypesByTypeId(int typeID);

        #endregion

        #region Driving Test

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "NewDrivingTest")]
        WcfResultType NewDrivingTest(string customerName, string phoneNumber,
            string date, int vehicleTypeId, string comment, int customerId);

        #endregion
    }
}
