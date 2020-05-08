using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using housemon_API.Models;
using Microsoft.AspNetCore.Cors;

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

        private readonly PropertyMonitorDbContext dbcontext;

        public ManagementController(PropertyMonitorDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

       
        //End point to remove landlord from house for instance if he sold his shares of property
        [Route("unlink/landlord/from/property/{property_id}/landlord/{landlord_id}")]
        [HttpPost]
        public ResponseModel<LandLordProperty> unlinkPropertyFromLandlord(string property_id, string landlord_id)
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
                return new ResponseModel<LandLordProperty>
                {
                    data = null,
                    message = ex.Message,
                    status = false
                };
            }

            //Look for the landlord with specified id in the bridging table who's id matchs Id in the property
            var landlordProperties = dbcontext.landlordProperties.Where(v => v.LandLord.userId == landlord.userId && v.Property.propertyId == property.propertyId);

            //Remove the landlord everywhere where we find his Id
            dbcontext.landlordProperties.RemoveRange(landlordProperties);
            dbcontext.SaveChanges();
            return new ResponseModel<LandLordProperty>
            {
                data = null,
                message = $"{landlord.names} was successfully unlinked from {property.address}",
                status = true
            };
        }

        //This end point will link landlord to a property
        [Route("link/landlord/to/property/{property_id}/landlord/{landlord_id}")]
        [HttpPost]
        public ResponseModel<LandLordProperty> linkPropertyToLandlord(string property_id,string landlord_id)
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
                return new ResponseModel<LandLordProperty>
                {
                    data = null,
                    message = ex.Message,
                    status = false
                };
            }
            //on success add landlordId and propertyId to bridging table
            var landlordProperty = new LandLordProperty
            {
                LandLord = landlord,
                Property = property
            };

            //Add and save to database
            dbcontext.landlordProperties.Add(landlordProperty);
            dbcontext.SaveChanges();

            //return landlordProperty,name and adress with success(True) status
            return new ResponseModel<LandLordProperty>
            {
                data = landlordProperty,
                message = $"{landlord.names} was successfully linked {property.address}",
                status = true
            };
        }

        //Create A property
        [Route("create/property")]
        [HttpPost]
        public ResponseModel<Property> creatProperty([FromBody]Property property)
        {
            try
            {
                var _house = dbcontext.properties.FirstOrDefault(v => v.address == property.address);
                //we dont add a property that already exists
                if (_house != null)
                {
                    return new ResponseModel<Property>
                    {
                        data = null,
                        message = $"Property with address {property.address} already exists",
                        status = false
                    };
                }
                dbcontext.properties.Add(property);
                dbcontext.SaveChanges();
                return new ResponseModel<Property>
                {
                    data = property,
                    message = "successfully added property",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Property>
                {
                    data = null,
                    message = "unable to add property " + ex.Message,
                    status = true
                };
            }
        }

        //Method to get all houses link to the specified landlord
        List<Property> getLandlordHouses(string landlord_id)
        {
            var result = dbcontext.landlordProperties.Where(u => u.LandLord.userId == landlord_id).Select(v => v.Property).ToList();
            return result;
        }

        [Route("get/properties/for/{landlord_id}")]
        [HttpGet]
        public ResponseModel<List<Property>> getHouses(string landlord_id)
        {
            try
            { 
                var getLandlord = dbcontext.landlords.FirstOrDefault(u => u.userId == landlord_id);
                //just normal check if the landlord exists
                if (getLandlord != null) {
                    return new ResponseModel<List<Property>>
                    {
                        //data will be houses under landlord_id
                        data = getLandlordHouses(landlord_id),
                        message = "Error getting landlord houses ",
                        status = true
                    };
                }
                return new ResponseModel<List<Property>>
                {
                    data = null,
                    message = "Error getting landlord houses ",
                    status = false
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<Property>>
                {
                    data = null,
                    message = "An unknown Error has occured " + ex.Message,
                    status = false
                };
            }
        }

        [Route("get/tenants/for/{landlord_id}")]
        [HttpGet]
        public ResponseModel<List<Tenant>> getTenants(string landlord_id) {
            try
            {
                var landlordHouses = getLandlordHouses(landlord_id);
                List<Tenant> allTenants = new List<Tenant>();
                //get all a list of tenats in every house linked to landlord_id
                landlordHouses.ForEach(house =>
                {
                    allTenants.AddRange(house.tenants.ToList());
                });
                    return new ResponseModel<List<Tenant>>
                    {
                        data = allTenants,
                        message = "We got " + allTenants.Count + " tenants",
                        status = true
                    };
            }
            catch (Exception ex) {
                return new ResponseModel<List<Tenant>>
                {
                    data = null,
                    message = "An unknown Error has occured " + ex.Message,
                    status = false
                };
            }
        }

        //End point to delete a manager

        [Route("delete/tenant/{userId}")]
        [HttpGet]
        public ResponseModel<string> deleteTenant(string userId) {
            try
            {
              var user = dbcontext.tenants.Where(u=> u.userId == userId);
                if (user != null)
                {
                    return new ResponseModel<string>
                    {
                        data = null,
                        message = "tenant does not exist",
                        status = false
                    };
                }
                else {
                    dbcontext.Remove(user);
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
        [Route("delete/property/{propertyId}")]
        [HttpGet]
        public ResponseModel<string> deleteProperty(string propertyId)
        {
            try
            {

                var property = dbcontext.properties.Where(p => p.propertyId == propertyId);
                if(property != null)
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
                    dbcontext.Remove(property);
                    dbcontext.SaveChanges();
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

        //general get all tenats endpoint : This is for testing purposes 

        [Route("get/all/users")]
        [HttpGet]
        public ResponseModel<List<Tenant>> getAllusers()
        {
            try {
                var users = dbcontext.tenants.ToList();
                return new ResponseModel<List<Tenant>>
                {
                    data = users,
                    message = "list of users",
                    status = true
                };
            }
            catch (Exception ex) { 
                return new ResponseModel<List<Tenant>>
                {
                      data= null,
                      message="unable to load users " + ex.Message,
                      status=false
                };
            }
        }
    }
}
