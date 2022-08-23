using PetArmy.ViewModels;

namespace PetArmy.Models
{
    public class Camp_Castracion : BaseViewModel
    {
        private int _id_campana;

        public int id_campana
        {
            get { return _id_campana; }
            set
            {
                _id_campana = value;
                OnPropertyChanged();
            }
        }

        private string _nombre_camp;

        public string nombre_camp
        {
            get { return _nombre_camp; }
            set
            {
                _nombre_camp = value;
                OnPropertyChanged();
            }
        }

        private string _descripcion;

        public string descripcion
        {
            get { return _descripcion; }
            set
            {
                _descripcion = value;
                OnPropertyChanged();
            }
        }

        private string _direccion;

        public string direccion
        {
            get { return _direccion; }
            set
            {
                _direccion = value;
                OnPropertyChanged();
            }
        }

        private string _tel_contacto;

        public string tel_contacto
        {
            get { return _tel_contacto; }
            set
            {
                _tel_contacto = value;
                OnPropertyChanged();
            }
        }


        public Camp_Castracion()
        {

        }

        public Camp_Castracion(int id_campana, string nombre_camp, string descripcion, string direccion, string tel_contacto)
        {
            this.id_campana = id_campana;
            this.nombre_camp = nombre_camp;
            this.descripcion = descripcion;
            this.direccion = direccion;
            this.tel_contacto = tel_contacto;
        }
    }
}