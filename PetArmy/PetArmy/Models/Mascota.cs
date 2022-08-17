﻿using PetArmy.ViewModels;
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

        private int _id_refugio;

        public int id_refugio 
        {
            get { return _id_refugio; }
            set
            {
                _id_refugio = value;
                OnPropertyChanged();
            }
        }


        private bool _castrado ;

        public bool castrado 
        {
            get { return _castrado; }
            set { _castrado = value; OnPropertyChanged(); }
        }

        private bool _alergias;

        public bool alergias 
        {
            get { return _alergias; }
            set { _alergias = value; OnPropertyChanged(); }
        }

        private bool _discapacidad;

        public bool discapacidad
        {
            get { return _discapacidad; }
            set { _discapacidad = value; OnPropertyChanged(); }
        }


        private bool _enfermedad ;

        public bool enfermedad 
        {
            get { return _enfermedad; }
            set { _enfermedad = value; OnPropertyChanged(); } 
        }

        private string _especie;

        public string especie
        {
            get { return _especie; }
            set { _especie = value;
                OnPropertyChanged();
            }
        }

        private int _id_mascota;

        public int id_mascota
        {
            get { return _id_mascota; }
            set { _id_mascota = value;
                OnPropertyChanged();
            }
        }

        private bool  _estado;

        public bool estado
        {
            get { return _estado; }
            set { _estado = value;
                OnPropertyChanged();
            }
        }

        private float _peso;

        public float peso
        {
            get { return _peso; }
            set { _peso = value;
                OnPropertyChanged();
            }
        }

        private float _edad;

        public float edad
        {
            get { return _edad; }
            set { _edad = value;
                OnPropertyChanged();
            }
        }

        
        private string _raza;

        public string raza
        {
            get { return _raza; }
            set { _raza = value;
                OnPropertyChanged();
            }
        }

        private bool _vacunado;
        public bool vacunado
        {
            get { return _vacunado; }
            set { _vacunado = value;
                OnPropertyChanged();
            }
        }

        private string _descripcion;
        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value;
                OnPropertyChanged();
            }
        }

        private Refugio _refugio;
        public Refugio refugio
        {
            get { return _refugio; }
            set { _refugio = value;
                OnPropertyChanged();
            }
        }

        private List<MascotaTag> _mascota_tags;
        public List<MascotaTag> mascota_tags
        {
            get { return _mascota_tags; }
            set { _mascota_tags = value;
                OnPropertyChanged();
            }
        }


        private IEnumerable<Imagen_Mascota> _imagenes_mascota;
        public IEnumerable<Imagen_Mascota> imagenes_mascota
        {
            get { return _imagenes_mascota; }
            set { _imagenes_mascota = value;
                OnPropertyChanged();
            }
        }


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
