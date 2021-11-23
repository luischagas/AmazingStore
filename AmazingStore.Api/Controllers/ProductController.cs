using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Product.Request;
using AmazingStore.Application.Models.Product.Response;
using AmazingStore.Domain.Shared.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AmazingStore.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : BaseController
    {
        #region Private Fields

        private readonly IProductService _productService;

        #endregion Private Fields

        #region Public Constructors

        public ProductController(INotifier notifier,
            IProductService productService)
            : base(notifier)
        {
            _productService = productService;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Adds a new product using the properties supplied, returns a GUID reference for the order created
        /// </summary>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppServiceResponse<CreateProductModelResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(AppServiceResponse<ICollection<Notification>>))]
        public async Task<IActionResult> Create([FromBody] CreateProductModelRequest request)
        {
            if (ModelState.IsValid is false)
            {
                NotifyModelStateErrors();

                return GenerateResponse(HttpStatusCode.BadRequest, new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Product", false));
            }

            var result = await _productService.Create(request);

            if (result.Success is false)
                return GenerateResponse(HttpStatusCode.BadRequest, result);

            return GenerateResponse(HttpStatusCode.OK, result);
        }


        /// <summary>
        /// Update a product using the properties supplied, returns all data updated
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppServiceResponse<UpdateProductModelResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(AppServiceResponse<ICollection<Notification>>))]
        public async Task<IActionResult> Update([FromBody] UpdateProductModelRequest request, Guid id)
        {
            if (ModelState.IsValid is false)
            {
                NotifyModelStateErrors();

                return GenerateResponse(HttpStatusCode.BadRequest, new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Updating Product", false));
            }

            var result = await _productService.Update(request, id);

            if (result.Success is false)
                return GenerateResponse(HttpStatusCode.BadRequest, result);

            return GenerateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Get all products being able to filter specific properties 
        /// </summary>
        /// <param name="request"></param>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppServiceResponse<SearchProductModelResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(AppServiceResponse<ICollection<Notification>>))]
        public async Task<IActionResult> GetAll([FromQuery] SearchProductModelRequest request)
        {
            if (ModelState.IsValid is false)
            {
                NotifyModelStateErrors();

                return GenerateResponse(HttpStatusCode.BadRequest, new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Getting Products", false));
            }

            var result = await _productService.GetAll(request);

            if (result.Success is false)
                return GenerateResponse(HttpStatusCode.BadRequest, result);

            return GenerateResponse(HttpStatusCode.OK, result);
        }

        #endregion Public Methods
    }
}