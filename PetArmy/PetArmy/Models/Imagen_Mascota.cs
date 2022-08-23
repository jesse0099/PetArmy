namespace PetArmy.Models
{
    public class Imagen_Mascota
    {
        public int id_imagen { get; set; }
        public int id_mascota { get; set; }
        public string imagen { get; set; }
        public bool isDefault { get; set; }

        public Imagen_Mascota()
        {

        }
        public Imagen_Mascota(string imagen)
        {
            this.imagen = imagen;
        }
        public Imagen_Mascota(int id_imagen, string imagen, bool isDefault)
        {
            this.id_imagen = id_imagen;
            this.imagen = imagen;
            this.isDefault = isDefault;
        }

        public Imagen_Mascota(int id_imagen, int id_mascota, string imagen, bool isDefault)
        {
            this.id_imagen = id_imagen;
            this.id_mascota = id_mascota;
            this.imagen = imagen;
            this.isDefault = isDefault;

        }

        public override bool Equals(object obj)
        {
            var external = obj as Imagen_Mascota;
            
            if(external is null)
                if(this is not null)
                    return false;
                else 
                    return true;

            if (this is null)
                return false;
            else
               if (external.id_imagen != this.id_imagen || external.id_mascota != this.id_mascota)
                return false;
            else
                return true;
        }
    }
}
