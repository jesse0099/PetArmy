using PetArmy.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PetArmy.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}