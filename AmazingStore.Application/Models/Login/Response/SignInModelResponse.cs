using Newtonsoft.Json;

namespace AmazingStore.Application.Models.Login.Response
{
    public class SignInModelResponse
    {
        #region Public Constructors

        public SignInModelResponse(string accessToken)
        {
            AccessToken = accessToken;
        }

        #endregion Public Constructors

        #region Public Properties

        public string AccessToken { get; set; }

        #endregion Public Properties

    }
}