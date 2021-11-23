using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Order.Request;
using AmazingStore.Application.Models.Order.Response;
using AmazingStore.Domain.Shared.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AmazingStore.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        #region Private Fields

        private readonly IOrderService _orderService;

        #endregion Private Fields

        #region Public Constructors

        public OrderController(INotifier notifier,
            IOrderService orderService)
            : base(notifier)
        {
            _orderService = orderService;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Get all orders being able to filter specific properties 
        /// </summary>
        /// <param name="request"></param>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppServiceResponse<SearchOrderModelResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(AppServiceResponse<ICollection<Notification>>))]
        public async Task<IActionResult> GetAll([FromQuery] SearchOrderModelRequest request)
        {
            if (ModelState.IsValid is false)
            {
                NotifyModelStateErrors();

                return GenerateResponse(HttpStatusCode.BadRequest, new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Getting Orders", false));
            }

            var result = await _orderService.GetAll(request);

            if (result.Success is false)
                return GenerateResponse(HttpStatusCode.BadRequest, result);

            return GenerateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Adds a new order using the properties supplied, returns a GUID reference for the order created
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppServiceResponse<CreateOrderModelResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(AppServiceResponse<ICollection<Notification>>))]
        public async Task<IActionResult> Create([FromBody] CreateOrderModelRequest request)
        {
            if (ModelState.IsValid is false)
            {
                NotifyModelStateErrors();

                return GenerateResponse(HttpStatusCode.BadRequest, new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Order", false));
            }

            var result = await _orderService.Create(request);

            if (result.Success is false)
                return GenerateResponse(HttpStatusCode.BadRequest, result);

            return GenerateResponse(HttpStatusCode.OK, result);
        }

        #endregion Public Methods
    }
}