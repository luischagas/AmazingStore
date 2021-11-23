using AmazingStore.Domain.Shared.DataAnnotation;
using System;
using System.Collections.Generic;

namespace AmazingStore.Application.Models.Order.Request
{
    public class CreateOrderModelRequest
    {
        #region Public Constructors

        public CreateOrderModelRequest()
        {
            Products = new List<CreateOrderProductModelRequest>();
        }

        #endregion Public Constructors

        #region Public Properties

        [GuidRequired(ErrorMessage = "User id is required")]
        public Guid UserId { get; set; }

        public List<CreateOrderProductModelRequest> Products { get; set; }

        #endregion Public Properties
    }
}