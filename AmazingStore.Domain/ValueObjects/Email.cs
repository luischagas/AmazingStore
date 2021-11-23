using AmazingStore.Domain.Shared.ValueObjects;
using FluentValidation;

namespace AmazingStore.Domain.ValueObjects
{
    public class Email : ValueObject<Email>
    {
        #region Public Constructors

        public Email(string address)
        {
            Address = address;
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected Email()
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Address { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public override bool IsValid()
        {
            ValidateAddress();

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        #endregion Public Methods

        #region Private Methods

        private void ValidateAddress()
        {
            RuleFor(a => a.Address)
                .NotEmpty().WithMessage("Name cannot be empty")
                .EmailAddress().WithMessage("Invalid email")
                .Length(5, 100).WithMessage("Email must contain between 5 and 100 characters");
        }

        #endregion Private Methods
    }
}