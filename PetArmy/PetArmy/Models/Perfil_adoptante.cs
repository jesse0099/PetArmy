using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Perfil_adoptante
    {
        public string uid { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public bool casa_cuna { get; set; }
        public string direccion { get; set; }

        public Perfil_adoptante()
        {
        }

        public Perfil_adoptante(string uid, string nombre, string correo, string telefono, bool casa_cuna, string direccion)
        {
            this.uid = uid;
            this.nombre = nombre;
            this.correo = correo;
            this.telefono = telefono;
            this.casa_cuna = casa_cuna;
            this.direccion = direccion;
        }
    }
}
