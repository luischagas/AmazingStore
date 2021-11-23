using System;
using System.ComponentModel.DataAnnotations;
using AmazingStore.Domain.Shared.DataAnnotation;

namespace AmazingStore.Application.Models.Order.Request
{
    public class CreateOrderProductModelRequest
    {
        #region Public Properties

        [GuidRequired(ErrorMessage = "Product id is required")]
        public Guid Id { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0.1, 999999999999999.99)]
        public decimal CurrentPrice { get; set; }

        [Range(1, 100, ErrorMessage = "Invalid quantity ")]
        public int Quantity { get; set; }

        #endregion Public Properties
    }
}