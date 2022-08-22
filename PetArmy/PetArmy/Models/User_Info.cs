using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class User_Info
    {
        public int idInfo { get; set; }
        public string idUser { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string username { get; set; }
        public string canton { get; set; }
        public string profilePicture { get; set; }
        public int age { get; set; }

        public User_Info()
        {
        }
    }
}
