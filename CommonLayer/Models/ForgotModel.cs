using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class ForgotPasswordModel
    {
        public int UserId { get; set; }

        public string EmailId { get; set; }

        public string Token { get; set; }
    }
}
