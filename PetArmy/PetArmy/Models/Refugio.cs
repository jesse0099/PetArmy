using System.Collections.Generic;

namespace PetArmy.Models
{
    public class Refugio
    {
        public bool activo { get; set; }
        public string administrador { get; set; }
        public string telefono { get; set; }
        public string nombre { get; set; }
        public string info_legal { get; set; }
        public int id_refugio { get; set; }
        public string direccion { get; set; }
        public string correo { get; set; }
        public int capacidad { get; set; }
        public List<ubicaciones_refugios> ubicacion { get; set; }

        public Refugio()
        {
        }

        public Refugio(int id_refugio, List<ubicaciones_refugios> ubicacion)
        {
            this.id_refugio = id_refugio;
            this.ubicacion = ubicacion;
        }

        public Refugio(bool activo, string administrador, string telefono, string nombre, string info_legal, int id_refugio, string direccion, string correo, int capacidad)
        {
            this.activo = activo;
            this.administrador = administrador;
            this.telefono = telefono;
            this.nombre = nombre;
            this.info_legal = info_legal;
            this.id_refugio = id_refugio;
            this.direccion = direccion;
            this.correo = correo;
            this.capacidad = capacidad;
        }

        public Refugio(bool activo, string administrador, string telefono, string nombre, int id_refugio, string direccion, string correo, int capacidad)
        {
            this.activo = activo;
            this.administrador = administrador;
            this.telefono = telefono;
            this.nombre = nombre;
            this.id_refugio = id_refugio;
            this.direccion = direccion;
            this.correo = correo;
            this.capacidad = capacidad;
        }
    }
}
