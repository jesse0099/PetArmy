using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class AdoptanteInfo
    {
        public string correo { get; set; }
        public string nombre { get; set; }

        public AdoptanteInfo()
        {
        }

        public AdoptanteInfo(string correo, string nombre)
        {
            this.correo = correo;
            this.nombre = nombre;
        }
    }
}
