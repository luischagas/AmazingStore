using System;

namespace AmazingStore.Application.Models.User.Response
{
    public class SearchUserModelResponse
    {
        #region Public Constructors

        public SearchUserModelResponse(Guid id, string name, string username, string email, DateTime createdOn)
        {
            Id = id;
            Name = name;
            Username = username;
            Email = email;
            CreatedOn = createdOn;
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }

        #endregion Public Properties
    }
}