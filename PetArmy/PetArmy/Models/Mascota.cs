using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Mascota
    {
        public string nombre { get; set; }

        public int id_refugio { get; set; }
        public bool castrado { get; set; }
        public bool alergias { get; set; }
        public bool discapacidad { get; set; }
        public bool enfermedad { get; set; }
        public string especie { get; set; }
        public int id_mascota { get; set; }
        public bool estado { get; set; }
        public float peso { get; set; }
        public string raza { get; set; }
        public bool vacunado { get; set; }
        public string descripcion { get; set; }
    }


        public Mascota(string nombre)
        {
            this.nombre = nombre;
        }


    }

