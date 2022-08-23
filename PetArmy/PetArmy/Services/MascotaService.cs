using System;
using GraphQL.Client.Http;
using System.Collections.Generic;
using PetArmy.Models.GraphQL_Responses;
using System.Threading.Tasks;
using PetArmy.Helpers;
using GraphQL.Client.Serializer.Newtonsoft;
using PetArmy.Models;
using PetArmy.Models.GrapQLRequests;
using PetArmy.Models.GrapQLRequests.UpdatePetRequestModels;

namespace PetArmy.Services
{

    public static class MascotaService
    {

        public static async Task<IEnumerable<Mascota>> getAllMascotas(string adminId)
        {
           try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = Commons.GetPetsByShelter,
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    Variables = new {
                        adminId
                    }
                };

                var response = await client.SendQueryAsync<PetsByShelterResponse>(request);
                return response.Data.pets_by_shelter;

            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public static async Task<Mascota> getMascotaByPK(string mascotaId)
        {
            Mascota mascota = new Mascota();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery { mascota_by_pk (  id_mascota: $mascotaId } {returning {id_refugio
                                                                                                            nombre       
                                                                                                            castrado     
                                                                                                            alergias     
                                                                                                            discapacidad 
                                                                                                            enfermedad   
                                                                                                            especie      
                                                                                                            id_mascota   
                                                                                                            estado       
                                                                                                            peso         
                                                                                                            raza         
                                                                                                            vacunado     
                                                                                                            descripcion  
                                                                                                            }}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    Variables = new
                    {
                        mascotaId = mascotaId

                    }
                };

                var response = await client.SendQueryAsync<MascotaGraphQLResponse>(request);
                mascota = response.Data.mascota_by_pk;

            }
            catch (Exception)
            {

                throw;
            }

            return mascota;

        }

        public static async Task<List<Mascota>> getMascotaByShelterID(int shelterId)
        {
            var mascotas = new List<Mascota>();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery  mascota (where: { id_refugio: { _eq:$shelterId}) } {returning {id_mascota   
                                                                                                            nombre       
                                                                                                            castrado     
                                                                                                            alergias     
                                                                                                            discapacidad 
                                                                                                            enfermedad   
                                                                                                            especie      
                                                                                                            id_mascota   
                                                                                                            estado       
                                                                                                            peso         
                                                                                                            raza         
                                                                                                            vacunado     
                                                                                                            descripcion  
                                                                                                            id_refugio 
                                                                                                            }}",


                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    Variables = new
                    {
                        shelterId = shelterId.ToString()
                    }
                };

                var response = await client.SendQueryAsync<MascotaGraphQLResponse>(request);
                mascotas = response.Data.mascota;

            }
            catch (Exception)
            {

                throw;
            }

            return mascotas;

        }




        public static async Task<bool> addMascota(Mascota newPet)
        {
            bool completed = false;
            //bool isValidated = await validateCurUser(user);

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = Commons.InsertPet,

                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    Variables = new
                    {
                        newPet.alergias,
                        newPet.castrado,
                        newPet.descripcion,
                        newPet.discapacidad,
                        newPet.enfermedad,
                        newPet.especie,
                        newPet.estado,
                        newPet.nombre,
                        newPet.peso,
                        newPet.raza,
                        newPet.vacunado,
                        newPet.id_refugio,
                    }
                };

                var response = await client.SendQueryAsync<RefugioGraphQLResponse>(request);
                completed = true;
            }
            catch (Exception)
            {

                throw;
            }
            return completed;
        }


        public static async Task<Mascota> updateMascota(Mascota mascota, IEnumerable<ImagenMascotaInsertRequest> addedImages, 
            IEnumerable<UpdatedImage> updatedImages, IEnumerable<int> deletedImages)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = Commons.UpdatePetByPk,
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    Variables = new
                    {
                        mascota.id_mascota,
                        mascota.alergias,
                        mascota.castrado,
                        mascota.descripcion,
                        mascota.discapacidad,
                        mascota.edad,
                        mascota.enfermedad,
                        mascota.especie,
                        mascota.estado,
                        mascota.refugio.id_refugio,
                        mascota.nombre,
                        mascota.peso,
                        mascota.raza,
                        mascota.vacunado,
                        addedImages,
                        updatedImages,
                        deletedImages
                    }
                };

                var response = await client.SendQueryAsync<Mascota>(request);
                var mascota_response = response.Data;

                return mascota_response;

            }
            catch (Exception e)
            {
                Console.WriteLine("Update Pet", e.Message);
                return new Mascota();
            }
        }

        public static async Task<bool> DeleteMascota(int mascotaId, int refugioId)
        {
            bool success = false;
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = Commons.DeletePetByPlk,

                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    Variables = new
                    {
                        mascotaId,
                        refugioId
                        
                    }
                };

                var response = await client.SendQueryAsync<Mascota>(request);
                success = true;

            }
            catch (Exception e)
            {
                throw e;
            }
            return success;

        }

        #region Pet_Images


        public static async Task addPetImages(Imagen_Mascota img)
        //this parameter should be a list.

        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"mutation MyMutation {{insert_imagen_mascota(objects: {{id_imagen: {$id_imagen} , imagen:  {$imagen}, id_mascota:{$id_mascota},isDefault: {$isDefault} }}) {{returning {{id_imagen}} }} }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    Variables = new
                    {
                        id_mascota = img.id_mascota,
                        id_imagen = img.id_imagen,
                        imagen = img.imagen,
                        isDefault = img.isDefault

                    }
                };

                var response = await client.SendQueryAsync<Imagen_refugioGraphQLResponse>(request);
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task<List<Imagen_Mascota>> getPet_Images(int id_mascota)
        {
            List<Imagen_Mascota> images = new List<Imagen_Mascota>();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery { imagen_mascotas(where: {id_mascota: {_eq: $id_mascota }}) {id_imagen id_id_mascota imagen isDefault}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    Variables = new
                    {
                        id_mascota = id_mascota
                    }
                };
                var response = await client.SendQueryAsync<Imagen_MascotaGraphQLResponse>(request);
                images = response.Data.imagen_mascota;
                client.Dispose();
            }

            catch (Exception)
            {
                throw;
            }

            return images;
        }

        public static async Task<List<Imagen_Mascota>> getAllPetImages()
        {
            List<Imagen_Mascota> images = new List<Imagen_Mascota>();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"A query that actually works pls!!!!!PLS PLS PLS PSL",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) },
                    //Variables = new
                    //{
                    //    id_mascota = id_mascota
                    //}
                };
                var response = await client.SendQueryAsync<Imagen_MascotaGraphQLResponse>(request);
                images = response.Data.imagen_mascota;
                client.Dispose();
            }

            catch (Exception)
            {
                throw;
            }

            return images;
        }
        #endregion




    }


}
