using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class Preferencia_adoptanteGraphQLResponse
    {

       public List<Preferencia_adoptante> preferencia_adoptante { get; set; }
       public Preferencia_adoptante delete_preferencia_adoptante { get; set; }
       public Preferencia_adoptante insert_preferencia_adoptante { get; set; }

        public Preferencia_adoptanteGraphQLResponse()
        {
        }

        public Preferencia_adoptanteGraphQLResponse(List<Preferencia_adoptante> preferencias, Preferencia_adoptante delete_preferencia_adoptante, Preferencia_adoptante insert_preferencia_adoptante)
        {
            this.preferencia_adoptante = preferencias;
            this.delete_preferencia_adoptante = delete_preferencia_adoptante;
            this.insert_preferencia_adoptante = insert_preferencia_adoptante;
        }
    }
}
