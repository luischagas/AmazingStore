using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Login.Request;
using AmazingStore.Application.Models.Login.Response;
using AmazingStore.Domain.Shared.Notifications;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AmazingStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : BaseController
    {
        #region Private Fields

        private readonly ILoginService _loginService;

        #endregion Private Fields

        #region Public Constructors

        public LoginController(INotifier notifier,
            ILoginService loginService)
            : base(notifier)
        {
            _loginService = loginService;
        }

        #endregion Public Constructors

        #region Public Methods


        /// <summary>
        /// Authenticates the user, returns a bearer token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppServiceResponse<SignInModelResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(AppServiceResponse<ICollection<Notification>>))]
        public async Task<IActionResult> SignIn([FromBody] SignInModelRequest request)
        {
            if (ModelState.IsValid is false)
            {
                NotifyModelStateErrors();

                return GenerateResponse(HttpStatusCode.BadRequest, new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating User", false));
            }

            var result = await _loginService.SignIn(request);

            if (result.Success is false)
                return GenerateResponse(HttpStatusCode.BadRequest, result);

            return GenerateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Adds a new user using the properties supplied, returns a GUID reference for the user created
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppServiceResponse<SignUpResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(AppServiceResponse<ICollection<Notification>>))]
        public async Task<IActionResult> SignUp([FromBody] SignUpModelRequest request)
        {
            if (ModelState.IsValid is false)
            {
                NotifyModelStateErrors();

                return GenerateResponse(HttpStatusCode.BadRequest, new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating User", false));
            }

            var result = await _loginService.SignUp(request);

            if (result.Success is false)
                return GenerateResponse(HttpStatusCode.BadRequest, result);

            return GenerateResponse(HttpStatusCode.OK, result);
        }

        #endregion Public Methods
    }
}