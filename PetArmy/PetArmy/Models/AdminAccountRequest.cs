using System;

namespace PetArmy.Models
{
    public class AdminAccountRequest
    {
        public string _approvedBy { get; set; }
        public DateTime _createdOn { get; set; }
        public AdminAccountDetails _adminAccountDetail { get; set; }
        public string _status{ get; set; }
        public string _docId{ get; set; }
        public string _motive{ get; set; }
    }
}
