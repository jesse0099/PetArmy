using System.Collections.Generic;

namespace PetArmy.Models.GrapQLRequests.UpdatePetRequestModels
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class And
    {
        public IdMascota id_mascota { get; set; }
        public IdImagen id_imagen { get; set; }
    }

    public class IdImagen
    {
        public int _eq { get; set; }
    }

    public class IdMascota
    {
        public int _eq { get; set; }
    }

    public class Root
    {
        public List<UpdatedImage> updatedImages { get; set; }
    }

    public class Set
    {
        public string image { get; set; }
    }

    public class UpdatedImage
    {
        public Where where { get; set; }
        public Set _set { get; set; }
    }

    public class Where
    {
        public List<And> _and { get; set; }
    }

}
