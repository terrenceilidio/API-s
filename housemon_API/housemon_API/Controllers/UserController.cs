using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using housemon_API.Models;
using Microsoft.AspNetCore.Cors;

namespace housemon_API.Controllers
{
    //[EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly PropertyMonitorDbContext dbcontext;

        public UserController(PropertyMonitorDbContext dbcontext)
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

        [Route("create/{userType}/user")]
        [HttpPost]
        public ResponseModel<User> createUser([FromBody] User user, string userType)
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

                switch (userType)
                {
                    case "landlord":
                        dbcontext.landlords.Add(user.makeUser<Landlord>());
                        break;
                    case "manager":
                        dbcontext.managers.Add(user.makeUser<Manager>());
                        break;
                    case "tenant":
                        dbcontext.tenants.Add(user.makeUser<Tenant>());
                        break;
                    default:
                        //unknown usertype handler
                        return new ResponseModel<User>
                        {
                            data = null,
                            message = $"We do not have a user type of {userType}",
                            status = false
                        };
                }

                //save that user
                dbcontext.SaveChanges();
                return new ResponseModel<User>
                {
                    data = user,
                    message = "successfully added user",
                    status = true
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel<User>
                {
                    data = null,
                    message = "Unable to add user, " + ex.Message,
                    status = true
                };
            }
        }

        //This end point edits the profile of a "TENANT"
        [Route("edit/user/profile/{_Id}")]
        [HttpPost]
        public ResponseModel<User> updateuserprofile(String userId, [FromBody] User newUserDetails)
        {
            try
            {
                //search for tenant with given Id
                var user = dbcontext.tenants.FirstOrDefault(u => u.userId == userId);

                //We will get inside the if statement if we find tenant
                if (user != null)
                {
                    //change existing details with new provided details
                    //this not modify fields which we do not want to modify

                    user.names = newUserDetails.names;
                    user.gender = newUserDetails.gender;
                    user.idNumber = newUserDetails.idNumber;
                    user.username = newUserDetails.username;
                    user.cellNumber = newUserDetails.cellNumber;
                    user.updatedAt = new DateTime();
                    user.updatedAt = newUserDetails.updatedAt;
                    user.profilePicture = newUserDetails.profilePicture;
                }

                //save our changes to database
                dbcontext.SaveChanges();

                return new ResponseModel<User>
                {
                    data = user,
                    message = "successfully logged in",
                    status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<User>
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
        public ResponseModel<User> userlogin([FromBody] User user)
        {
            try
            {
                //Every user inherits from user and for login we use varibles that every user has
                //which is why we are using User model  for assignment of user found

                user = null;

                //Look for user in tenants
                user = dbcontext.tenants.FirstOrDefault(u => u.username == user.username && u.password == user.password);

                if (user == null)
                {
                    //we didn't find user in tenats table
                    //Look for user in managers
                    user = dbcontext.managers.FirstOrDefault(u => u.username == user.username && u.password == user.password);

                    if (user == null)
                    {
                        //we didn't find user in managers table
                        //Look for user in landlords
                        user = dbcontext.landlords.FirstOrDefault(u => u.username == user.username && u.password == user.password);
                        if (user == null)
                        {
                            //we didn't find user in landloards table
                            //we don't have the specified user in oue system
                            throw new Exception("User not found");
                        }
                    }
                }

                return new ResponseModel<User>
                {
                    data = user,
                    message = "successfully logged in",
                    status = true
                };
            }
            catch (Exception err)
            {
                return new ResponseModel<User>
                {
                    data = null,
                    message = "Incorrect user details" + err.Message,
                    status = false
                };
            }

        }

    }
}
