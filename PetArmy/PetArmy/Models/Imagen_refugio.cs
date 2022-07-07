using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Imagen_refugio
    {
   
        public int id_refugio { get; set; }
        public int id_imagen { get; set; }
        public byte[] imagen { get; set; }
        public bool isDefault { get; set; }

        public Imagen_refugio()
        {
        }

        public Imagen_refugio(int id_refugio, int id_imagen, byte[] imagen, bool isDefault)
        {
            this.id_refugio = id_refugio;
            this.id_imagen = id_imagen;
            this.imagen = imagen;
            this.isDefault = isDefault;
        }
    }
}
