using System;
using GraphQL.Client.Http;
using System.Collections.Generic;
using System.Net.Http;
using GraphQL.Client.Abstractions;
//using PetArmy.Models.GraphQL_Responses;
using System.Threading.Tasks;
using PetArmy.Helpers;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Query.Builder;
using PetArmy.Models;

namespace PetArmy.Services
{
    public class UpdateProfileResponse
    {
            public string nombre { get; set; }
            public bool casa_cuna { get; set; }
            public string correo { get; set; }
            public string direccion { get; set; }
            public string uid { get; set; }
            public object telefono { get; set; }
    }

    public static class ProfilesServices
    {

        /*
         * mutation MyMutation {
              update_perfil_adoptante(_set: {casa_cuna: false, correo: "test@test.com", direccion: "hola", nombre: "hola", telefono: "666666"}, where: {uid: {_eq: "6q1GNuhlG1OVavpauhBS1Jaw2wj1"}}) {
                affected_rows
              }
            }
         *
         */


        public static async Task<UserProfile> updateProfile(UserProfile profile)
        {
            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
            var updateProfileRequest = new GraphQLHttpRequestWithHeaders
            {
                Query = @"
                    mutation UpdateProfile {
                  update_perfil_adoptante(_set: {
                            casa_cuna: $casa_cuna, correo: $correo, direccion: $direccion, nombre: $nombre, telefono: $telefono}, 
                            where: {uid: {_eq: $uid} }) 
                      {
                          returning {
                          casa_cuna
                          correo
                          direccion
                          nombre
                          telefono
                          uid
                        }
                      }
                    }",
                OperationName = "UpdateProfile",
                Variables = new
                {
                    id = profile.Uid,
                    casa_cuna = profile.CasaCuna,
                    correo = profile.Email,
                    direccion = profile.Address,
                    telefono = profile.Phone,
                    nombre = profile.Name,
                },
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var updateProfileResponse = await client.SendQueryAsync<UpdateProfileResponse>(updateProfileRequest);

            Console.WriteLine(updateProfileResponse);

            return new UserProfile{
                Name = updateProfileResponse.Data.nombre,
                CasaCuna = updateProfileResponse.Data.casa_cuna,
                Email  = updateProfileResponse.Data.correo,
                Address = updateProfileResponse.Data.direccion,
                Uid = updateProfileResponse.Data.uid,
                Phone = updateProfileResponse.Data.telefono
            };



        }

    }




}
