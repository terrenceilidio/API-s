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
{    //[EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : ControllerBase
    {

        //NB: ResponseModel is a generic class that returns an object of format
        /*ResponseModel<Type>{
         *  data = content you want to return : should be of  specied type in <Type>,
         *  messag= prefered custom message to display with response 
         *  status= true if we got what we want(success) false if we didn't(failure)
         * }
         */


        private readonly Models.PropertyMonitorDbContext dbcontext;

        public ManagementController(Models.PropertyMonitorDbContext dbcontext)
        {
            this.dbcontext = dbcontext;   
        } 

       
        //End point to remove landlord from house for instance if he sold his shares of property
        [Route("unlink/landlord/from/property/{property_id}/landlord/{landlord_id}")]
        [HttpPost]
        public ResponseModel<string> unlinkPropertyFromLandlord(string property_id, string landlord_id)
        {
            var landlord = dbcontext.landlords.FirstOrDefault(v => v.userId == landlord_id);
            var property = dbcontext.properties.FirstOrDefault(v => v.propertyId == property_id);

            try
            {
                if (landlord == null) throw new Exception("Land lord not found");
                if (property == null) throw new Exception("Property not found");
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = "",
                    message = ex.Message,
                    status = false
                };
            }

            //Look for the landlord with specified id in the bridging table who's id matchs Id in the property
            var landlordProperties = dbcontext.landlordProperties.Where(v => v.LandLord.userId == landlord.userId && v.Property.propertyId == property.propertyId);

            //Remove the landlord everywhere where we find his Id
            dbcontext.landlordProperties.RemoveRange(landlordProperties);
            dbcontext.SaveChanges();
            return new ResponseModel<string>
            {
                data = $"{landlord.names} was successfully unlinked from {property.address}",
                message = $"{landlord.names} was successfully unlinked from {property.address}",
                status = true
            };
        }

        //This end point will link landlord to a property
        [Route("link/landlord/to/property/{property_id}/landlord/{landlord_id}")]
        [HttpPost]
        public ResponseModel<string> linkPropertyToLandlord(string property_id,string landlord_id)
        {
            //search for the landlord we want to link
            var landlord = dbcontext.landlords.FirstOrDefault(v => v.userId == landlord_id);

            //search for the property we want to link him to
            var property = dbcontext.properties.FirstOrDefault(v => v.propertyId == property_id);

            try
            {
                //throw exception if we don't find landlord and property
                if (landlord == null) throw new Exception("Land lord not found");
                if (property == null) throw new Exception("Property not found");
            }
            catch(Exception ex)
            {
                //return null if we didn't find landlord and property with a status of false for unsuccessfull attempt
                return new ResponseModel<string>
                {
                    data = null,
                    message = ex.Message,
                    status = false
                };
            }
            //on success add landlordId and propertyId to bridging table
            var landlordProperty = new Models.LandLordProperty
            {
                LandLord = landlord,
                Property = property
            };

            //Add and save to database
            dbcontext.landlordProperties.Add(landlordProperty);
            dbcontext.SaveChanges();

            //return landlordProperty,name and adress with success(True) status
            return new ResponseModel<string>
            {
                data = $"{landlord.names} was successfully linked {property.address}",
                message = $"{landlord.names} was successfully linked {property.address}",
                status = true
            };
        }


        [Route("get/property/by/id/{property_id}")]
        [HttpGet]
        public ResponseModel<PropertyReturnModel> getProperty(string property_id)
        {
            try
            {
                var propertyLookup = dbcontext
                    .properties
                    .Include(v => v.tenants)
                    .Include(v => v.managers)
                    .Include(v => v.Leases)
                    .Include(v => v.Complaints)
                    .Include("landLordProperties.Property")
                    .Include("landLordProperties.LandLord")
                    .FirstOrDefault(u => u.landLordProperties.Any(l => l.Property.propertyId == property_id));
                if (propertyLookup == null)
                {
                    throw new Exception("Property not found");
                }

                return new ResponseModel<PropertyReturnModel>
                {
                    data = Converter.MakeReturnProperty(propertyLookup),
                    message = "Success",
                    status = true
                };
            }
            catch (Exception err)
            {
                return new ResponseModel<PropertyReturnModel>
                {
                    data = null,
                    message = "An error has occured" + err.Message,
                    status = false
                };
            }

        }


        [Route("get/property/by/id")]
        [HttpGet]
        public ResponseModel<PropertyReturnModel> getListedProperty(string propertyId)
        {
            try
            {
                var propertyLookup = dbcontext.properties.FirstOrDefault(v => v.propertyId == propertyId);
                
                if (propertyLookup == null)
                {
                    throw new Exception("Property not found");
                }

                return new ResponseModel<PropertyReturnModel>
                {
                    data = Converter.MakeReturnProperty(propertyLookup),
                    message = "Success",
                    status = true
                };
            }
            catch (Exception err)
            {
                return new ResponseModel<PropertyReturnModel>
                {
                    data = null,
                    message = "An error has occured" + err.Message,
                    status = false
                };
            }

        }


        //Create A property
        [Route("create/property")]
        [HttpPost]
        public ResponseModel<PropertyReturnModel> createProperty([FromBody]CreatePropertyRequestModel property)
        {
            try
            {
                var _house = dbcontext.properties.FirstOrDefault(v => v.address == property.Address);
                //we dont add a property that already exists
                if (_house != null)
                {
                    return new ResponseModel<PropertyReturnModel>
                    {
                        data = null,
                        message = $"Property with address {property.Address} already exists",
                        status = false
                    };
                }
                var landlord = dbcontext.landlords.FirstOrDefault(v => v.userId == property.LandlordId);
                if (landlord == null)
                {
                    return new ResponseModel<PropertyReturnModel>
                    {
                        data = null,
                        message = $"LandLord does not exist",
                        status = false
                    };
                }

                var newProperty = new Models.Property
                {
                    address = property.Address,
                    rooms = property.rooms,
                    country = property.country,
                    rules = property.rules,
                    landLordProperties = new List<Models.LandLordProperty>()
                };

                dbcontext.properties.Add(newProperty);
                dbcontext.SaveChanges();

                newProperty.landLordProperties.Add(new Models.LandLordProperty
                {
                    LandLord = landlord,
                    Property = newProperty
                });

                dbcontext.SaveChanges();

                var currentProperty = Converter.MakeReturnProperty(newProperty);
                
                return new ResponseModel<PropertyReturnModel>
                {
                    data = currentProperty,
                    message = "successfully added property",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<PropertyReturnModel>
                {
                    data = null,
                    message = "unable to add property " + ex.Message,
                    status = true
                };
            }
        }

        [Route("delete/tenant/{userId}")]
        [HttpGet]
        public ResponseModel<string> deleteTenant(string userId) {
            try
            {

                var user = dbcontext.tenants.FirstOrDefault(v => v.userId == userId);
                if (user == null)
                {
                    return new ResponseModel<string>
                    {
                        data = null,
                        message = "tenant does not exist at all",
                        status = false
                    };
                }
                else {
                    user.propertyId = null;
                    dbcontext.SaveChanges();
                    return new ResponseModel<string>
                    {
                        data = null,
                        message = "successfully removed tenant",
                        status = true
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = null,
                    message = "An unknown Error has occured " + ex.Message,
                    status = false
                };
            }
        }

        //End point to delete a manager
        [Route("delete/manager/{userId}")]
        [HttpGet]
        public ResponseModel<string> deleteManager(string userId)
        {
            try
            {
                var user = dbcontext.managers.Where(u => u.userId == userId);
                if (user != null)
                {
                    return new ResponseModel<string>
                    {
                        data = null,
                        message = "manager does not exist",
                        status = false
                    };
                }
                else
                {
                    dbcontext.Remove(user);
                    dbcontext.SaveChanges();
                    return new ResponseModel<string>
                    {
                        data = null,
                        message = "successfully removed manager",
                        status = true
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = null,
                    message = "An unknown Error has occured " + ex.Message,
                    status = false
                };
            }
        }

        //End point to delete a property
        [Route("delete/property/{propertyId}/{landlordId}")]
        [HttpGet]
        public ResponseModel<string> deleteProperty(string propertyId, string landlordId)
        {
            try
            {

                var property = dbcontext.properties.FirstOrDefault(p => p.propertyId == propertyId);
                if(property == null)
                {     
                    return new ResponseModel<string>
                    {
                        data = null,
                        message = "property does not exist",
                        status = false
                    };
                }
                else
                {
                    /** This is throwing an exception
                    dbcontext.Remove(property);
                    dbcontext.SaveChanges();
                    **/
                    //Alternatively we can unlink it
                    unlinkPropertyFromLandlord(propertyId, landlordId);
                    return new ResponseModel<string>
                    {
                        data = null,
                        message = "successfully removed property",
                        status = true
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = null,
                    message = "An unknown Error has occured " + ex.Message,
                    status = false
                };
            }
        }

        //Creating a notice
        [Route("create/notice")]
        [HttpPost]
        public ResponseModel<string> createNotice([FromBody]CreateNoticeRequestModel notice)
        {
            try
            {
                var dbNotice = new Models.Notice
                {
                    data = notice.data,
                    endDate = notice.endDate,
                    propertyId = notice.PropertyId,
                    userId = notice.userId
                };

                dbcontext.notices.Add(dbNotice);
                dbcontext.SaveChanges();
                return new ResponseModel<string>
                {
                    data = "Notice saved",
                    message = "Successfully added the notice",
                    status = true
                };

            }
            catch(Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = null,
                    message = "Unable to add a notice " + ex.Message,
                    status = false
                };
            }
        }

        public Models.User getUserById(string userId)
        {
            try
            {
                Models.User user = dbcontext.landlords.FirstOrDefault(v => v.userId == userId);
                if(user == null)
                {
                    user = dbcontext.managers.FirstOrDefault(v => v.userId == userId);
                    if(user == null)
                    {
                        user = dbcontext.tenants.FirstOrDefault(v => v.userId == userId);
                    }
                }
                return user;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        //get property notices
        [Route("get/notices/{userId}")]
        [HttpGet]
        public ResponseModel<List<NoticeReturnModel>> getNotices(string userId)
        {
            try
            {
                var properties = dbcontext.properties
                    .Include(v => v.managers)
                    .Include(v => v.tenants)
                    .Include("landLordProperties.LandLord")
                    .Where(v => v.managers.Any(v => v.userId == userId) || v.tenants.Any(v => v.userId == userId) || v.landLordProperties.Any(v => v.LandLord.userId == userId))
                    .ToList();

                List<Models.Notice> notices = new List<Models.Notice>();
                  foreach (var p in properties) {
                    notices.AddRange(dbcontext.notices.Where(v => v.propertyId == p.propertyId));
                   }
                return new ResponseModel<List<NoticeReturnModel>>
                {   
                    data = notices.Select(dbNotice => Converter.MakeReturnNotice(dbNotice, getUserById(dbNotice.userId))).ToList(),
                    message = "Successfully returned all notices",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<NoticeReturnModel>>
                {
                    data = null,
                    message = "Unable to get notices " + ex.Message,
                    status = false
                };
            }
        }

        //Creating a complaint
        [Route("create/complaint")]
        [HttpPost]
        public ResponseModel<string> createComplaint([FromBody]CreateComplaintRequestModel complaint)
        {
            try
            {
                var tenant = dbcontext.tenants.FirstOrDefault(v => v.userId == complaint.tenantId);
                if (tenant == null || tenant.propertyId == null)
                {
                    throw new Exception("Tenant does not exist/ Tenant has no property");
                }
                var dbComplaint = new Models.Complaint
                {
                    data = complaint.data,
                    dateMade = DateTimeOffset.Now,
                    userId = complaint.tenantId,
                    propertyId = tenant.propertyId
                };
                dbcontext.complaints.Add(dbComplaint);
                dbcontext.SaveChanges();
                return new ResponseModel<string>
                {
                    data = "successfully added complaint",
                    message = "success",
                    status = true
                };

            }
            catch (Exception ex)
            {

                return new ResponseModel<string>
                {
                    data = null,
                    message = "Unable to add a complaint "+ ex.Message,
                    status = false

                };
            }
        }
        //updating a complaint
        [Route("update/complaint/{complaintId}")]
        [HttpPut]
        public ResponseModel<string> updateComplaint(String complaintId)
        {

            try
            {
                //serach for complaint with given Id
                Models.Complaint complaint = dbcontext.complaints.FirstOrDefault(c => c.complaintId == complaintId);
                if (complaint != null)
                {
                    complaint.isResolved = !complaint.isResolved;
                    complaint.dateResolved = DateTime.Now;
                }
                dbcontext.SaveChanges();

                var Name = getUserById(complaint.userId).names;
                var obj = new PushNotification();
                obj.SendPushNotification($"Hi  {Name} ",$"The following complain {complaint.data} has been resolved", "dL5-i9W16Fc:APA91bF-tenV-RQ0fBR-hpEbbRFt_KaWoP-NP-j1WFHg552UtrUuoJADwC0JZhPgYt57QbThyjyR6pgedtOE1uhlQV5Q2dfHnYkMvJIIDu6sXEUWTqmlOdBB_3-cdam2nD8aYnppEiGu");
                return new ResponseModel<string>
                {
                    data = "Successfully updated complaint details",
                    message = "Successfully updated complaint details",
                    status = true
                };
            }
            catch (Exception ex)
            {

                return new ResponseModel<string>
                {
                    data = null,
                    message = "Error updating complaint " + ex.Message,
                    status = false
                };
            }
        }

        //Get all complaints
      [Route("get/complaints/{propertyId}")]
        [HttpGet]
        public ResponseModel<List<ComplaintReturnModel>> getComplaints()
        {
            try
            {
                var LookupComplaint  = dbcontext.complaints.ToList();
                return new ResponseModel<List<ComplaintReturnModel>>
                {
                    data = LookupComplaint.Select(dbComplaint => Converter.MakeReturnComplaint(dbComplaint,getUserById(dbComplaint.userId))).ToList(),
                    message = "List of complaints",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ComplaintReturnModel>>
                {
                    data = null,
                    message = "Unable to load complaints "+ ex.Message,
                    status =false
                };
            }

        }

        //Get all complaints
        [Route("get/complaints/for/{userId}")]
        [HttpGet]
        public ResponseModel<List<ComplaintReturnModel>> getTenantComplaints(string userId)
        {
            try
            {
                var LookupComplaint = dbcontext.complaints.Where(u => u.userId == userId).ToList();
                if (LookupComplaint == null)
                {
                    throw new Exception("Complaint Not Found");
                }
                return new ResponseModel<List<ComplaintReturnModel>>
                {
                    data = LookupComplaint.Select(dbComplaint => Converter.MakeReturnComplaint(dbComplaint, getUserById(dbComplaint.userId))).ToList(),
                    message = "Successfully loaded complaints for house ",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<ComplaintReturnModel>>
                {
                    data = null,
                    message = "Unable to load complaints " + ex.Message,
                    status = false

                };
            }
        }

        //Get complaints for a particular house
        [Route("get/complaints/for/property/{propertyId}")]
        [HttpGet]
        public ResponseModel<List<ComplaintReturnModel>> getHouseComplaints(string propertyId)
        {
            try
            {
                var LookupComplaint = dbcontext.complaints.Where(u => u.propertyId == propertyId).ToList();
                if (LookupComplaint == null) {
                    throw new Exception("Complaint Not Found");
                }
                return new ResponseModel<List<ComplaintReturnModel>>
                {
                    data = LookupComplaint.Select(dbComplaint => Converter.MakeReturnComplaint(dbComplaint, getUserById(dbComplaint.userId))).ToList(),
                    message = "Successfully loaded complaints for house "+ propertyId,
                    status = true
                };
             }
               catch (Exception ex)
            {
                return new ResponseModel<List<ComplaintReturnModel>> 
                {
                    data = null,
                    message = "Unable to load complaints "+ ex.Message,
                    status = false
                
                };
            }
        }


        //send message
        [Route("send/message")]
        [HttpPost]
        public ResponseModel<Models.Chat> sendMessage([FromBody]SendMessageRequestModel chat)
        {
            try
            {
                   var newMessage = new Models.Chat
                {

                     message = chat.message,
                     receiverId = chat.receiverId,
                     senderId =chat.senderId,
                     timeSent = DateTimeOffset.Now

                };

                dbcontext.chats.Add(newMessage);
                dbcontext.SaveChanges();

                 return new ResponseModel<Models.Chat>
                {
                    data = newMessage,
                    message = "message sent ",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Models.Chat>
                {
                    data = null,
                    message = "unable send " + ex.Message,
                    status = true
                };
            }
        }


        //get property notices
        [Route("get/tenants/for/all/properties/{userId}")]
         [HttpGet]
         public ResponseModel<List<TenantReturnModel>> getTenants(string userId)
         {
             try
             {
                var allTenants = new List<TenantReturnModel>();
                 var tenants = dbcontext.properties
                     .Include(v => v.tenants)
                     .Where(v =>  v.landLordProperties.Any(v => v.LandLord.userId == userId))
                     .ToList().Select(v => v.tenants.Select(t=> Converter.MakeReturnTenant(t, v)));
                  tenants.ToList().ForEach(t => allTenants.AddRange(t));
                 return new ResponseModel<List<TenantReturnModel>>
                 {
                     data = allTenants,
                     message = "Successfully returned all tenants",
                     status = true
                 };
             }
             catch (Exception ex)
             {
                 return new ResponseModel<List<TenantReturnModel>>
                 {
                     data = null,
                     message = "Unable to get tenants " + ex.Message,
                     status = false
                 };
             }
         }

        public class XY {
            public int tenantCount { get; set; }
            public int monthIndex { get; set; }
        }

        //get tenants count
        [Route("get/tenants/count/permonth/{propertyId}")]
        [HttpGet]
        public ResponseModel<List<XY>> getTenantsCountPerMonth(string propertyId)
        {
            try
            {
                var allTenants = new List<XY>();
                var tenants = dbcontext.properties
                    .FirstOrDefault(v => v.propertyId == propertyId)
                    .tenants;

                List<XY> obj = new List<XY>();
                tenants.ToList().ForEach(tenant => {
                    var obj2 = obj.FirstOrDefault(v => v.monthIndex == tenant.createdAt.Month);
                    if (obj2 != null) {
                        obj2.tenantCount++;
                    }
                    else
                    {
                        obj.Add(new XY { monthIndex = tenant.createdAt.Month, tenantCount = 1 });
                    }
                });

                return new ResponseModel<List<XY>>
                {
                    data = obj,
                    message = "Successfully returned all tenants",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<XY>>
                {
                    data = null,
                    message = "Unable to get tenants " + ex.Message,
                    status = false
                };
            }
        }


        //get property notices
        [Route("get/all/chats/{senderId}/{recepientId}")]
        [HttpGet]
        public ResponseModel<List<Models.Chat>> getAllChats(string senderId, string receiverId)
        {
            try
            {
                var chats = dbcontext.chats.Where(v => v.senderId == senderId && v.receiverId == receiverId).OrderBy(v => v.timeReceived).ToList();
         
                return new ResponseModel<List<Models.Chat>>
                {
                    data = chats,
                    message = "Successfully returned all tenants",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<Models.Chat>>
                {
                    data = null,
                    message = "Unable to get tenants " + ex.Message,
                    status = false
                };
            }
        }
    }
}
