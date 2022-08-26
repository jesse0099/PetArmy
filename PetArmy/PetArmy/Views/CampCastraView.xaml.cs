using PetArmy.Infraestructure;
using Syncfusion.ListView.XForms;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampCastraView : ContentPage
    {
        public CampCastraView()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.Resources.TryGetValue("Locator", out object locator);
            Task.Run(async () => {((InstanceLocator)locator).Main.CampCastracion.getCampCastra(); });
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Application.Current == null)
                return;
            if (Application.Current.MainPage == null)
                return;
            if (width > 0 && DeviceDisplay.MainDisplayInfo.Width != width)
            {
                var size = Application.Current.MainPage.Width / CartItemsView.ItemSize;
                gridLayout.SpanCount = (int)size;
                CartItemsView.LayoutManager = gridLayout;
            }
        }
    }
}