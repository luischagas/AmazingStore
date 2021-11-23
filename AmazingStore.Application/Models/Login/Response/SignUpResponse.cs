using System;
using Newtonsoft.Json;

namespace AmazingStore.Application.Models.Login.Response
{
    public class SignUpResponse
    {
        #region Public Constructors

        public SignUpResponse(Guid id, string username)
        {
            Id = id;
            Username = username;
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid Id { get; set; }

        public string Username { get; set; }

        #endregion Public Properties

    }
}