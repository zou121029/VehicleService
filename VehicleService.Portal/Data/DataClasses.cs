namespace VehicleService.Portal.Data
{
    partial class DataClassesDataContext
    {
    }

    public enum WcfResultType
    {
        GeneralError = -1,
        Success = 0,
        InputValidationError = 1,
        DuplicationError = 2,
        NotExistError = 3,
    }

    //public enum GenderType
    //{
    //    Female = 0,
    //    Male = 1,
    //}

    public enum AppointmentType
    {
        Repair = 0,
        Maintenance = 1,
    }

    public enum CustomerRequestStatusType
    {
        Normal = 0,
        Confirmed = 1,
        Finished = 2,
        Canceled = 3,
    }

    public enum CustomerUserStatusType
    {
        Normal = 0,
        Disabled = 1,
    }

    public enum ServiceEmployeeType
    {
        ServiceAgent = 0,
        Worker = 1,
        Sales = 2,
        InsuranceAgent = 3,
    }

    public enum RichMessageType
    {
        News = 0,
        Event = 1,
        Promotion = 2,
        Tips = 3,
    }
}
