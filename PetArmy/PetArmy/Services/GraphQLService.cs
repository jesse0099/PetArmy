using System;
using GraphQL.Client.Http;
using System.Collections.Generic;
using System.Net.Http;
using GraphQL.Client.Abstractions;
using PetArmy.Models.GraphQL_Responses;
using System.Threading.Tasks;
using PetArmy.Helpers;
using GraphQL.Client.Serializer.Newtonsoft;
using PetArmy.Models;
using System.Text;

namespace PetArmy.Services
{

    public class GraphQLHttpRequestWithHeaders : GraphQLHttpRequest
    {
        public List<(String, String)>? Headers { get; set; }

        public override HttpRequestMessage ToHttpRequestMessage(GraphQLHttpClientOptions options, IGraphQLJsonSerializer serializer)
        {
            var r = base.ToHttpRequestMessage(options, serializer);

            foreach (var item in Headers)
            {
                r.Headers.Add(item.Item1, item.Item2);
            }

            return r;
        }
    }

    public static class GraphQLService
    {

        #region GraphQL Query Examples

        /*
        public static async Task GetMockString()
        {

            var client = new GraphQLHttpClient(Settings.GQL_URL,new NewtonsoftJsonSerializer());

            var request = new GraphQLHttpRequestWithHeaders
            {
                Query = @" query MyQuery{mockTable{mockString}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var response = await client.SendQueryAsync<MockGraphQLResponse>(request);

            foreach (var item in response.Data.mockTable)
            {
                Console.WriteLine("String: "+item.mockString);
            }
        }

        public static async Task Insert_MockString(string inString)
        {

            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var request = new GraphQLHttpRequestWithHeaders
            {
                Query = " mutation{ insert_mockTable(objects: [{mockString: \""+ inString +"\"}]){ returning{ mockString }}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var response = await client.SendQueryAsync<MockGraphQLResponse>(request);

        }

        public static async Task Get_MockString_ById(string id)
        {
            Console.WriteLine("Entra");

            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var request = new GraphQLHttpRequestWithHeaders
            {
                Query = "query MyQuery{ mockTable_by_pk(mockString: \""+id+"\"){mockString}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var response = await client.SendQueryAsync<MockGraphQLResponse>(request);
            Console.WriteLine("Sale");
        }
        
        */

        #endregion

        #region User Operations


