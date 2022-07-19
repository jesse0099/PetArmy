using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Mascota
    {
        public string nombre { get; set; }

        public Mascota()
        {
        }

        public Mascota(string nombre)
        {
            this.nombre = nombre;
        }


    }
}
