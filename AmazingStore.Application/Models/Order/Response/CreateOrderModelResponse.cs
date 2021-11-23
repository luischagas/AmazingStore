using System;

namespace AmazingStore.Application.Models.Order.Response
{
    public class CreateOrderModelResponse
    {
        #region Public Constructors

        public CreateOrderModelResponse(Guid id)
        {
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid Id { get; set; }

        #endregion Public Properties
    }
}