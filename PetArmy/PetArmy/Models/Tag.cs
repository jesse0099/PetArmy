using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Tag
    {
        public string nombre_tag { get; set; }
        public int id_tag { get; set; }

        public Tag()
        {
        }

        public Tag(string nombre_tag, int id_tag)
        {
            this.nombre_tag = nombre_tag;
            this.id_tag = id_tag;
        }
    }
}
