namespace PetArmy.Models.GrapQLRequests
{
    public class ImagenMascotaInsertRequest
    {
        public int id_imagen { get; set; }
        public int id_mascota { get; set; }
        public string image { get; set; }
        public bool idDefault { get; set; }
    }
}
