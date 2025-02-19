using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace ManagerLayer
{
    public interface IUserManager
    {
        public Users Registration(RegisterModel model);

        public bool MailExists(string email);

        public string Login(LoginModel model);

        public string ForgetPassword(string Email);

        public bool ResetLink(string email, string password, string confirmPassword);

        
        










    }
}
