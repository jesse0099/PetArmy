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


namespace PetArmy.Services
{
    public static class GraphQLService
    {

        #region ClientConfig

        static readonly Lazy<GraphQLHttpClient> _client = new(CreateClient);
        static GraphQLHttpClient Client => _client.Value;


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
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query { mockTable_by_pk(mockString: \"" + login + "\"){ mockString }}}"
                };

                var gitHubUserResponse = await AttemptAndRetry(() => Client.SendQueryAsync<MockGraphQLResponse>(graphQLRequest)).ConfigureAwait(false);

                return gitHubUserResponse.Data;
            }
            catch (Exception e)
            {

                Console.WriteLine("Error login: " + e);

                throw;
            }
        }

    }
}
