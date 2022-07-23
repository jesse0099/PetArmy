using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Camp_Castracion
    {
        public int id_campana { get; set; }
        public string nombre_camp { get; set; }
        public string descripcion { get; set; }
        public string direccion { get; set; }
        public string tel_contacto { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public bool activo { get; set; }

        public Camp_Castracion()
        {

        }

        public Camp_Castracion(int id_campana, string nombre_camp, string descripcion, string direccion, string tel_contacto, DateTime fecha_inicio, DateTime fecha_fin, bool activo)
        {
            this.id_campana = id_campana;
            this.nombre_camp = nombre_camp;
            this.descripcion = descripcion;
            this.direccion = direccion;
            this.tel_contacto = tel_contacto;
            this.fecha_inicio = fecha_inicio;
            this.fecha_fin = fecha_fin;
            this.activo = activo;
        }
    }
}