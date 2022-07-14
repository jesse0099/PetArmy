using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class Perfil_AdoptanteGraphQLResponse
    {

       public List<Perfil_adoptante> perfil_adoptante { get; set; }
       public Perfil_adoptante insert_perfil_adoptante { get; set; }
       public Perfil_adoptante perfil_adoptante_by_pk { get; set; }

    }
}
