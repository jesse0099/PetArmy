namespace PetArmy.Models.CloudFuntionsCalls
{
    public class UpdateAdminAccountAccessRequest
    {
        public string motive { get; set; }
        public string action { get; set; }
        public string docId { get; set; }
        public string email { get; set; }
    }
}