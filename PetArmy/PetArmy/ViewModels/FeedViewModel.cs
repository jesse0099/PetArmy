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

        private Mascota _topItem;

        public Mascota TopItem
        {
            get { return _topItem; }
            set { _topItem = value;
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

        private string _currentActionText;
        public string CurrentActionText
        {
            get { return _currentActionText; }
            set
            {
                _currentActionText = value;
                OnPropertyChanged();
            }
        }

        private bool _currentActionVisibility;
        public bool CurrentActionVisibility
        {
            get { return _currentActionVisibility; }
            set
            {
                _currentActionVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _openConfirmationPopUp;
        public bool OpenConfirmationPopUp
        {
            get { return _openConfirmationPopUp; }
            set
            {
                _openConfirmationPopUp = value;
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
            CurrentActionVisibility = false;
            OpenConfirmationPopUp = false;
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
            for (int i = 0; i < 5; i++)
            {
                Application.Current.Resources.TryGetValue($"Grad{i + 1}", out object color);
                gradient.Add((Color)color);
            }

            foreach (var pet in Mascotas)
            {
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
                    {
                        var local = CurrentActionText;
                        CurrentActionText = string.Empty;
                        if (string.IsNullOrEmpty(local))
                            break;
                        if(!local.Equals(AppResources.RequestAdoption))
                            break;
                        ConfirmAdoptionRequestViewModel.GetInstance().CurrentPet = TopItem;
                        OpenConfirmationPopUp = true;
                        break;
                    }
                case SwipeCardDirection.Left:
                    break;
                case SwipeCardDirection.Up:
                    {
                        var local = CurrentActionText;
                        CurrentActionText = string.Empty;
                        if (string.IsNullOrEmpty(local))
                            break;
                        if (!local.Equals(AppResources.RequestAdoption))
                            break;
                        ConfirmAdoptionRequestViewModel.GetInstance().CurrentPet = TopItem;
                        OpenConfirmationPopUp = true;
                        break;
                    }
                case SwipeCardDirection.Down:
                    break;
            }
        }
        public void DraggingCommand(DraggingCardEventArgs e)
        {
            Color addopt_color = Color.LightGray;
            Color next_color = Color.LightGray;
            Color non_selected_color = Color.LightGray;

            Application.Current.Resources.TryGetValue("ArcadeYellow", out object arcade_yellow);
            Application.Current.Resources.TryGetValue("ArcadeGreen", out object arcade_green);
            non_selected_color = (Color)arcade_yellow;
            addopt_color = (Color)arcade_green;

            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                Application.Current.Resources.TryGetValue("SynthPumpkin", out object synth_pupmkin);
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
                case DraggingCardPosition.Start:
                    break;
                case DraggingCardPosition.UnderThreshold:
                    {
                        CurrentActionVisibility = false;
                        if (e.Direction == SwipeCardDirection.Right || (e.Direction == SwipeCardDirection.Up && e.DistanceDraggedX == 0))
                            Border_Color = addopt_color;
                        if (e.Direction == SwipeCardDirection.Left || (e.Direction == SwipeCardDirection.Down))
                            Border_Color = next_color;
                        break;
                    }
                case DraggingCardPosition.OverThreshold:
                    {
                        //Label with action name
                        //Request adoption
                        if (e.Direction == SwipeCardDirection.Right || (e.Direction == SwipeCardDirection.Up && e.DistanceDraggedX == 0))
                        {
                            CurrentActionVisibility = true;
                            CurrentActionText = AppResources.RequestAdoption;
                            Border_Color = addopt_color;
                        }
                        //Next Profile
                        if (e.Direction == SwipeCardDirection.Left || (e.Direction == SwipeCardDirection.Down)
                            || (e.Direction == SwipeCardDirection.Up && e.DistanceDraggedX != 0))
                        {
                            CurrentActionVisibility = true;
                            CurrentActionText = AppResources.NextPetProfile;
                            Border_Color = next_color;
                        }
                        break;
                    }
                case DraggingCardPosition.FinishedUnderThreshold:
                    {
                        //Neutral State
                        Border_Color = non_selected_color;
                        CurrentActionVisibility = false;
                        break;
                    }
                case DraggingCardPosition.FinishedOverThreshold:
                    {
                        //Neutral State (Next Cards)
                        Border_Color = non_selected_color;
                        CurrentActionVisibility = false;
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
