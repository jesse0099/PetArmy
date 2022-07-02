using System;
using GraphQL.Client.Http;
using PetArmy.Helpers;
using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Threading.Tasks;
using Polly;
using System.Linq;
using PetArmy.Models;
using System.Collections;
using System.Collections.Generic;
using ModernHttpClient;
using System.Net.Http;
using GraphQL.Client.Abstractions;

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

        #region ClientConfig

        static readonly GraphQLHttpClient _client = CreateClient();
        static GraphQLHttpClient Client => _client;



        static GraphQLHttpClient CreateClient()
        {
            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(Settings.GQL_URL),
                HttpMessageHandler = new NativeMessageHandler(),
            };

            var client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer());
         
            return client;
        }

        static async Task<T> AttemptAndRetry<T>(Func<Task<GraphQLResponse<T>>> action, int numRetries = 2)
        {
            var response = await Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action).ConfigureAwait(false);

            if (response.Errors != null && response.Errors.Count() > 1)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));
            else if (response.Errors != null && response.Errors.Any())
                throw new Exception(response.Errors.First().Message.ToString());

            return response.Data;

            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
        #endregion


        public static async Task<MockString> GetMockString()
        {
            string login = "s1";

            try
            {
                var graphQLRequest = new GraphQLHttpRequestWithHeaders
                {
                    Query = "query { mockTable_by_pk(mockString: \"" + login + "\"){ mockString }}}",
                    Headers = new List<(String, String)>{
                    (@"X-Hasura-Admin-Secret", @"sl0iQE5H49U1EtomqiRf43Wtq3YEXlPA0g2099PWuEiKMwb6qWn4r7od416BIrNn")
                }
                };

                MockString s = new MockString("hola");
                var graphQLResponse  = await AttemptAndRetry(() => Client.SendQueryAsync<MockGraphQLResponse>(graphQLRequest)) ?? new MockGraphQLResponse(s);

                Console.WriteLine(graphQLResponse.Data);

              
                return s;
            }
            catch (Exception e)
            {

                Console.WriteLine("Error login: " + e);

                throw;
            }
        }

    }
}
