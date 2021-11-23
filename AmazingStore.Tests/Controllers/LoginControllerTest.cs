using AmazingStore.Api.Controllers;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Login.Request;
using AmazingStore.Application.Models.Login.Response;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Tests.Fake;
using AmazingStore.Tests.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AmazingStore.Tests.Controllers
{
    public class LoginControllerTest : ModelStateTestController
    {
        #region Private Fields

        private readonly LoginController _loginController;

        #endregion Private Fields

        #region Public Constructors

        public LoginControllerTest()
        {
            var mockNotifier = new Mock<INotifier>().Object;
            var service = new LoginServiceFake();

            _loginController = new LoginController(mockNotifier, service);
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public void SignUp_With_Sucess()
        {
            var signUpModelRequest = new SignUpModelRequest()
            {
                Name = "Yasmine Silva",
                Email = "eu@yasminne.com",
                Password = "Test@12345",
                Username = "yasminne"
            };

            ModelStateValidation(signUpModelRequest, _loginController);

            var modelState = _loginController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _loginController.SignUp(signUpModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<SignUpResponse>>(objectResponse.Value);

            var signUpResponse = objectResponse.Value as AppServiceResponse<SignUpResponse>;

            Assert.True(signUpResponse.Success);

            Assert.Equal(signUpResponse.Data.Username, signUpModelRequest.Username);
        }

        [Fact]
        public void SignUp_Fail_Model_State_Validation()
        {
            var signUpModelRequest = new SignUpModelRequest();

            ModelStateValidation(signUpModelRequest, _loginController);

            var modelState = _loginController.ModelState;

            Assert.True(modelState.Any());
        }

        [Fact]
        public void SignUp_Fail_Existent_Username()
        {
            var signUpModelRequest = new SignUpModelRequest()
            {
                Name = "Luis Felipe",
                Email = "eu@luisfelipe.dev",
                Password = "Test@321",
                Username = "luisfelipe"
            };

            ModelStateValidation(signUpModelRequest, _loginController);

            var modelState = _loginController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _loginController.SignUp(signUpModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<ICollection<Notification>>>(objectResponse.Value);

            var errorList = objectResponse.Value as AppServiceResponse<ICollection<Notification>>;

            Assert.True(errorList.Success is false);
        }

        [Fact]
        public void SignIn_With_Sucess()
        {
            var signInModelRequest = new SignInModelRequest()
            {
                Username = "luisfelipe",
                Password = "Test@321"
            };

            ModelStateValidation(signInModelRequest, _loginController);

            var modelState = _loginController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _loginController.SignIn(signInModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<SignInModelResponse>>(objectResponse.Value);

            var signUpResponse = objectResponse.Value as AppServiceResponse<SignInModelResponse>;

            Assert.True(signUpResponse.Success);
        }

        [Fact]
        public void SignIn_With_Fail_Wrong_Credentials()
        {
            var signInModelRequest = new SignInModelRequest()
            {
                Username = "luisfelipe",
                Password = "Test@123"
            };

            ModelStateValidation(signInModelRequest, _loginController);

            var modelState = _loginController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _loginController.SignIn(signInModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<ICollection<Notification>>>(objectResponse.Value);

            var errorList = objectResponse.Value as AppServiceResponse<ICollection<Notification>>;

            Assert.True(errorList.Success is false);
        }

        [Fact]
        public void SignIn_Fail_Model_State_Validation()
        {
            var signInModelRequest = new SignInModelRequest();

            ModelStateValidation(signInModelRequest, _loginController);

            var modelState = _loginController.ModelState;

            Assert.True(modelState.Any());
        }

        #endregion Public Methods
    }
}