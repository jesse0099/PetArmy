using System.ComponentModel;
using PetArmy.Models;

namespace PetArmy.ViewModels
{
    public class MascotasViewModel : BaseViewModel
    {
        private static MascotasViewModel _instance;

        
        private BindingList<Mascota> _mascotas;
        public BindingList<Mascota> Mascotas
        {
            get { return _mascotas; }
            set
            {
                _mascotas = value;
                OnPropertyChanged();
            }
        }

        public static MascotasViewModel GetInstance()
        {
            // esto es tuanis usenlo malditos
            return _instance ??= _instance = new MascotasViewModel();
        }

        private MascotasViewModel()
        {

            this.Mascotas = new BindingList<Mascota>()
            {
                new Mascota()
                {
                    nombre = null,
                    id_refugio = 0,
                    castrado = false,
                    alergias = false,
                    discapacidad = false,
                    enfermedad = false,
                    especie = "loool",
                    id_mascota = 10,
                    estado = false,
                    peso = 0,
                    edad = 0,
                    raza = null,
                    vacunado = false,
                    descripcion = "amazin",
                    refugio = null,
                    mascota_tags = null,
                    imagenes_mascota = null,
                    Db_Bools = null,
                    Ubicacion = null
                }
            };
        }
        
    }
}