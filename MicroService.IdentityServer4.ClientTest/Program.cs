using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroService.IdentityServer4.ClientTest
{
    class Program
    {
        async static Task Main(string[] args)
        {

            await GetTokenAndCallApi();

            Console.ReadKey();
        }

        private async static Task GetTokenAndCallApi()
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var authencinateUrl = "http://localhost:7200";
            var disco = await client.GetDiscoveryDocumentAsync(authencinateUrl);

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //request token
            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId="fang_client",
                ClientSecret= "fang_autheration_pwd",
                Scope= "fangtest"
            };
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            //api call
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var apiUrl = "http://localhost:7300/api/test/AuthenticateTest";
            var apiResponse = await apiClient.GetAsync(apiUrl);

            if (apiResponse.IsSuccessStatusCode)
            {
                var contnet = await apiResponse.Content.ReadAsStringAsync();
                //System.Text.Json.JsonSerializer.Deserialize<dynamic>(contnet);
                Console.WriteLine(contnet);
            }
            else
            {
                Console.WriteLine(apiResponse.StatusCode);
            }
        }
    }
}
