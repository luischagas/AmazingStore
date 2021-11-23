using AmazingStore.Api.Controllers;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.User.Request;
using AmazingStore.Application.Models.User.Response;
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
    public class UserControllerTest : ModelStateTestController
    {
        #region Private Fields

        private readonly UserController _userController;

        #endregion Private Fields

        #region Public Constructors

        public UserControllerTest()
        {
            var mockNotifier = new Mock<INotifier>().Object;
            var service = new UserServiceFake();

            _userController = new UserController(mockNotifier, service);
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public void Get_All_With_Success()
        {
            var searchUserModelRequest = new SearchUserModelRequest();

            ModelStateValidation(searchUserModelRequest, _userController);

            var modelState = _userController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _userController.GetAll(new SearchUserModelRequest());

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<List<SearchUserModelResponse>>>(objectResponse.Value);

            var listUsers = objectResponse.Value as AppServiceResponse<List<SearchUserModelResponse>>;

            Assert.Equal(1, listUsers.Data.Count);
        }

        #endregion Public Methods
    }
}