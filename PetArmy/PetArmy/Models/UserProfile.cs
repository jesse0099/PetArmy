namespace PetArmy.Models
{
    public class UserProfile
    {
        public string Name { get; set; }
        public bool CasaCuna { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Uid { get; set; }
        public object Phone { get; set; }

        public UserProfile()
        {
        }
    }
}
