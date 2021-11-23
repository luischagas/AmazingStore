using AmazingStore.Api.Controllers;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Login.Request;
using AmazingStore.Application.Models.Login.Response;
using AmazingStore.Application.Models.Order.Request;
using AmazingStore.Application.Models.Order.Response;
using AmazingStore.Application.Models.Product.Request;
using AmazingStore.Application.Models.Product.Response;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Tests.Fake;
using AmazingStore.Tests.TestUtilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AmazingStore.Tests.Controllers
{
    public class OrderControllerTest : ModelStateTestController
    {
        #region Private Fields

        private readonly OrderController _orderController;
        private readonly LoginController _loginController;
        private readonly ProductController _productController;

        #endregion Private Fields

        #region Public Constructors

        public OrderControllerTest()
        {
            var mockNotifier = new Mock<INotifier>().Object;
            var service = new OrderServiceFake();
            var serviceLogin = new LoginServiceFake(true);
            var serviceProduct = new ProductServiceFake(true);

            _orderController = new OrderController(mockNotifier, service);
            _loginController = new LoginController(mockNotifier, serviceLogin);
            _productController = new ProductController(mockNotifier, serviceProduct);
        }

        #endregion Public Constructors

        #region Public Methods
        [Fact]
        public void Add_Order_With_Success()
        {
            #region Create User

            var signUpModelRequest = new SignUpModelRequest()
            {
                Name = "Pedro Nascimento",
                Email = "eu@pedro.dev",
                Password = "Pedro@123",
                Username = "pedron"
            };

            var userResponse = _loginController.SignUp(signUpModelRequest);

            var userObjectResponse = userResponse.Result as ObjectResult;

            var user = userObjectResponse.Value as AppServiceResponse<SignUpResponse>;

            #endregion Create User

            #region Create Product

            var createProductModelRequest = new CreateProductModelRequest()
            {
                Name = "Nintendo Switch",
                Description = "Mario World",
                Price = 2000
            };

            var productResponse = _productController.Create(createProductModelRequest);

            var productObjectResponse = productResponse.Result as ObjectResult;

            var product = productObjectResponse.Value as AppServiceResponse<CreateProductModelResponse>;

            #endregion Create Product

            #region Create Order

            var orderModelRequest = new CreateOrderModelRequest()
            {
                UserId = user.Data.Id,
                Products = new List<CreateOrderProductModelRequest>()
                {
                    new()
                    {
                        Id = product.Data.Id,
                        CurrentPrice = 150,
                        Quantity = 2
                    }
                }
            };

            ModelStateValidation(orderModelRequest, _orderController);

            var modelState = _orderController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _orderController.Create(orderModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<CreateOrderModelResponse>>(objectResponse.Value);

            var productItem = objectResponse.Value as AppServiceResponse<CreateOrderModelResponse>;

            Assert.True(productItem.Success);

            #endregion Create Order
        }

        [Fact]
        public void Add_Order_Fail_User_Not_Found()
        {
            #region Create Product

            var createProductModelRequest = new CreateProductModelRequest()
            {
                Name = "FIFA 22",
                Description = "Soccer Game",
                Price = 350
            };

            var productResponse = _productController.Create(createProductModelRequest);

            var productObjectResponse = productResponse.Result as ObjectResult;

            var product = productObjectResponse.Value as AppServiceResponse<CreateProductModelResponse>;

            #endregion Create Product

            #region Create Order

            var orderModelRequest = new CreateOrderModelRequest()
            {
                UserId = Guid.NewGuid(),
                Products = new List<CreateOrderProductModelRequest>()
                {
                    new()
                    {
                        Id = product.Data.Id,
                        CurrentPrice = 150,
                        Quantity = 2
                    }
                }
            };

            ModelStateValidation(orderModelRequest, _orderController);

            var modelState = _orderController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _orderController.Create(orderModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<ICollection<Notification>>>(objectResponse.Value);

            var errorList = objectResponse.Value as AppServiceResponse<ICollection<Notification>>;

            Assert.True(errorList.Success is false);

            #endregion Create Order
        }

        [Fact]
        public void Add_Order_Fail_Without_Products()
        {
            #region Create User

            var signUpModelRequest = new SignUpModelRequest()
            {
                Name = "Mario Nascimento",
                Email = "eu@mario.dev",
                Password = "Mario@123",
                Username = "marioN"
            };

            var userResponse = _loginController.SignUp(signUpModelRequest);

            var userObjectResponse = userResponse.Result as ObjectResult;

            var user = userObjectResponse.Value as AppServiceResponse<SignUpResponse>;

            #endregion Create User

            #region Create Order

            var orderModelRequest = new CreateOrderModelRequest()
            {
                UserId = user.Data.Id,
                Products = new List<CreateOrderProductModelRequest>()
            };

            ModelStateValidation(orderModelRequest, _orderController);

            var modelState = _orderController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _orderController.Create(orderModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<ICollection<Notification>>>(objectResponse.Value);

            var errorList = objectResponse.Value as AppServiceResponse<ICollection<Notification>>;

            Assert.True(errorList.Success is false);

            #endregion Create Order
        }

        [Fact]
        public void Add_Order_Fail_With_Product_Not_Exist()
        {
            #region Create User

            var signUpModelRequest = new SignUpModelRequest()
            {
                Name = "Joao Nascimento",
                Email = "eu@joao.dev",
                Password = "joao@123",
                Username = "joaon"
            };

            var userResponse = _loginController.SignUp(signUpModelRequest);

            var userObjectResponse = userResponse.Result as ObjectResult;

            var user = userObjectResponse.Value as AppServiceResponse<SignUpResponse>;

            #endregion Create User

            #region Create Order

            var orderModelRequest = new CreateOrderModelRequest()
            {
                UserId = user.Data.Id,
                Products = new List<CreateOrderProductModelRequest>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        CurrentPrice = 150,
                        Quantity = 2
                    }
                }
            };

            ModelStateValidation(orderModelRequest, _orderController);

            var modelState = _orderController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _orderController.Create(orderModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<ICollection<Notification>>>(objectResponse.Value);

            var errorList = objectResponse.Value as AppServiceResponse<ICollection<Notification>>;

            Assert.True(errorList.Success is false);

            #endregion Create Order
        }

        [Fact]
        public void Get_All_With_Success()
        {
            var searchOrderModelRequest = new SearchOrderModelRequest();

            ModelStateValidation(searchOrderModelRequest, _orderController);

            var modelState = _orderController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _orderController.GetAll(searchOrderModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<List<SearchOrderModelResponse>>>(objectResponse.Value);

            var listOrders = objectResponse.Value as AppServiceResponse<List<SearchOrderModelResponse>>;

            Assert.Single(listOrders.Data);
        }


        #endregion Public Methods
    }
}