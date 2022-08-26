using PetArmy.Infraestructure;
using Syncfusion.ListView.XForms;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListMascotasPage : ContentPage
    {
        public ListMascotasPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.Resources.TryGetValue("Locator", out object locator);
            InstanceLocator local_locator = locator as InstanceLocator;
            Task.Run(() => local_locator.Main.Mascotas.getData());
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {

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