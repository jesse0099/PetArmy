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


        #endregion


        #region SearchBar Operations

        public static async Task<IEnumerable<Tag>> getAllTags()
        {
            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var findRequest = new GraphQLHttpRequestWithHeaders
            {
                Query = "query MyQuery {tag {id_tagnombre_tag}}",
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
            
            var client = new GraphQLHttpClient(Settings.GQL_URL, new NewtonsoftJsonSerializer());

            var findRequest = new GraphQLHttpRequestWithHeaders
            {
                Query = "query MyQuery {mascota_tag(where: { id_tag: { _eq: \"" + searchTag + "\"}}) {id_mascota}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            };

            var foundResponse = await client.SendQueryAsync<SearchBarGraphQLResponse>(findRequest);

            var foundResponsePets = foundResponse;

            if (foundResponse.Data.mascota_tag != null)
            {
                findRequest = new GraphQLHttpRequestWithHeaders
                {
                Query = "query MyQuery {mascota(where: { id_mascota: { _eq: \"" + foundResponse.Data.mascota_tag + "\" }}) {nombre}}",
                Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
                };

                foundResponsePets = await client.SendQueryAsync<SearchBarGraphQLResponse>(findRequest);
            }

            //foreach (int item in foundResponse.Data.mascota_tag)
            //{
            //findRequest = new GraphQLHttpRequestWithHeaders
            //{
            //Query = "query MyQuery {mascota(where: { id_mascota: { _eq: \"" + item + "\" }}) {nombre}}",
            //Headers = new List<(string, string)> { (@"X-Hasura-Admin-Secret", Settings.GQL_Secret) }
            //};

            //var foundResponsePets = await client.SendQueryAsync<SearchBarGraphQLResponse>(findRequest);
            //result.Add(foundResponsePets.Data.mascota);
            //}

            return foundResponsePets.Data.mascota;

        }

        #endregion

    }
}
