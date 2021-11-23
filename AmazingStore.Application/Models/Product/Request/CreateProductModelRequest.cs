using System;
using System.ComponentModel.DataAnnotations;

namespace AmazingStore.Application.Models.Product.Request
{
    public class CreateProductModelRequest
    {
        #region Public Properties

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must contain between 5 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "Description must contain between 5 and 100 characters")]
        public string Description { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0.1, 999999999999999.99)]
        public decimal Price { get; set; }

        #endregion Public Properties
    }
}