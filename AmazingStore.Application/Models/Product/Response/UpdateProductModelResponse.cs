using System;

namespace AmazingStore.Application.Models.Product.Response
{
    public class UpdateProductModelResponse
    {
        #region Public Constructors

        public UpdateProductModelResponse(Guid id, string name, string description, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        #endregion Public Properties
    }
}