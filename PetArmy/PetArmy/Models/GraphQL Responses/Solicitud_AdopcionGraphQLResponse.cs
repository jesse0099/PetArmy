using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PetArmy.Models.GraphQL_Responses
{
    public class Solicitud_AdopcionGraphQLResponse
    {
        public IEnumerable<Solicitud_Adopcion> solicitudes_adopcion { get; set; }

        public Solicitud_Adopcion update_solicitudes_adopcion_by_pk { get; set; }

        public Solicitud_Adopcion update_mascota_by_pk { get; set; }

        public Solicitud_Adopcion insert_registro_adopcion_one { get; set; }
        public Solicitud_AdopcionGraphQLResponse()
        {
        }

        public Solicitud_AdopcionGraphQLResponse(IEnumerable<Solicitud_Adopcion> solicitudes_adopcion, Solicitud_Adopcion update_solicitudes_adopcion_by_pk, Solicitud_Adopcion update_mascota_by_pk, Solicitud_Adopcion insert_registro_adopcion_one)
        {
            this.solicitudes_adopcion = solicitudes_adopcion;
            this.update_solicitudes_adopcion_by_pk = update_solicitudes_adopcion_by_pk;
            this.update_mascota_by_pk = update_mascota_by_pk;
            this.insert_registro_adopcion_one = insert_registro_adopcion_one;
        }
    }
}
