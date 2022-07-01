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

namespace PetArmy.Services
{
    public static class GraphQLService
    {

        #region Client Config 
        static readonly Lazy<GraphQLHttpClient> _client = new(CreateGitHubGraphQLClient);
        static GraphQLHttpClient Client => _client.Value;

        static GraphQLHttpClient CreateGitHubGraphQLClient()
        {
            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("https://pet-army-101.hasura.app/v1/graphql"),
                HttpMessageHandler = new NativeMessageHandler()
            };

            var client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer());

            client.HttpClient.DefaultRequestHeaders.Add("x-hasura-admin-secret", Settings.GQL_Secret);


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

        #region Queries 

        public static async Task<IEnumerable<MockString>> GetMockData()
        {
            IEnumerable<MockString> mStrings = null;

            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = " query { mockTable { mockString } } "
                };

                var response = await Client.SendQueryAsync<MockGraphQLResponse>(graphQLRequest);
      
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e);
            }

            return mStrings ;
        }


        #endregion

    }
}
