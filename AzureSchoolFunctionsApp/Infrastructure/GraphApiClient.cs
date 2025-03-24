

using Azure.Identity;
using Microsoft.Graph;

namespace AzureSchoolFunctionsApp.Infrastructure
{
    public class GraphApiClient
    {
        public static GraphServiceClient GetClient(string tenantId, string clientId, string clientSecret)
        {
            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            return new GraphServiceClient(credential);
        }
    }
}
