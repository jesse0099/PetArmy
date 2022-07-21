using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class ubicaciones_refugiosGraphQLResponse
    {
        public List<ubicaciones_refugios> ubicaciones_refugios { get; set; }

        public ubicaciones_refugios ubicacion_refugio { get; set; }

        public ubicaciones_refugios insert_ubicaciones_refugios { get; set; }

        public ubicaciones_refugios update_ubicaciones_refugios_by_pk {get;set;}

    }
}
