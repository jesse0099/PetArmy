using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class Camp_CastracionGraphQLResponse
    {
        public List<Camp_Castracion> Camp_Castracion { get; set; }
        public Camp_Castracion insert_camp_castracion { get; set; }
        public Camp_Castracion update_camp_castracion { get; set; }
        public Camp_Castracion delete_camp_castracion { get; set; }

        public Camp_CastracionGraphQLResponse()
        {
        }

        public Camp_CastracionGraphQLResponse(List<Camp_Castracion> camp_Castracion, 
                                                   Camp_Castracion insert_camp_castracion, 
                                                   Camp_Castracion update_camp_castracion, 
                                                   Camp_Castracion delete_camp_castracion)
        {
            Camp_Castracion = camp_Castracion;
            this.insert_camp_castracion = insert_camp_castracion;
            this.update_camp_castracion = update_camp_castracion;
            this.delete_camp_castracion = delete_camp_castracion;
        }
    }
}
