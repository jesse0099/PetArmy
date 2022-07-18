using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Usuario
    {
        public string uid { get; set; }
        public int tipo { get; set; }

        public Usuario()
        {
        }

        public Usuario(string uid, int tipo)
        {
            this.uid = uid;
            this.tipo = tipo;
        }
    }
}
