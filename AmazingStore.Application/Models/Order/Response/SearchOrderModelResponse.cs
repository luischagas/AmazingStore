using AmazingStore.Domain.Enums.Order;
using System;
using System.Collections.Generic;

namespace AmazingStore.Application.Models.Order.Response
{
    public class SearchOrderModelResponse
    {
        #region Public Constructors

        public SearchOrderModelResponse(Guid id, Guid userId, string userName, EOrderStatus status)
        {
            Id = id;
            UserId = userId;
            UserName = userName;
            Status = status;
            Products = new List<SearchOrderProductModelResponse>();
        }

        #endregion Public Constructors

        #region Public Properties

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public EOrderStatus Status { get; set; }

        public List<SearchOrderProductModelResponse> Products { get; set; }

        #endregion Public Properties

    }
}