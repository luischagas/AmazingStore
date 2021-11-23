namespace AmazingStore.Domain.Shared.Entities
{
    public class Auth0
    {
        #region Public Properties

        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Audience { get; set; }

        #endregion Public Properties
    }
}