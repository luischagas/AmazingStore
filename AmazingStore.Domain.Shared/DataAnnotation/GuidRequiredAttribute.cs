using System;
using System.ComponentModel.DataAnnotations;

namespace AmazingStore.Domain.Shared.DataAnnotation
{
    public class GuidRequiredAttribute : ValidationAttribute
    {
        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext ctx)
        {
            if (ctx.MemberName != null)
            {
                var prop = ctx.ObjectType.GetProperty(ctx.MemberName);

                var input = (Guid)prop.GetValue(ctx.ObjectInstance);

                if (Guid.Empty == input)
                    return new ValidationResult($"The property {prop.Name} cannot be an empty GUID.");
            }

            return null;
        }

        #endregion Methods
    }
}