using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Solicitud_Adopcion
    {
        public string adoptante { get; set; }
        public bool aprobacion { get; set; }

        public string fecha_solicitud { get; set; }

        public string fecha_revision { get; set; }

        public int id_mascota { get; set; }

        public int id_refugio { get; set; }

        public AdoptanteInfo adoptanteInfo { get; set; }

        public Mascota mascota { get; set; }

        public Refugio refugio { get; set; }

        public Solicitud_Adopcion()
        {
        }

        public Solicitud_Adopcion(string adoptante, bool aprobacion, string fecha_solicitud, int id_mascota, int id_refugio, AdoptanteInfo adoptanteInfo, Mascota mascota, Refugio refugio)
        {
            this.adoptante = adoptante;
            this.aprobacion = aprobacion;
            this.fecha_solicitud = fecha_solicitud;
            this.id_mascota = id_mascota;
            this.id_refugio = id_refugio;
            this.adoptanteInfo = adoptanteInfo;
            this.mascota = mascota;
            this.refugio = refugio;
        }
    }
}
