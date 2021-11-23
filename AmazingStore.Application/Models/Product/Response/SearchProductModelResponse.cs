using System;

namespace AmazingStore.Application.Models.Product.Response
{
    public class SearchProductModelResponse
    {
        #region Public Constructors

        public SearchProductModelResponse(Guid id, string name, string description, decimal price, DateTime createdOn)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            CreatedOn = createdOn;
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }

        #endregion Public Properties

    }
}