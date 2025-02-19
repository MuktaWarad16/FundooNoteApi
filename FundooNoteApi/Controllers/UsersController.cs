using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLayer;
using CommonLayer.Models;
using ManagerLayer;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Configuration;
using MSMQ.Messaging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;


namespace FundooNotesApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {


        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;
        private readonly IUserManager manager;

        public UsersController(IUserManager manager, FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
            this.manager = manager;
        }
        [HttpPost]
        [Route("Reg")]
        public IActionResult Register(RegisterModel model)
        {
            var checkEmail = manager.MailExists(model.Email);
            if (checkEmail)
            {
                return BadRequest(new ResponseModel<bool> { success = true, message = "email already exists" });
            }
            else
            {
                var result = manager.Registration(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<Users> { success = true, message = "register successful", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<Users> { success = false, message = "registration failed" });
                }
            }


        }
        [HttpPost]
        [Route("Login")]

        public IActionResult UserLogin(LoginModel model)
        {
            var response = manager.Login(model);
            if (response != null)
            {
                return Ok(new ResponseModel<string> { success = true, message = "login successful", Data = response.ToString() });
            }
            return BadRequest(new ResponseModel<string> { success = false, message = "login failed" });
        }


        //[HttpGet("ForgotPassword")]
        //public async Task<IActionResult> ForgetPassword(string email)
        //{
        //    try
        //    {

        //        ForgotPasswordModel forgotPasswordmodel = new ForgotPasswordModel();
        //        Send send = new Send();
        //        send.SendEmail(forgotPasswordmodel.EmailId, forgotPasswordmodel.Token);
        //        Uri uri = new Uri("rabbitmq://localhost/FundooNotesEmailQueue");
        //        var endPoint = await bus.GetSendEndpoint(uri);
        //        await endPoint.Send(forgotPasswordmodel);
        //        return Ok(new ResponseModel<string> { success = true, message = "mail sent successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { success = false, message = "please provide valid email", });
        //    }
        //}

        
        [HttpPost]
        [Route("ForgetPassword")]

        public IActionResult ForgetPassword(string Email)
        {
            try
            {
                var result = manager.ForgetPassword(Email);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Email sent Successful" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Reset Email not Sent" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [Authorize]
        [HttpPost]
        [Route("ResetLink")]

        public IActionResult ResetLink(string password, string confirmPassword)
        {
            try
            {
                var Email = User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

                var result = manager.ResetLink(Email, password, confirmPassword);

                if (result)
                {
                    return Ok(new { success = true, message = "Reset Password Successful" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Reset Password not Sent" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        //----------------

       // [Authorize]
        //[HttpGet]
        //[Route("GetAllUsers")]
        //public IActionResult GetAllUsers()
        //{

        //    List<Users> users = new List<Users>();

        //    var response = context.Users.Select(x => x);


        //    foreach (var u in users)
        //    {
        //        users.Add(u);

        //    }

        //    if (response != null)
        //    {
        //        return Ok(new { success = true, message = "found all users" });
        //    }
        //    else
        //    {
        //        return BadRequest(new { success = false, message = "could not get all users" });

        //    }


        //}


        //[Authorize]
        //[HttpGet]
        //[Route("GetUserById")]
        //public IActionResult GetUserById(int id)
        //{
        //    var users = context.Users.FirstOrDefault(x => x.UserId == id);

        //    if (users != null)
        //    {
        //        return Ok(new { success = true, message = "got the user by id" });

        //    }
        //    else
        //    {
        //        return BadRequest(new { success = false, message = "could not find the user" });

        //    }
        //}




        //[Authorize]
        //[HttpGet]
        //[Route("GetUsersByNameStartsWithA")]
        //public IActionResult GetUsersByNameStartsWithA()
        //{
        //    try
        //    {

        //        var users = context.Users.Where(u => u.FirstName.StartsWith("A")).ToList();


        //        if (users != null)
        //        {
        //            return Ok(new { success = true, message = "names starting with a are" });

        //        }
        //        else
        //        {
        //            return BadRequest(new { success = false, message = "could not find such records" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("GetTotalUserCount")]
        //public IActionResult GetTotalUserCount()
        //{
        //    var users = manager.GetTota
        //    var userCount = users.Count();

        //    if (userCount >= 0)
        //    {
        //        return Ok(new { success = true, message = "Total number of users", data = userCount });
        //    }
        //    else
        //    {
        //        return BadRequest(new { success = false, message = "Could not count users" });
        //    }


        //}

        //[Authorize]
        //[HttpGet]
        //[Route("GetAllUsersOrderedByName")]
        //public IActionResult GetAllUsersOrderedByName()
        //{

        //    List<Users> userlist = new List<Users>();
        //    var ascOrder = context.Users.OrderBy(x => x.FirstName).ToList();


        //    if (ascOrder != null)
        //    {
        //        return Ok(new { success = true, message = "Users ordered by name" });
        //    }
        //    else
        //    {
        //        return BadRequest(new { success = false, message = "No users found" });
        //    }


        //}

        //[Authorize]
        //[HttpGet]
        //[Route("GetAverageAgeOfUsers")]
        //public IActionResult GetAverageAgeOfUsers()
        //{

        //    var users = context.Users.Select(x => DateTime.Now.Year - x.DOB.Year).Average();


        //    if (users != 0)
        //    {
        //        return Ok(new { success = true, message = "Average age of users" });
        //    }
        //    else
        //    {
        //        return BadRequest(new { success = false, message = "No users found" });
        //    }

        //}


        //[Authorize]
        //[HttpGet]
        //[Route("GetOldestAndYoungestUserAge")]
        //public IActionResult GetOldestAndYoungestUserAge()
        //{

        //    var youngestUser = DateTime.Now.Year - context.Users.Select(x => x).Max(x => x.DOB).Year;

        //    var oldestUser = DateTime.Now.Year - context.Users.Select(x => x).Min(x => x.DOB).Year;

        //    if (youngestUser != 0 && oldestUser != 0)
        //    {
        //        return Ok(new { success = true, message = "youngest and oldest users are " });
        //    }
        //    else
        //    {
        //        return BadRequest(new { success = false, message = "No records found" });
        //    }


        //}

    }
}
