using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Imagen
    {
        public int id_imagen { get; set; }
        public int id_refugio { get; set; }
        public byte[] imagen { get; set; }
        public bool isDefault { get; set; }
       

    }
}
