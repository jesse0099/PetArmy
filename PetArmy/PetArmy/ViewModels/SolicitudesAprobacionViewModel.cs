using PetArmy.Models;
using PetArmy.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace PetArmy.ViewModels
{
    public class SolicitudesAprobacionViewModel : BaseViewModel
    {

        #region Singleton

        public static SolicitudesAprobacionViewModel Instance = null;


        public static SolicitudesAprobacionViewModel GetInstance()
        {
            if (Instance == null)
            {
                Instance = new SolicitudesAprobacionViewModel();
            }

            return Instance;
        }

        public static void DisposeInstance()
        {
            if (Instance != null)
            {
                Instance = null;
            }
        }

        public SolicitudesAprobacionViewModel()
        {
            initCommands();
            initClass();
            getRequests();
        }

        public void initCommands()
        {
            GetRequests = new Command(getRequests);
            UpdateRequest = new Command<Solicitud_Adopcion>(updateRequest);
            ApproveRequest = new Command<Solicitud_Adopcion>(approveRequest);
            RejectRequest = new Command<Solicitud_Adopcion>(rejectRequest);

        }

        public void initClass()
        {

        }


        #endregion


        #region Varaiables

        private BindingList<Solicitud_Adopcion> solicitudes;
        public BindingList<Solicitud_Adopcion> Solicitudes
        {
            get { return solicitudes; }
            set { solicitudes = value; OnPropertyChanged(); }
        }
        private BindingList<Solicitud_Adopcion> solicitudesRaw;
        public BindingList<Solicitud_Adopcion> SolicitudesRaw
        {
            get { return solicitudesRaw; }
            set { solicitudesRaw = value; OnPropertyChanged(); }
        }

        #endregion


        #region Commands and Functions


        public ICommand GetRequests { get; set; }

        public async void getRequests()
        {
            try
            {
                Solicitudes = new BindingList<Solicitud_Adopcion>(await GraphQLService.getRequests() as List<Solicitud_Adopcion>);
                SolicitudesRaw = Solicitudes;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommand UpdateRequest { get; set; }

        public async void updateRequest(Solicitud_Adopcion solicitud_seleccionada)
        {
            try
            {
                await GraphQLService.updateAdoptionRequest(solicitud_seleccionada).ConfigureAwait(false);
                await GraphQLService.insertAdoptionRecord(solicitud_seleccionada).ConfigureAwait(false);
                await GraphQLService.updatePetStatus(solicitud_seleccionada).ConfigureAwait(false);
                getRequests();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommand ApproveRequest { get; set; }

        public void approveRequest(Solicitud_Adopcion solicitud_seleccionada)
        {
            try
            {
                solicitud_seleccionada.aprobacion = true;
                updateRequest(solicitud_seleccionada);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommand SortByStatusCommand
        {
            get
            {
                return new Command<bool>(e =>
                {
                    SortByStatusExecute(e);
                });
            }
        }

        void SortByStatusExecute(bool aprobacion)
        {
            Solicitudes = new BindingList<Solicitud_Adopcion>(SolicitudesRaw.Where(x => x.aprobacion.Equals(aprobacion)).ToList());
        }
        public ICommand RejectRequest { get; set; }

        public void rejectRequest(Solicitud_Adopcion solicitud_seleccionada)
        {
            try
            {
                solicitud_seleccionada.aprobacion = false;
                updateRequest(solicitud_seleccionada);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
