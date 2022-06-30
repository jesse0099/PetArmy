using System;
using System.Collections.Generic;
using System.Text;
using PetArmy.Interfaces;
using PetArmy.Models;
using System.Windows.Input;
using Xamarin.Forms;
using Google.Cloud.Firestore;

namespace PetArmy.ViewModels
{
    
    public class GetUsersRequest : BaseViewModel
    {
        FirestoreDb database = FirestoreDb.Create("platzigramgc");
        public async void GetData()
        {
            Query allCitiesQuery = database.Collection("userCreationRequests");
            QuerySnapshot allRequestQuery = await allCitiesQuery.GetSnapshotAsync();

            foreach (DocumentSnapshot docSnap in allRequestQuery.Documents)
            {
                CreateAdminUserRequest UserRequest = docSnap.ConvertTo<CreateAdminUserRequest>();

                if (docSnap.Exists)
                {
                    Console.WriteLine("Firts Name: " + UserRequest.firstName + "\n" +
                                      "Last Name: " + UserRequest.lastName + "\n");
                }
            }

        }
    }
}
