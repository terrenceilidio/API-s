using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace housemon_API.Helpers
{
    public static class Converter
    {
        public static T MakeUser<T>(RegisterUserRequestModel user) where T : Models.User, new()
        {
            return new T
            {
                names = user.names,
                gender = user.gender,
                idNumber = user.idNumber,
                username = user.username,
                cellNumber = user.cellNumber,
                updatedAt = user.updatedAt,
                profilePicture = user.profilePicture,
                createdAt = DateTimeOffset.Now,
                userId = user.userId,
                password = user.password,
                role = user.role
            };
        }

        public static UserReturnModel MakeReturnUser(Models.User dbUser)
        {
            return new UserReturnModel
            {
                UserId = dbUser.userId,
                Role = dbUser.role
            };
        }

        public static ManagerReturnModel MakeReturnManager(Models.Manager dbManager,Models.Property dbProperty)
        {
            return new ManagerReturnModel
            {
                Name = dbManager.names,
                UserId = dbManager.userId,
                Username = dbManager.username,
                Salary = dbManager.salary,
                Role = dbManager.role,

                Property = new MiniProperty
                {
                    Address = dbProperty.address,
                    rooms = dbProperty.rooms,
                    country = dbProperty.country,
                    PropertyId = dbProperty.propertyId,
                    TotalLandLords = dbProperty.landLordProperties.Count,
                    TotalManagers = dbProperty.managers.Count,
                    TotalTenants = dbProperty.tenants.Count
                }
                // Add more manager fields
            };
        }
        public static TenantReturnModel MakeReturnTenant(Models.Tenant dbTenant,Models.Property dbProperty)
        {
            return new TenantReturnModel
            {
                Name = dbTenant.names,
                UserId = dbTenant.userId,
                Username = dbTenant.username,
                propertyId = dbProperty.propertyId,
                leaseId = dbTenant.leaseId,
                Property = new MiniProperty
                {
                    Address = dbProperty.address,
                    PropertyId = dbProperty.propertyId,
                    TotalLandLords = dbProperty.landLordProperties.Count,
                    TotalManagers = dbProperty.managers.Count,
                    TotalTenants = dbProperty.tenants.Count
                },
                // TODO : Add more tenant fields
                    gender=dbTenant.gender,
                    idNumber=dbTenant.idNumber,
                    cellNumber=dbTenant.cellNumber,
                    createdAt=dbTenant.createdAt,
                    profilePicture=dbTenant.profilePicture,
                    deposit=dbTenant.deposit,
                    rentAmount=dbTenant.rentAmount,
                    Role=dbTenant.role,

    };
        }
        public static LandLordReturnModel MakeReturnLandlord(Models.Landlord dbLandlord)
        {
            return new LandLordReturnModel
            {
                Name = dbLandlord.names,
                UserId = dbLandlord.userId,
                Username = dbLandlord.username,
                Role = dbLandlord.role,
                Properties = dbLandlord.landlordProperties.Select(v => new MiniProperty
                {
                    Address = v.Property.address,
                    rooms = v.Property.rooms,
                    country = v.Property.country,
                    PropertyId = v.Property.propertyId,
                    TotalLandLords = v.Property.landLordProperties.Count,
                    TotalManagers = v.Property.managers.Count,
                    TotalTenants = v.Property.tenants.Count
                }).ToList()
            };
        }

        public static ComplaintReturnModel MakeReturnComplaint(Models.Complaint dbComplaint,Models.User user = null) {
            return new ComplaintReturnModel
            {
                userId = dbComplaint.userId,
                propertyId = dbComplaint.propertyId,
                dateResolved = dbComplaint.dateResolved,
                isResolved = dbComplaint.isResolved,
                dateMade = dbComplaint.dateMade,
                data= dbComplaint.data,
                userName = user != null ? user.names : "",
                complaintId = dbComplaint.complaintId

            };
        }

        public static LeaseReturnModel MakeReturnLease(Models.Lease dbLease)
        {
            return new LeaseReturnModel
            {
                startDate= dbLease.startDate,
                endDate=dbLease.endDate,
                leaseId = dbLease.leaseId,
                propertyId = dbLease.propertyId,
                data = dbLease.data,
            };
        }

        public static NoticeReturnModel MakeReturnNotice(Models.Notice dbNotice,Models.User user)
        {
            return new NoticeReturnModel
            {
                propertyId = dbNotice.propertyId,
                endDate = dbNotice.endDate,
                data = dbNotice.data,
                userName = user != null ? user.names : ""
            };
        }

        public static ChatReturnModel MakeReturnChat(Models.Chat dbMessage) 
        {
            return new ChatReturnModel
            { 
                message = dbMessage.message,
                timeSent = dbMessage.timeSent
            };
        
        }

        public static PropertyReturnModel MakeReturnProperty(Models.Property dbProperty){
            double totalMonthlyIncome = 0;
            Boolean PropertyIsfull = false;
            dbProperty.tenants.Select(v => v.rentAmount).ToList().ForEach(amount => {
                totalMonthlyIncome += amount;
            });
            var RoomsOccupied = dbProperty.tenants.Select(v => MakeReturnTenant(v,dbProperty)).ToList().Count;
            var availableRooms = dbProperty.rooms - RoomsOccupied;
            if (availableRooms <= 0)
            {
                PropertyIsfull = true;
            }
            else {
                PropertyIsfull = false;
            };
            return new PropertyReturnModel
            {
                rooms = dbProperty.rooms,
                RoomsOccupied= RoomsOccupied,
                availableRooms = availableRooms,
                isFull = PropertyIsfull,
                MonthlyIncome = totalMonthlyIncome,
                Address = dbProperty.address,
                LandLords = dbProperty.landLordProperties.Select(v => MakeReturnLandlord(v.LandLord)).ToList(),
                Managers = dbProperty.managers.Select(v => MakeReturnManager(v,dbProperty)).ToList(),
                Tenants = dbProperty.tenants.Select(v => MakeReturnTenant(v,dbProperty)).ToList(),
                Leases = dbProperty.Leases.Select(v => MakeReturnLease(v)).ToList(),
                Complaints = dbProperty.Complaints.Select(v => MakeReturnComplaint(v)).ToList(),
            };
        }
    }

}
