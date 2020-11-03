using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Helpers
{
    public class LoginRequestModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class RegisterUserRequestModel
    {
        public string profilePicture { get; set; }
        public DateTimeOffset? updatedAt { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string names { get; set; }
        public string gender { get; set; }
        public string idNumber { get; set; }
        public string? leaseId { get; set; }
        public string username { get; set; }
        public string cellNumber { get; set; }
        public string propertyId { get; set; }
        public int salary { get; set; }
        public DateTimeOffset rentDueDate { get; set; }
        public int rentAmount { get; set; }
        public int deposit { get; set; }
        public string role { get; set; }
    }

    public class CreatePropertyRequestModel
    {
        public string Address { get; set; }
        public string rules { get; set;}
        public int rooms { get; set; }
        public string country { get; set; }
        public string LandlordId { get; set; }
    }

    public class CreateNoticeRequestModel
    {
        public string data { get; set; }
        public string userId { get; set; }
        public DateTimeOffset endDate { get; set; }
        public int NumberOfDaysForNoticeToExist { get; set; }
        public string PropertyId { get; set; }
    }
    public class CreateComplaintRequestModel
    {
        public string tenantId { get; set; }
        public string data { get; set; }
    }


    public class CreateLeaseRequestModel
    {
        public string propertyId { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset endDate { get; set; }
        public string data { get; set; }
    }

    public class AssignLeaseToTenants
    {
        public string leaseId { get; set; }
        public string [] tenantIds{ get; set; }
    }

    public class SendMessageRequestModel
    {
        public string receiverId { get; set; }
        public string senderId { get; set; }
        public string message { get; set; }
        public DateTimeOffset timeSent { get; set; }

    }


}
