using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class ubicaciones_refugios
    {
        public string canton { get; set; }
        public int id_refugio { get; set; }
        public int id_ubicacion { get; set; }
        public double lalitud { get; set; }
        public double longitud { get; set; }

        public ubicaciones_refugios()
        {

        }

        public ubicaciones_refugios(string canton, int id_refugio, int id_ubicacion, double lalitud, double longitud)
        {
            this.canton = canton;
            this.id_refugio = id_refugio;
            this.id_ubicacion = id_ubicacion;
            this.lalitud = lalitud;
            this.longitud = longitud;
        }
    }
}
