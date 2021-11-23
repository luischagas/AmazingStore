namespace AmazingStore.Infra.CrossCutting.Services.Communication.Models
{
    public class AuthModelRequest
    {
        #region Public Properties

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Audience { get; set; }

        public string GrantType { get; set; }

        #endregion Public Properties
    }
}