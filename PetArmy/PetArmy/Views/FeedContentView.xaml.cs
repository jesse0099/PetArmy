using MLToolkit.Forms.SwipeCardView.Core;
using PetArmy.Infraestructure;
using PetArmy.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetArmy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedContentView : ContentPage
    {
        private FeedViewModel _feed_Instance;
        public FeedContentView()
        {
            InitializeComponent();
            _feed_Instance = FeedViewModel.GetInstance();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.Resources.TryGetValue("Locator", out object locator);
            InstanceLocator local_locator = locator as InstanceLocator;
            var tags = await local_locator.Main.Feed.GetGlobalTags();
            var user_current_location = EditShelterViewModel.GetCurrentPosition().Result;
            local_locator.Main.Feed.GetNearPetsBytags(tags, user_current_location.Latitude, user_current_location.Longitude);
        }

        void OnSwiped(object sender, SwipedCardEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeCardDirection.None:
                    break;
                case SwipeCardDirection.Right:
                    break;
                case SwipeCardDirection.Left:
                    break;
                case SwipeCardDirection.Up:
                    {
                        _feed_Instance.Border_Color = Color.LightSteelBlue;
                        break;
                    }
                case SwipeCardDirection.Down:
                    break;
            }
        }
    }
}