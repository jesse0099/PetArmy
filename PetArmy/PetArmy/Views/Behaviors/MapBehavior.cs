using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PetArmy.Models;

namespace PetArmy.Views.Behaviors
{
    public class MapBehavior : BindableBehavior<Map>
    {

        private Map _map;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.CreateAttached("ItemsSource", typeof(IEnumerable<UsersLocation>), typeof(MapBehavior),
                                            default(IEnumerable<UsersLocation>), BindingMode.Default, null, OnItemsSourceChanged);

        public IEnumerable<UsersLocation> ItemsSource
        {
            get { return (IEnumerable<UsersLocation>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject view, object oldValue, object newValue)
        {
            var mapBehavior = view as MapBehavior;

            if (mapBehavior != null)
            {
                mapBehavior.AddPins();
                mapBehavior.PositionMap();
            }
        }

        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);

            _map = bindable;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);

            _map = null;
        }

        private void AddPins()
        {
            for (int i = _map.Pins.Count - 1; i >= 0; i--)
            {
                _map.Pins.RemoveAt(i);
            }

            var pins = ItemsSource.Select(x =>
            {
                var pin = new Pin
                {
                    Type = PinType.SearchResult,
                    Position = new Position(x.Latitude, x.Longitude),
                    Label = x.Description
                };

                return pin;
            }).ToArray();

            foreach (var pin in pins)
                _map.Pins.Add(pin);
        }

        private void PositionMap()
        {
            if (ItemsSource == null || !ItemsSource.Any()) return;

            var centerPosition = new Position(ItemsSource.Average(x => x.Latitude), ItemsSource.Average(x => x.Longitude));

            var distance = 2.5;

            _map.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                _map.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));
                return false;
            });
        }
    }
}
