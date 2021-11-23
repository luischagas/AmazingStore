using AmazingStore.Domain.Shared.Services;
using Newtonsoft.Json;
using RestSharp;

namespace AmazingStore.Infra.CrossCutting.Services.Communication
{
    public class CommunicationApiService : ICommunicationApiService
    {
        #region Public Methods

        public IRestResponse SendRequest(string url, Method method, object body)
        {
            var client = new RestClient($"{url}")
            {
                Timeout = -1
            };

            var request = new RestRequest(method);

            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var result = client.Execute(request);

            return result;
        }

        #endregion Public Methods
    }
}