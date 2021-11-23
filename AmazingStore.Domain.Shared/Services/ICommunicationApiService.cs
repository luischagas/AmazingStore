using RestSharp;

namespace AmazingStore.Domain.Shared.Services
{
    public interface ICommunicationApiService
    {
        #region Public Methods

        IRestResponse SendRequest(string url, Method method, object body);

        #endregion Public Methods
    }
}