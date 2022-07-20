using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class Camp_CastracionGraphQLResponse
    {
        public List<Camp_Castracion> Camp_Castracion { get; set; }

        public Camp_CastracionGraphQLResponse()
        {
        }
        public Camp_CastracionGraphQLResponse(List<Camp_Castracion> camp_Castracion)
        {
            Camp_Castracion = camp_Castracion;
        }
    }
}
