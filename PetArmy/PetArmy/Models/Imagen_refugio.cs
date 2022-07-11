using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Imagen_refugio
    {

        public int id_imagen { get; set; }
        public int id_refugio { get; set; }
        public string imagen { get; set; }
        public bool isDefault { get; set; }
   
        public Imagen_refugio()
        {

        }
        public Imagen_refugio(int id_imagen, string imagen, bool isDefault)
        {
            this.id_imagen = id_imagen;
            this.imagen = imagen;
            this.isDefault = isDefault;
        }

        public Imagen_refugio(int id_imagen, int id_refugio, string imagen, bool isDefault)
        {
            this.id_imagen = id_imagen;
            this.id_refugio = id_refugio;
            this.imagen = imagen;
            this.isDefault = isDefault;
          
        }
    }
}
