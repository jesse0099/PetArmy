using System;
using GraphQL.Client.Http;
using System.Collections.Generic;
using System.Net.Http;
using GraphQL.Client.Abstractions;
using PetArmy.Models.GraphQL_Responses;

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




    }

}