        public static async Task<bool> validateCurUser(Usuario usuario)
        {

            bool validated = false;

            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var findRequest = new GraphQLHttpRequestWithHeaders
            {
                Query = "query MyQuery{ usuario_by_pk(uid: \"" + usuario.uid + "\"){uid tipo}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var foundResponse = await client.SendQueryAsync<UsuarioGraphQLResponse>(findRequest);

            if (foundResponse.Data.usuario_by_pk == null)
            {
                var createRequest = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation{ insert_usuario(objects: { tipo: \"" + usuario.tipo + "\", uid: \"" + usuario.uid + "\"}){ returning{ uid }}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var createdResponse = await client.SendQueryAsync<UsuarioGraphQLResponse>(createRequest);

                if (createdResponse.Data.insert_usuario != null)
                {
                    validated = true;
                }
                else
                {
                    validated = false;
                }
            }
            else
            {
                validated = true;
            }

            return validated;
        }


        #endregion

        #region Shelter Operations


        public static async Task<List<Refugio>> getAllShelters()
        {
            List<Refugio> shelters = null;

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery { refugio { id_refugio telefono nombre direccion correo capacidad administrador activo info_legal }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<RefugioGraphQLResponse>(request);
                shelters = response.Data.refugio;
            }
            catch (Exception)
            {

                throw;
            }

            return shelters;
        }


        public static async Task<int> countAllShelters()
        {
            int num = 0;
            try
            {
                List<Refugio> shelters = await getAllShelters();
                num = shelters.Count;
            }
            catch (Exception)
            {

                throw;
            }

            return num;
        }

        public static async Task<bool> createShelter(Refugio newShelter, Usuario user)
        {
            bool completed = false;

            bool isValidated = await validateCurUser(user);

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation { insert_refugio(objects: { " + "activo: " + newShelter.activo +
                                                                               ",administrador: \"" + newShelter.administrador + "\"" +
                                                                               ",capacidad: " + newShelter.capacidad +
                                                                               ",correo: \"" + newShelter.correo + "\"" +
                                                                               ",direccion: \"" + newShelter.direccion + "\"" +
                                                                               ",id_refugio: \"" + newShelter.id_refugio + "\"" +
                                                                               ",nombre: \"" + newShelter.nombre + "\"" +
                                                                               ",telefono: \"" + newShelter.telefono + "\"})" +
                                                                               "{ returning{ id_refugio }}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
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


        public static async Task<List<Refugio>> shelters_ByUser(string uid)
        {
            List<Refugio> shelters = new List<Refugio>();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery { refugio(where: {administrador: {_eq: \"" + uid + "\"}}) { id_refugio telefono nombre direccion correo capacidad administrador activo info_legal }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<RefugioGraphQLResponse>(request);
                shelters = response.Data.refugio;
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
            
            return shelters;
        }

        private static List<Imagen_refugio> imagesCollection;

        public static List<Imagen_refugio> ImagesCollection
        {
            get { return imagesCollection; }
            set { imagesCollection = value;}
        }

        
        public static async Task<List<Imagen_refugio>> getAllImages(){

            var clientNew = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
            try
            {
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery {imagen_refugio {id_imagen id_refugio imagen isDefault}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

               var response = await clientNew.SendQueryAsync<Imagen_refugioGraphQLResponse>(request);
               imagesCollection = response.Data.imagen_refugio;

                clientNew.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return imagesCollection;
        }

        public static async Task<int> countAllImages()
        {
            List<Imagen_refugio> imagens = await getAllImages();
            int count = imagens.Count;
            return count;
        }


        public static async Task addImage(Imagen_refugio img)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {insert_imagen_refugio(objects: {id_imagen: "+img.id_imagen
                                                                                 +", id_refugio: "+img.id_refugio
                                                                                 +",  imagen: \""+ img.imagen
                                                                                 +"\",isDefault: "+ img.isDefault 
                                                                                 +"}){returning {id_imagen}}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<Imagen_refugioGraphQLResponse>(request);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task<List<Imagen_refugio>> getImages_ByShelter(int shelter)
        {
            List<Imagen_refugio> images = new List<Imagen_refugio>();
         
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery { imagen_refugio(where: {id_refugio: {_eq: "+shelter+" }}) {id_imagen id_refugio imagen isDefault}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };
                var response = await client.SendQueryAsync<Imagen_refugioGraphQLResponse>(request);
                images = response.Data.imagen_refugio;
            }
            
            catch (Exception)
            {
                throw;
            }

            return images;
        }

        #endregion

        #region Default Images 

        public static async Task uploadDefaultImage(Default_Images defImg)
        {
            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var request = new GraphQLHttpRequestWithHeaders
            {
                Query = "mutation MyMutation {insert_default_Images(objects: {image: \""+defImg.imageName+"\"" + ", imageName: \""+defImg.image+"\"" + "}) {returning {image imageName}}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };
            var foundResponse = await client.SendQueryAsync<default_ImagesGraphQLResponse>(request);
        }

        #endregion


        #region Mascotas
        public static async Task<List<Mascota>> getAllMascotas()
        {
            List<Mascota> mascotas = new List<Mascota>();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery { mascota {  id_refugio,   " +
                                                         " nombre,       " +
                                                         " castrado,     " +
                                                         " alergias,     " +
                                                         " discapacidad, " +
                                                         " enfermedad,   " +
                                                         " especie,      " +
                                                         " id_mascota,   " +
                                                         " estado,       " +
                                                         " peso,         " +
                                                         " raza,         " +
                                                         " vacunado,     " +
                                                         " descripcion  " +
                                                         " }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
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


        public static async Task<Mascota> getMascotaByPK(string mascotaId)
        {
            Mascota mascota = new Mascota();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery { mascota_by_pk (  id_mascota: " + mascotaId + " )} {returning {id_refugio   " +
                                                                                                            " nombre       " +
                                                                                                            " castrado     " +
                                                                                                            " alergias     " +
                                                                                                            " discapacidad " +
                                                                                                            " enfermedad   " +
                                                                                                            " especie      " +
                                                                                                            " id_mascota   " +
                                                                                                            " estado       " +
                                                                                                            " peso         " +
                                                                                                            " raza         " +
                                                                                                            " vacunado     " +
                                                                                                            " descripcion  " +
                                                                                                            " }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
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
                    Query = @"query MyQuery  mascota (where: { id_refugio: { _eq: "+ shelterId.ToString() +"}) } {returning {id_mascota   " +
                                                                                                            " nombre       " +
                                                                                                            " castrado     " +
                                                                                                            " alergias     " +
                                                                                                            " discapacidad " +
                                                                                                            " enfermedad   " +
                                                                                                            " especie      " +
                                                                                                            " id_mascota   " +
                                                                                                            " estado       " +
                                                                                                            " peso         " +
                                                                                                            " raza         " +
                                                                                                            " vacunado     " +
                                                                                                            " descripcion  " +
                                                                                                            " id_refugio  " +
                                                                                                            " }}",
    

                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
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



        public static async Task<bool> addMascota(Refugio shelter, Mascota newPet)
        {
            bool completed = false;
            //bool isValidated = await validateCurUser(user);

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation { insert_mascota(objects: {alergias:" +  newPet.alergias + ", "+
                                                                            "castrado:" + newPet.castrado + ", " +
                                                                            "descripcion:" + newPet.descripcion + ", " +
                                                                            "discapacidad:" + newPet.discapacidad + ", " +
                                                                            "enfermedad:" + newPet.enfermedad + ", " +
                                                                            "especie:" + newPet.especie + ", " +
                                                                            "estado:" + newPet.estado + ", " +
                                                                            "id_mascota:" + newPet.id_mascota + ", " +
                                                                            "nombre:" + newPet.nombre + ", " +
                                                                            "peso:" + newPet.peso + ", " +
                                                                            " raza:" + newPet.raza + ", " +
                                                                            " vacunado:" + newPet.vacunado + ", "+
                                                                            "id_refugio:" + shelter.id_refugio +
                                                                            "})}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
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


        public static async Task<bool> deleteMascota(int mascotaId) 
        {
            bool success = false;
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {  delete_mascota(where: {id_mascota: {_eq : "+ mascotaId.ToString() + "}}) { returning {" +
                                                                            "castrado" + 
                                                                            "descripcion" + 
                                                                            "discapacidad" + 
                                                                            "enfermedad" + 
                                                                            "especie" + 
                                                                            "estado" + 
                                                                            "id_mascota" +
                                                                            "nombre" +
                                                                            "peso" +
                                                                            " raza" +
                                                                            " vacunado" + 
                                                                            "id_refugio" +
                                                                            "}}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<RefugioGraphQLResponse>(request);
                success = true;

            }
            catch (Exception)
            {

            }
            return success;

        }




        #endregion
    }

}
