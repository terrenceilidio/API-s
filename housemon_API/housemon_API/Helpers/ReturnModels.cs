using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Helpers
{

    public class ResponseModel<T>
    {
        public string message { get; set; }
        public Boolean status { get; set; }
        public T data { get; set; }
    }

    public class _baseUser
    {
        public string Name{ get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }
    }

    public class UserReturnModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }

    public class ManagerReturnModel : _baseUser
    {
        public double Salary { get; set; }
        public MiniProperty Property { get; set; }
    }
    public class TenantReturnModel : _baseUser
    {
        //Here we can add more variables we want to return 
        public string leaseId { get; set; }
        public MiniProperty Property { get; set; }
        public string propertyId { get; set; }
        public int rentAmount { get; set; }
        public int deposit { get; set; }
        public DateTimeOffset rentDueDate { get; set; }
        //Added properties
        public string gender { get; set; }
        public string idNumber { get; set; }
        public string cellNumber { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public DateTimeOffset updatedAt { get; set; }
        public string? profilePicture { get; set; }
    }

    public class MiniProperty
    {
        public string PropertyId { get; set; }
        public string Address{ get; set; }
        public int TotalTenants{ get; set; }
        public int rooms { get; set; }
        public string country { get; set; }
        public int TotalManagers{ get; set; }
        public int TotalLandLords{ get; set; }
        public double MonthlyIncome { get; set; }
        public int RoomsOccupied { get; set; }
        public int availableRooms { get; set; }

    }
    public class LandLordReturnModel : _baseUser
    {
        //Here we can add more variables we want to return 
        public List<MiniProperty> Properties { get; set; }
    }

    public class PropertyReturnModel
    {
        public double MonthlyIncome { get; set; }
        public string Address { get; set; }
        public int rooms { get; set; }
        public int RoomsOccupied { get; set; }
        public int availableRooms { get; set; }
        public Boolean isFull { get; set; }
        public List<ManagerReturnModel> Managers{ get; set; }
        public List<LandLordReturnModel> LandLords { get; set; }
        public List<TenantReturnModel> Tenants { get; set; }
        public List<LeaseReturnModel> Leases { get; set; }
        public List<ComplaintReturnModel> Complaints { get; set; }
    }

    public class ComplaintReturnModel
    {
        public string complaintId { get; set; }
        public string userId { get; set; }
        public string userName{ get; set; }
        public string propertyId { get; set; }
        public DateTimeOffset dateMade { get; set; }
        public Boolean isResolved { get; set; }
        public DateTimeOffset dateResolved { get; set; }
        public string data { get; set; }
    }

    public class NoticeReturnModel
    {
        public string propertyId { get; set; }
        public string userName { get; set; }
        public DateTimeOffset endDate { get; set; }
        public int NumberOfDaysForNoticeToExist { get; set; } 
        public string data { get; set; }
       
    }

    public class LeaseReturnModel
    {
        public string leaseId { get; set; }
        public string propertyId { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset endDate { get; set; }
        public string data { get; set; }
    }

    public class ChatReturnModel 
    {

        public string message { get; set; }
        public DateTimeOffset timeSent { get; set; }

    }
}
