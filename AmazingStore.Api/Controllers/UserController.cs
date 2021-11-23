using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.User.Request;
using AmazingStore.Application.Models.User.Response;
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
    public class UserController : BaseController
    {
        #region Private Fields

        private readonly IUserService _userService;

        #endregion Private Fields

        #region Public Constructors

        public UserController(INotifier notifier,
            IUserService userService)
            : base(notifier)
        {
            _userService = userService;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Get all users being able to filter specific properties 
        /// </summary>
        /// <param name="request"></param>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppServiceResponse<SearchUserModelResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(AppServiceResponse<ICollection<Notification>>))]
        public async Task<IActionResult> GetAll([FromQuery] SearchUserModelRequest request)
        {
            if (ModelState.IsValid is false)
            {
                NotifyModelStateErrors();

                return GenerateResponse(HttpStatusCode.BadRequest, new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Getting Users", false));
            }

            var result = await _userService.GetAll(request);

            if (result.Success is false)
                return GenerateResponse(HttpStatusCode.BadRequest, result);

            return GenerateResponse(HttpStatusCode.OK, result);
        }

        #endregion Public Methods
    }
}