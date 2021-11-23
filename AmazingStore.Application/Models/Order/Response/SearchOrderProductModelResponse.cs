using System;

namespace AmazingStore.Application.Models.Order.Response
{
    public class SearchOrderProductModelResponse
    {
        #region Public Constructors

        public SearchOrderProductModelResponse(Guid id, string name, decimal currentPrice, int quantity)
        {
            Id = id;
            Name = name;
            CurrentPrice = currentPrice;
            Quantity = quantity;
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public int Quantity { get; set; }

        #endregion Public Properties

    }
}