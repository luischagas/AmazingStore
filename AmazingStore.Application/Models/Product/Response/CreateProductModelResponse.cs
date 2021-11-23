using System;

namespace AmazingStore.Application.Models.Product.Response
{
    public class CreateProductModelResponse
    {
        #region Public Constructors

        public CreateProductModelResponse(Guid id)
        {
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid Id { get; set; }

        #endregion Public Properties
    }
}