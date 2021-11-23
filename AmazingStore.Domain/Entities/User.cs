using AmazingStore.Domain.ValueObjects;
using FluentValidation;
using System.Collections.Generic;
using AmazingStore.Domain.Shared.Entities;

namespace AmazingStore.Domain.Entities
{
    public class User : Entity<User>
    {
        protected IList<Order> _orders;

        #region Public Constructors

        public User(string name, string userName, string password, Email email)
        {
            Name = name;
            Username = userName;
            Password = password;
            Email = email;
            _orders = new List<Order>();
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected User()
        {
            _orders = new List<Order>();
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Name { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public Email Email { get; private set; }

        public IEnumerable<Order> Orders => _orders;

        #endregion Public Properties

        #region Public Methods

        public override bool IsValid()
        {
            ValidateName();
            ValidateUsername();
            ValidatePassword();

            AddErrors(Validate(this));

            return ValidationResult.IsValid;
        }

        #endregion Public Methods

        #region Private Methods

        private void ValidateName()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Name cannot be empty")
                .Length(5, 100).WithMessage("Name must contain between 5 and 100 characters");
        }

        private void ValidateUsername()
        {
            RuleFor(d => d.Username)
                .NotEmpty().WithMessage("User Name cannot be empty")
                .Length(5, 100).WithMessage("User Name must contain between 5 and 100 characters");
        }

        private void ValidatePassword()
        {
            RuleFor(d => d.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .Length(5, 100).WithMessage("Password must contain between 5 and 100 characters");
        }

        #endregion Private Methods
    }
}