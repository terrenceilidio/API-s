using System;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using housemon_API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace housemon_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeaseController
    {

        private readonly Models.PropertyMonitorDbContext dbcontext;

        public async Task SendPushNotification(string title, string body, string token)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
            });

            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body

                },
                Token = token
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
        }

        public LeaseController(Models.PropertyMonitorDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [Route("create/Lease/for/{propertyId}")]
        [HttpPost]
        public ResponseModel<string> createLease(string propertyId,[FromBody] CreateLeaseRequestModel lease)
        {
            try
            {
          
               var dbLease = new Models.Lease
                {
                   startDate=lease.startDate,
                    endDate=lease.endDate,
                    data = lease.data,
                    propertyId = propertyId
                };
                dbcontext.leases.Add(dbLease);
                dbcontext.SaveChanges();
                return new ResponseModel<string>
                {
                    data = null,
                    message = "successfully added lease",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = null,
                    message = "unable to add lease " + ex.Message,
                    status = false
                };
            }
        }
        
        [Route("assign/tenants/lease")]
        [HttpPost]
        public ResponseModel<string> updateLease([FromBody]  AssignLeaseToTenants assignedTenants)
        {
            try {
                foreach (string tenantId in assignedTenants.tenantIds) {
                    var tenant = dbcontext.tenants.FirstOrDefault(v => v.userId == tenantId);
                    if(tenant != null)
                    {
                        tenant.leaseId = assignedTenants.leaseId;
                        dbcontext.SaveChanges();
                    }
                    this.SendPushNotification("Hi", "you been assigned a lease log into the mobile app to view", "dL5-i9W16Fc:APA91bF-tenV-RQ0fBR-hpEbbRFt_KaWoP-NP-j1WFHg552UtrUuoJADwC0JZhPgYt57QbThyjyR6pgedtOE1uhlQV5Q2dfHnYkMvJIIDu6sXEUWTqmlOdBB_3-cdam2nD8aYnppEiGu");

                }

                return new ResponseModel<string>
                {
                    data = null,
                    message = "Successfully updated lease details",
                    status = true
                };
            }
            catch(Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = null,
                    message = "Error updating lease " + ex.Message,
                    status = false
                };
            }
   
        }

        [Route("get/lease/by/id/{leaseId}")]
        [HttpGet]
        public ResponseModel<LeaseReturnModel> getListedProperty(string leaseId)
        {
            try
            {
                var leaseLookup = dbcontext.leases.FirstOrDefault(v => v.leaseId == leaseId);

                if (leaseLookup == null)
                {
                    throw new Exception("lease not found");
                }

                return new ResponseModel<LeaseReturnModel>
                {
                    data = Converter.MakeReturnLease(leaseLookup),
                    message = "Success",
                    status = true
                };
            }
            catch (Exception err)
            {
                return new ResponseModel<LeaseReturnModel>
                {
                    data = null,
                    message = "An error has occured" + err.Message,
                    status = false
                };
            }

        }


    }
}
