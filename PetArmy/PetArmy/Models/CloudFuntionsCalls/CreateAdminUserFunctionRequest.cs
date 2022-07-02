using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.CloudFuntionsCalls
{
    public class CreateAdminUserFunctionRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }
    }
}
