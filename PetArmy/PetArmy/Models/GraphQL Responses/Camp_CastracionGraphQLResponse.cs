using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class Camp_CastracionGraphQLResponse
    {
        public List<Camp_Castracion> camp_castracion { get; set; }

        public Camp_Castracion camp_castracion_by_pk { get; set; }
        public Camp_Castracion insert_camp_Castracion { get; set; }
        public Camp_Castracion update_camp_Castracion_by_pk { get; set; }
        public Camp_Castracion delete_camp_Castracion_by_pk { get; set; }

        public Camp_CastracionGraphQLResponse()
        {
        }

        public Camp_CastracionGraphQLResponse(List<Camp_Castracion> camp_castracion, Camp_Castracion camp_castracion_by_pk, Camp_Castracion insert_camp_Castracion, Camp_Castracion update_camp_Castracion_by_pk, Camp_Castracion delete_camp_Castracion_by_pk)
        {
            this.camp_castracion = camp_castracion;
            this.camp_castracion_by_pk = camp_castracion_by_pk;
            this.insert_camp_Castracion = insert_camp_Castracion;
            this.update_camp_Castracion_by_pk = update_camp_Castracion_by_pk;
            this.delete_camp_Castracion_by_pk = delete_camp_Castracion_by_pk;
        }
    }
}