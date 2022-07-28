using MLToolkit.Forms.SwipeCardView.Core;
using PetArmy.Models;
using PetArmy.Services;
using Resx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static PetArmy.Models.Mascota;

namespace PetArmy.ViewModels
{
    public class FeedViewModel : BaseViewModel
    {
        private BindingList<Mascota> _mascotas;

        public BindingList<Mascota> Mascotas
        {
            get { return _mascotas; }
            set
            {
                _mascotas = value;
                OnPropertyChanged();
            }
        }

        private Color _border_Color;

        public Color Border_Color
        {
            get { return _border_Color; }
            set
            {
                _border_Color = value;
                OnPropertyChanged();
            }
        }

        public ICommand Dragging_Command
        {
            get
            {
                return new Command((e) =>
                {
                    DraggingCommand(e as DraggingCardEventArgs);
                });
            }
        }

        public ICommand Swiped_Command
        {
            get
            {
                return new Command((e) =>
                {
                    SwipedCommand(e as SwipedCardEventArgs);
                });
            }
        }

        public FeedViewModel()
        {
            _instance = this;
        }

        async public Task<string[]> GetGlobalTags()
        {
            string[] tag_names = null;
            try
            {
                var response = await GraphQLService.getAllTags();
                tag_names = Array.ConvertAll(Enumerable.ToArray<Tag>(response), x => x.nombre_tag);
            }
            catch (Exception e)
            {

                Console.WriteLine(nameof(FeedViewModel), e.Message);
            }
            return tag_names;
        }

        async public void GetNearPetsBytags(string[] tags, double latitude, double longitude)
        {
            Mascotas = new BindingList<Mascota>(await GraphQLService.GetNearPetsByTags(tags, latitude, longitude, 1000) as List<Mascota>);
            
            //UI Settings  
            List<Color> gradient = new();
            for (int i = 0; i < 5; i++){
                Application.Current.Resources.TryGetValue($"Grad{i + 1}", out object color);
                gradient.Add((Color)color);
            }

            foreach (var pet in Mascotas){
                var bool_values = new List<PetDbBools>()
                {
                    new PetDbBools() { Bool_Name = AppResources.Disability, Bool_Value = pet.discapacidad, Bool_Color = gradient[0] },
                    new PetDbBools() { Bool_Name = AppResources.Illness, Bool_Value = pet.enfermedad, Bool_Color = gradient[1]  },
                    new PetDbBools() { Bool_Name = AppResources.Allergies, Bool_Value = pet.alergias, Bool_Color = gradient[2]  },
                    new PetDbBools() { Bool_Name = AppResources.VaxxedKey, Bool_Value = pet.vacunado, Bool_Color = gradient[3]  },
                    new PetDbBools() { Bool_Name = AppResources.CastratedKey, Bool_Value = pet.castrado, Bool_Color = gradient[4]  },
                };
                pet.Db_Bools = bool_values.Where(x => x.Bool_Value).ToList();
                pet.Ubicacion = pet.refugio.ubicacion[0];
            }
        }
        #region Swipe View Events
        public void SwipedCommand(SwipedCardEventArgs e)
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
                    break;
                case SwipeCardDirection.Down:
                    break;
            }
        }
        public void DraggingCommand(DraggingCardEventArgs e)
        {
            Color addopt_color = Color.LightGray;
            Color next_color = Color.LightGray;
            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                Application.Current.Resources.TryGetValue("TealGreen", out object teal_green_dark);
                Application.Current.Resources.TryGetValue("SynthPumpkin", out object synth_pupmkin);
                addopt_color = (Color)teal_green_dark;
                next_color = (Color)synth_pupmkin;
            }
            else
            {
                Application.Current.Resources.TryGetValue("FluoRed", out object fluo_red);
                addopt_color = Color.YellowGreen;
                next_color = (Color)fluo_red;
            }

            switch (e.Position)
            {
                //Podria quitarse en el futuro (Start), para prevenir excesiva facilidad para desplegar la adopcion
                case DraggingCardPosition.Start:
                    {
                        if (e.Direction == SwipeCardDirection.Up)
                        {
                            Border_Color = addopt_color;
                        }
                        break;
                    }
                case DraggingCardPosition.UnderThreshold:
                    {
                        if (e.Direction == SwipeCardDirection.Up)
                        {
                            Border_Color = addopt_color;
                        }
                        break;
                    }
                case DraggingCardPosition.OverThreshold:
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    {

                        Border_Color = next_color;
                        break;
                    }
                case DraggingCardPosition.FinishedOverThreshold:
                    {
                        Border_Color = next_color;
                        break;
                    }
            }
        }
        #endregion



        private static FeedViewModel _instance;
        public static FeedViewModel GetInstance()
        {
            return _instance ??= _instance = new FeedViewModel();
        }
    }
}
