using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class User_InfoGraphQLResponse
    {

        public List<User_Info> User_Info { get; set; }
        public User_Info update_User_Info { get; set; }
        public User_Info insert_User_Info { get; set; }
        public User_Info user_info { get; set; }


    }
}
