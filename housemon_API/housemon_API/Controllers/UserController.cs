using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using housemon_API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace housemon_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ApiControllerBase
    {
        private readonly Models.PropertyMonitorDbContext dbcontext;

        public UserController(Models.PropertyMonitorDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        //NB: ResponseModel is a generic class that returns an object of format
        /*ResponseModel<Type>{
         *  data = content you want to return : should be of  specied type in <Type>,
         *  messag= prefered custom message to display with response 
         *  status= true if we got what we want(success) false if we didn't(failure)
         * }
         */

        [Route("ping")]
        [HttpGet]
        public string ping() {
            return "pong";
        }

        [Route("create/{userType}")]
        [HttpPost]
        public ResponseModel<string> createUser([FromBody] RegisterUserRequestModel user, string userType)
        {
            try
            {
                //assign usertype specified on url to be passed to role variable

                user.role = userType;
               
                //Check if user with specified user name already exists in landlords,managers and tenants tables
                // this will help avoid creating users with identical usernames for multiple user types

                bool alreadyExists = dbcontext.landlords.FirstOrDefault(u => u.username == user.username) != null
                    || dbcontext.managers.FirstOrDefault(u => u.username == user.username) != null
                    || dbcontext.tenants.FirstOrDefault(u => u.username == user.username) != null;

                //soif that user if found will not create one
                if (alreadyExists)
                {
                    throw new Exception($"Username already exists");
                }

                //if username is not found the we will add to the appropriate table based on role AKA userType
                var currentProperty = dbcontext.properties.FirstOrDefault(p => p.propertyId == user.propertyId);
                if(currentProperty == null)
                {
                    throw new Exception($"A property is required.");
                }
                switch (userType)
                {
                    case "landlord":
                        // Add landlord to landlord table
                        var newLandlord = Converter.MakeUser<Models.Landlord>(user);
                        dbcontext.landlords.Add(newLandlord);
                        // TODO: Add to landlordProperties.
                        dbcontext.landlordProperties.Add(
                            new Models.LandLordProperty {
                                LandLord = newLandlord,
                                Property = currentProperty
                            });
                        break;
                    case "manager":
                        var newManager = Converter.MakeUser<Models.Manager>(user);
                        newManager.propertyId = currentProperty.propertyId;
                        newManager.salary = user.salary;
                        dbcontext.managers.Add(newManager);
                        break;
                    case "tenant":
                        var newTenant = Converter.MakeUser<Models.Tenant>(user);
                        newTenant.propertyId = currentProperty.propertyId;
                        newTenant.rentAmount = user.rentAmount;
                        newTenant.deposit = user.deposit;
                        newTenant.rentDueDate = user.rentDueDate;
                        dbcontext.tenants.Add(newTenant);
                        break;
                    default:
                        //unknown usertype handler
                        return new ResponseModel<string>
                        {
                            data = null,
                            message = $"We do not have a user type of {userType}",
                            status = false
                        };
                }

                //save that user
                dbcontext.SaveChanges();
                return new ResponseModel<string>
                {
                    data = "successfully added user",
                    message = "successfully added user",
                    status = true
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = null,
                    message = "Unable to add user, " + ex.Message,
                    status = true
                };
            }
        }

        //This end point edits the profile of a "User"
        [Route("edit/user/profile/{userId}")]
        [HttpPut]
        public ResponseModel<string> updateuserprofile(String userId, [FromBody] RegisterUserRequestModel newUserDetails)
        {
            try
            {
                //search for user with given Id

                Models.User user = dbcontext.tenants.FirstOrDefault(u => u.userId == userId);
                if(user==null)
                {
                    user = dbcontext.managers.FirstOrDefault(u => u.userId == userId);
                    if(user==null)
                        user = dbcontext.landlords.FirstOrDefault(u => u.userId == userId);
                }
              
                //We will get inside the if statement if we find tenant
                if (user != null)
                {
                    //change existing details with new provided details
                    //this not modify fields which we do not want to modify
                    if (newUserDetails.names != null && newUserDetails.names != "")
                    {
                        user.names = newUserDetails.names;
                    }

                    if (newUserDetails.username != null && newUserDetails.username != "")
                    {
                        user.username = newUserDetails.username;
                    }

                    if (newUserDetails.cellNumber != null && newUserDetails.cellNumber != "")
                    {
                        user.cellNumber = newUserDetails.cellNumber;
                    }

                    user.updatedAt = DateTime.Now;
                    if (newUserDetails.profilePicture != null && newUserDetails.profilePicture != "")
                    {
                        user.profilePicture = newUserDetails.profilePicture;
                    }

                    if (newUserDetails.password != null && newUserDetails.password != "")
                    {
                        user.password = newUserDetails.password;
                    }

                }

                //save our changes to database
                dbcontext.SaveChanges();

                return new ResponseModel<string>
                {
                    data = "successfully updated user details",
                    message = "successfully updated user details",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    data = null,
                    message = "Error updadting user" + ex.Message,
                    status = false
                };
            }
        }


        //This End point will allow a user to login with valid username and password

        [Route("login")]
        [HttpPost]
        public ResponseModel<UserReturnModel> userlogin([FromBody] LoginRequestModel user)
        {
            try
            {
                //Look for user in tenants
                Models.User lookup = dbcontext.tenants.FirstOrDefault(u => u.username == user.username && u.password == user.password);

                if (lookup == null)
                {
                    //we didn't find user in tenats table
                    //Look for user in managers
                    lookup = dbcontext.managers.FirstOrDefault(u => u.username == user.username && u.password == user.password);

                    if (lookup == null)
                    {
                        //we didn't find user in managers table
                        //Look for user in landlords
                        lookup = dbcontext.landlords.FirstOrDefault(u => u.username == user.username && u.password == user.password);
                        if (lookup == null)
                        {
                            //we didn't find user in landloards table
                            //we don't have the specified user in our system
                            throw new Exception("User not found");
                        }
                    }
                }

                return new ResponseModel<UserReturnModel>
                {
                    data = Converter.MakeReturnUser(lookup),
                    message = "successfully logged in",
                    status = true
                };
            }
            catch (Exception err)
            {
                return new ResponseModel<UserReturnModel>
                {
                    data = null,
                    message = "Incorrect user details" + err.Message,
                    status = false
                };
            }

        }


        [Route("get/tenant/by/id/{tenant_id}")]
        [HttpGet]
        public ResponseModel<TenantReturnModel> GetTenantById(string tenant_id)
        {
            try
            {
                //Look tenant by id
                Models.Tenant lookupTenant = dbcontext.tenants.FirstOrDefault(u => u.userId == tenant_id);
                if (lookupTenant == null)
                {
                     throw new Exception("User not found");
                }
                
                Models.Property dbProperty = dbcontext.properties.FirstOrDefault(u => u.propertyId == lookupTenant.propertyId);
                if (dbProperty == null)
                {
                    throw new Exception("This user is not linked to any property....");
                }

                return new ResponseModel<TenantReturnModel>
                {
                    data = Converter.MakeReturnTenant(lookupTenant,dbProperty),
                    message = "success",
                    status = true
                };
            }
            catch (Exception err)
            {
                return new ResponseModel<TenantReturnModel>
                {
                    data = null,
                    message = "An error has occured" + err.Message,
                    status = false
                };
            }

        }

        [Route("get/manager/by/id/{manager_id}")]
        [HttpGet]
        public ResponseModel<ManagerReturnModel> GetManagerById(string manager_id)
        {
            try
            {
                  var  lookupManager = dbcontext.managers.FirstOrDefault(u => u.userId == manager_id);
                    if (lookupManager == null)
                    {  
                            throw new Exception("User not found");
                    }

                Models.Property dbProperty = dbcontext.properties.FirstOrDefault(u => u.propertyId == lookupManager.propertyId);
                if (dbProperty == null)
                {
                    throw new Exception("This user is not linked to any property....");
                }

                return new ResponseModel<ManagerReturnModel>
                {
                    data = Converter.MakeReturnManager(lookupManager, dbProperty),
                    message = "success",
                    status = true
                };
            }
            catch (Exception err)
            {
                return new ResponseModel<ManagerReturnModel>
                {
                    data = null,
                    message = "An error has occured " + err.Message,
                    status = false
                };
            }

        }

        [Route("get/landlord/by/id/{landlord_id}")]
        [HttpGet]
        public ResponseModel<LandLordReturnModel> getLandLord(string landlord_id)
        {
            try
            {
                var lookupLandlord = dbcontext
                    .landlords
                    .Include("landlordProperties.Property")
                    .FirstOrDefault(u => u.userId == landlord_id);
                if (lookupLandlord == null)
                {
                    throw new Exception("User not found");
                }
                
                return new ResponseModel<LandLordReturnModel>
                {
                    data = Converter.MakeReturnLandlord(lookupLandlord),
                    message = "Success",
                    status = true
                };
            }
            catch (Exception err)
            {
                return new ResponseModel<LandLordReturnModel>
                {
                    data = null,
                    message = "An error has occured" + err.Message,
                    status = false
                };
            }

        }

    }
}
