using Newtonsoft.Json;

namespace AmazingStore.Application.Models.Login.Response
{
    public class AuthModelResponse
    {
        #region Public Properties

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string ExpiresIn { get; set; }

        #endregion Public Properties
    }
}