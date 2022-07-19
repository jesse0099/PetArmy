﻿using System;
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

            client.Dispose();
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
                client.Dispose();
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
                client.Dispose();
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
                client.Dispose();
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
                client.Dispose();
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

        #region Ubications

        public static async Task<List<ubicaciones_refugios>> getAllShelterUbications()
        {
            List<ubicaciones_refugios> locations = new List<ubicaciones_refugios>();

            try
            {

                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery { ubicaciones_refugios { canton id_refugio id_ubicacion latitud longitud }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<ubicaciones_refugiosGraphQLResponse>(request);
                locations = response.Data.ubicaciones_refugios;
                client.Dispose();

            }
            catch (Exception)
            {

                throw;
            }

            return locations;
        }

        public static async Task<int> countShelterLocations()
        {
            List < ubicaciones_refugios > locations= await getAllShelterUbications();

            return locations.Count;
        }

        public static async Task addShelterLocation(ubicaciones_refugios location)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {insert_ubicaciones_refugios(objects: {canton: \""+location.canton+"\""+
                                                                                        ", id_refugio: "+location.id_refugio+
                                                                                        ", id_ubicacion: "+location.id_ubicacion+
                                                                                        ", latitud: "+location.lalitud+
                                                                                        ", longitud: "+location.longitud+"}) {returning {id_refugio}}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<ubicaciones_refugiosGraphQLResponse>(request);
                client.Dispose();

            }
            catch (Exception)
            {

                throw;
            }

        } 

        public static async Task<List<ubicaciones_refugios>> getLocationsByShelter(int shelter)
        {
            List<ubicaciones_refugios> locations = new List<ubicaciones_refugios>();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery {ubicaciones_refugios(where: {id_refugio: {_eq: "+shelter+"}}) { canton id_refugio id_ubicacion latitud longitud}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<ubicaciones_refugiosGraphQLResponse>(request);
                locations = response.Data.ubicaciones_refugios;
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }


            return locations;
        }

        public static async Task<List<ubicaciones_casasCuna>> getAllCasasCuna()
        {
            List<ubicaciones_casasCuna> casasCuna = new List<ubicaciones_casasCuna>();

            try
            {

                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery {ubicaciones_casasCuna { canton id_ubicacion id_user lalitud longitud }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<Ubicaciones_CasasCunaGraphQLResponse>(request);
                casasCuna = response.Data.ubicaciones_casasCuna;
                client.Dispose();

            }
            catch (Exception)
            {

                throw;
            }


            return casasCuna;
        }

        public static async Task<int> countCasasCuna()
        {
            List<ubicaciones_casasCuna> locations = await getAllCasasCuna();

            return locations.Count;
        }

        public static async Task addCasaCunaLocation(ubicaciones_casasCuna location)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {insert_ubicaciones_refugios(objects: {canton: \"" + location.canton + "\"" +
                                                                                        ", id_ubicacion: " + location.id_ubicacion +
                                                                                        ", id_user: \"" + location.id_user +
                                                                                        "\", latitud: " + location.lalitud +
                                                                                        ", longitud: " + location.longitud + "}) {returning {id_refugio}}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<ubicaciones_refugiosGraphQLResponse>(request);
                client.Dispose();

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Adoptanes


        public static async Task<List<Perfil_adoptante>> getAllAdoptantes()
        {
            List<Perfil_adoptante> adoptantes = new List<Perfil_adoptante>();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = @"query MyQuery {perfil_adoptante { casa_cuna correo direccion nombre telefono uid }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<Perfil_AdoptanteGraphQLResponse>(request);
                adoptantes = response.Data.perfil_adoptante;
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }

            return adoptantes;
        }

        public static async Task addAdoptante(Perfil_adoptante adoptante)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {insert_perfil_adoptante(objects: {casa_cuna: "+adoptante.casa_cuna 
                                                                                   +", correo: \""+adoptante.correo
                                                                                   +"\", direccion: \""+adoptante.direccion
                                                                                   +"\", nombre: \""+adoptante.nombre
                                                                                   +"\", telefono: \""+adoptante.telefono
                                                                                   +"\", uid: \""+adoptante.uid
                                                                                   +"\"}) {returning {uid}}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<Perfil_AdoptanteGraphQLResponse>(request);
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<Perfil_adoptante> getAdoptanteByID(string uid)
        {
            Perfil_adoptante adoptante = new Perfil_adoptante();
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery {perfil_adoptante_by_pk(uid: \""+uid+"\"){ casa_cuna correo direccion nombre telefono uid }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<Perfil_AdoptanteGraphQLResponse>(request);
                adoptante = response.Data.perfil_adoptante_by_pk;
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }

            return adoptante;
        }

        #endregion

        #region SearchBar Operations

        public static async Task<IEnumerable<Tag>> getAllTags()
        {
            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var findRequest = new GraphQLHttpRequestWithHeaders
            {
                Query = "query MyQuery {tag {id_tag, nombre_tag}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var foundResponse = await client.SendQueryAsync<SearchBarGraphQLResponse>(findRequest);

            return foundResponse.Data.tag;
        }

        public static async Task<List<Mascota>> get30Pets()
        {


            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var findRequest = new GraphQLHttpRequestWithHeaders
            {
                Query = "query MyQuery {mascota(limit: 30) {nombre}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var foundResponse = await client.SendQueryAsync<SearchBarGraphQLResponse>(findRequest);

            return foundResponse.Data.mascota;
        }

        public static async Task<List<Mascota>> getPetsByTag(String searchTag)
        {
            var result = new List<Mascota>();
            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var findRequest = new GraphQLHttpRequestWithHeaders
            {
                Query = "query MyQuery {mascota_tag(where: { id_tag: { _eq: \"" + searchTag + "\"}}) {id_mascota}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var foundResponse = await client.SendQueryAsync<SearchBarGraphQLResponse>(findRequest);

            var foundResponsePets = foundResponse;


            foreach (var item in foundResponse.Data.mascota_tag)
            {
                findRequest = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery {mascota(where: { id_mascota: { _eq: \"" + item.id_mascota + "\" }}) {nombre}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                foundResponsePets = await client.SendQueryAsync<SearchBarGraphQLResponse>(findRequest);
                result.Add((foundResponsePets.Data.mascota[0]));
            }

            return result;

        }

        #endregion
    }

}
