using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class ubicaciones_casasCuna
    {
        public string canton { get; set; }
        public int id_ubicacion { get; set; }
        public string id_user { get; set; }
        public double lalitud { get; set; }
        public double longitud { get; set; }

        public ubicaciones_casasCuna()
        {
        }

        public ubicaciones_casasCuna(string canton, int id_ubicacion, string id_user, double lalitud, double longitud)
        {
            this.canton = canton;
            this.id_ubicacion = id_ubicacion;
            this.id_user = id_user;
            this.lalitud = lalitud;
            this.longitud = longitud;
        }
    }
}
