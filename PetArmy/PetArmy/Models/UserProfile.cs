using System;

namespace PetArmy.Models
{
    public class UserProfile
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public UserProfile()
        {
        }
    }
}
