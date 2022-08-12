using PetArmy.ViewModels;
using System.Collections.Generic;

namespace PetArmy.Models
{
    public class Mascota: BaseViewModel
    {
        private string _nombre;

        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; 
                OnPropertyChanged();
            }
        }

        public int id_refugio { get; set; }
        public bool castrado { get; set; }
        public bool alergias { get; set; }
        public bool discapacidad { get; set; }
        public bool enfermedad { get; set; }
        public string especie { get; set; }
        public int id_mascota { get; set; }
        public bool estado { get; set; }
        public float peso { get; set; }
        public float edad { get; set; }
        public string raza { get; set; }
        public bool vacunado { get; set; }
        public string descripcion { get; set; }
        public Refugio  refugio { get; set; }
        public List<MascotaTag>  mascota_tags { get; set; }
        public IEnumerable<Imagen_Mascota> imagenes_mascota { get; set; }

        private List<PetDbBools> _db_Bools;
        public List<PetDbBools> Db_Bools
        {
            get { return _db_Bools; }
            set { _db_Bools = value;
                OnPropertyChanged();
            }
        }

        private ubicaciones_refugios _ubicacion;
        public ubicaciones_refugios Ubicacion
        {
            get { return _ubicacion; }
            set { _ubicacion = value;
                OnPropertyChanged();
            }
        }




        public Mascota(string nombre)
        {
            this.nombre = nombre;
        }
        public Mascota(string nombre, int id_refugio, bool castrado, bool alergias, bool discapacidad, bool enfermedad, string especie, int id_mascota, bool estado, float peso, string raza, bool vacunado, string descripcion, Refugio refugio, List<MascotaTag> mascota_tags, IEnumerable<Imagen_Mascota> imagenes_mascota)
        {
            this.nombre = nombre;
            this.id_refugio = id_refugio;
            this.castrado = castrado;
            this.alergias = alergias;
            this.discapacidad = discapacidad;
            this.enfermedad = enfermedad;
            this.especie = especie;
            this.id_mascota = id_mascota;
            this.estado = estado;
            this.peso = peso;
            this.raza = raza;
            this.vacunado = vacunado;
            this.descripcion = descripcion;
            this.mascota_tags = mascota_tags;
            this.discapacidad = discapacidad;
            this.imagenes_mascota = imagenes_mascota;
            this.refugio = refugio;
        }

        public Mascota()
        {
            
        }


    }



}
