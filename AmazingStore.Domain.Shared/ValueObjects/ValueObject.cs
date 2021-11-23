using FluentValidation;
using FluentValidation.Results;

namespace AmazingStore.Domain.Shared.ValueObjects
{
    public abstract class ValueObject<T> : AbstractValidator<T>
         where T : ValueObject<T>
    {
        #region Constructors

        protected ValueObject()
        {
            ValidationResult = new ValidationResult();
        }

        #endregion Constructors

        #region Properties

        public ValidationResult ValidationResult { get; protected set; }

        #endregion Properties

        #region Methods

        public abstract bool IsValid();

        #endregion Methods
    }
}