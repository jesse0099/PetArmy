using PetArmy.ViewModels;
using System;
using System.Linq;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PetArmy.Models
{
    public class AdminAccountRequest : BaseViewModel
    {
        public string _approvedBy { get; set; }
        public DateTime _createdOn { get; set; }
        public AdminAccountDetails _adminAccountDetail { get; set; }
        private string status;
        public string _status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }
        public string _docId { get; set; }
        public string _motive { get; set; }
        private bool enabled;
        public bool _enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                OnPropertyChanged();
            }
        }
        public string _accessGrantedBy { get; set; }

    }
}