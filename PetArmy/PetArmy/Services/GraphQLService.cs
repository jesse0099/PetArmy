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

        public static async Task updateShelter(Refugio newShelter)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {update_refugio_by_pk(pk_columns: {id_refugio: "+newShelter.id_refugio+ "}, _set: { " + "activo: " + newShelter.activo +
                                                                               ",capacidad: " + newShelter.capacidad +
                                                                               ",correo: \"" + newShelter.correo + "\"" +
                                                                               ",direccion: \"" + newShelter.direccion + "\"" +
                                                                               ",nombre: \"" + newShelter.nombre + "\"" +
                                                                               ",telefono: \"" + newShelter.telefono + "\"}){id_refugio }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<RefugioGraphQLResponse>(request);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task deleteShelter(int idShelter)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {delete_refugio_by_pk(id_refugio: "+idShelter+") { nombre }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<RefugioGraphQLResponse>(request);
            }
            catch (Exception)
            {

                throw;
            }
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
                    Query = "query MyQuery {imagen_refugio(order_by: {id_imagen: asc}) { id_imagen id_refugio imagen isDefault }}",
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
                    Query = "mutation MyMutation {insert_imagen_refugio(objects: { id_refugio: "+img.id_refugio
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


        public static async Task<Refugio> getShelterByID(int idShelter)
        {
            Refugio refugio = new Refugio();    
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery {refugio_by_pk(id_refugio: "+idShelter +") { activo administrador capacidad correo direccion id_refugio nombre telefono }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };
                var response = await client.SendQueryAsync<RefugioGraphQLResponse>(request);
                refugio = response.Data.refugio_by_pk;
                client.Dispose();
            }

            catch (Exception)
            {
                throw;
            }
            return refugio;
        }


        public static async Task updateImage(Imagen_refugio img)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {update_imagen_refugio(where: {id_imagen: {_eq: "+img.id_imagen+"}}, _set: {isDefault: "+img.isDefault+"}) {returning {id_imagen}}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };
                var response = await client.SendQueryAsync<RefugioGraphQLResponse>(request);
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
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

        public static async Task UpdateShelterLocation(ubicaciones_refugios newLocation)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {update_ubicaciones_refugios_by_pk(pk_columns: {id_ubicacion: "+newLocation.id_ubicacion+"}, _set: {canton: \""+newLocation.canton+"\", longitud: \""+newLocation.longitud+"\", latitud: \""+newLocation.lalitud+"\"}) {id_ubicacion}}",
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

        public static async Task<List<ubicaciones_refugios>> getLocationByShelter(int shelter)
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


        public static async Task<ubicaciones_refugios> getShelterLocationByID()
        {
            ubicaciones_refugios location = new ubicaciones_refugios();




            return location;
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


        public static async Task createOrUpdate_Adoptante(bool isFound, Perfil_adoptante adopt)
        {
            try
            {
              

                if (isFound)
                {
                    /*Update*/
                    var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                    var request = new GraphQLHttpRequestWithHeaders
                    {
                        Query = "mutation MyMutation { update_perfil_adoptante(where: {uid: {_eq: \""+adopt.uid+"\"}}, _set: {casa_cuna: "+adopt.casa_cuna
                                                                                                                             +", correo: \""+adopt.correo
                                                                                                                             +"\", direccion: \""+adopt.direccion
                                                                                                                             +"\", nombre: \""+adopt.nombre
                                                                                                                             +"\", telefono: \""+adopt.telefono+"\"}) { returning { uid } } }",
                        Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                    };

                    var response = await client.SendQueryAsync<Perfil_AdoptanteGraphQLResponse>(request);
                }
                else
                {
                    /*Create*/
                    await addAdoptante(adopt);
                }

            }
            catch (Exception)
            {

                throw;
            }
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
                                                                                                            " }}}",
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
                    Query = @"query MyQuery  mascota (where: { id_refugio: { _eq: " + shelterId.ToString() + "}) } {returning {id_mascota   " +
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




        public static async Task<bool> addMascota(Mascota newPet)
        {
            bool completed = false;
            //bool isValidated = await validateCurUser(user);

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation { insert_mascota(objects: {alergias: " + newPet.alergias + ", " +
                                                                            "castrado:" + newPet.castrado + ", " +
                                                                            "descripcion:\"" + newPet.descripcion + "\", " +
                                                                            "discapacidad:" + newPet.discapacidad + ", " +
                                                                            "enfermedad:" + newPet.enfermedad + ", " +
                                                                            "especie:\"" + newPet.especie + "\", " +
                                                                            "estado:" + newPet.estado + ", " +
                                                                            //"id_mascota:" + newPet.id_mascota + ", " +
                                                                            "nombre: \"" + newPet.nombre + "\", " +
                                                                            "peso:" + newPet.peso + ", " +
                                                                            " raza:\"" + newPet.raza + "\", " +
                                                                            " vacunado:" + newPet.vacunado + ", " +
                                                                            "id_refugio:" + newPet.id_refugio +
                                                                            "}) { returning { id_mascota }}}",
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
                    Query = "mutation MyMutation {  delete_mascota(where: {id_mascota: {_eq : " + mascotaId.ToString() + "}}) { returning {" +
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

        #region Pet_Images

       
        public static async Task addPetImage(Imagen_Mascota img)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = $"mutation MyMutation {{insert_imagen_mascota(objects: {{id_imagen: { img.id_imagen } , id_refugio: { img.id_mascota },  imagen:  { img.imagen }, isDefault: { img.isDefault } }}) {{returning {{id_imagen}} }} }}",
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


        public static async Task<List<Imagen_Mascota>> getPet_Images(int id_mascota)
        {
            List<Imagen_Mascota> images = new List<Imagen_Mascota>();

            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery { imagen_mascotas(where: {id_mascota: {_eq: " + id_mascota + " }}) {id_imagen id_id_mascota imagen isDefault}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
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

        #region User info 

        public static async Task<List<User_Info>> getUserInfo_ByUID(string uid)
        {
            List<User_Info> result = new List<User_Info>();
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery { User_Info(where: {idUser: {_eq: \""+uid+"\"}}) { age canton idInfo idUser username surname profilePicture name }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<User_InfoGraphQLResponse>(request);
                result = response.Data.User_Info;
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public static async Task createOrUpdate_UserInfo(bool isFound, User_Info info)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

                if (isFound)
                {
                    var request = new GraphQLHttpRequestWithHeaders
                    {
                        Query = "mutation MyMutation { update_User_Info(where: {idUser: {_eq: \""+info.idUser+"\"}}, _set: {age: "+info.age
                                                                                                                           +", canton: \""+info.canton
                                                                                                                           +"\", name: \""+info.name
                                                                                                                           +"\", profilePicture: \""+info.profilePicture
                                                                                                                           +"\", surname: \""+info.surname
                                                                                                                           +"\", username: \""+info.username+"\"}) { returning { idInfo }}}",
                        Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                    };
                    var response = await client.SendQueryAsync<User_InfoGraphQLResponse>(request);
                    client.Dispose();
                }
                else
                {
                    var request = new GraphQLHttpRequestWithHeaders
                    {
                        Query = "mutation MyMutation {insert_User_Info(objects: {age: "+info.age
                                                                                     +", canton: \""+info.canton
                                                                                     +"\", idUser: \""+info.idUser
                                                                                     +"\", name: \""+info.name
                                                                                     +"\", profilePicture: \""+info.profilePicture
                                                                                     +"\", surname: \""+info.surname
                                                                                     +"\", username: \""+info.username+"\"}) {returning { idUser }}}",
                        Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                    };

                    var response = await client.SendQueryAsync<User_InfoGraphQLResponse>(request);
                    client.Dispose();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region User Preferences 

        public static async Task DeletePreference(Preferencia_adoptante preferencia)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation {delete_preferencia_adoptante(where: {uid: {_eq: \""+preferencia.uid+"\"}, id_tag: {_eq: "+preferencia.id_tag+"}}) { returning { uid }}}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                var response = await client.SendQueryAsync<Preferencia_adoptanteGraphQLResponse>(request);
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task AddPreference(Preferencia_adoptante preference)
        {
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "mutation MyMutation { insert_preferencia_adoptante(objects: {uid: \""+preference.uid+"\", id_tag: "+preference.id_tag+"}) { returning { uid } } }",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };
                var response = await client.SendQueryAsync<Preferencia_adoptanteGraphQLResponse>(request);
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static async Task<List<Preferencia_adoptante>> GetUserPreferences(string uid)
        {
            List<Preferencia_adoptante> preferences = new List<Preferencia_adoptante>();
            try
            {
                var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());
                var request = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query MyQuery { preferencia_adoptante(where: {uid: {_eq: \""+uid+"\" }}) { id_tag uid }}",
                    Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };
                var response = await client.SendQueryAsync<Preferencia_adoptanteGraphQLResponse>(request);
                preferences = response.Data.preferencia_adoptante;
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
            return preferences;
        }

        #endregion

    }

}

