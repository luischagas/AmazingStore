using AmazingStore.Api.Controllers;
using AmazingStore.Application.Models.Common;
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
    public class ProductControllerTest : ModelStateTestController
    {
        #region Private Fields

        private readonly ProductController _productController;

        #endregion Private Fields

        #region Public Constructors

        public ProductControllerTest()
        {
            var mockNotifier = new Mock<INotifier>().Object;
            var service = new ProductServiceFake();

            _productController = new ProductController(mockNotifier, service);
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public void Add_Product_With_Success()
        {
            var createProductModelRequest = new CreateProductModelRequest()
            {
                Name = "Playstation 5",
                Description = "The best console",
                Price = 3500
            };

            ModelStateValidation(createProductModelRequest, _productController);

            var modelState = _productController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _productController.Create(createProductModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<CreateProductModelResponse>>(objectResponse.Value);

            var productItem = objectResponse.Value as AppServiceResponse<CreateProductModelResponse>;

            Assert.True(productItem.Success);
        }

        [Fact]
        public void Add_Product_Fail_Model_State_Validation()
        {
            var createProductModelRequest = new CreateProductModelRequest();

            ModelStateValidation(createProductModelRequest, _productController);

            var modelState = _productController.ModelState;

            Assert.True(modelState.Any());
        }

        [Fact]
        public void Add_Product_Fail_Existent_Product()
        {
            var createProductModelRequest = new CreateProductModelRequest()
            {
                Name = "Echo Dot",
                Description = "A virtual assistant",
                Price = 100
            };

            ModelStateValidation(createProductModelRequest, _productController);

            var modelState = _productController.ModelState;

            Assert.True(modelState.Any() is false);

            var productExistent = _productController.Create(createProductModelRequest);

            Assert.IsType<ObjectResult>(productExistent.Result);

            var objectResponse = productExistent.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<ICollection<Notification>>>(objectResponse.Value);

            var errorList = objectResponse.Value as AppServiceResponse<ICollection<Notification>>;

            Assert.True(errorList.Success is false);
        }

        [Fact]
        public void Get_All_With_Success()
        {
            var searchProductModelRequest = new SearchProductModelRequest();

            ModelStateValidation(searchProductModelRequest, _productController);

            var modelState = _productController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _productController.GetAll(searchProductModelRequest);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<List<SearchProductModelResponse>>>(objectResponse.Value);

            var listProducts = objectResponse.Value as AppServiceResponse<List<SearchProductModelResponse>>;

            Assert.Equal(2, listProducts.Data.Count);
        }

        [Fact]
        public void Update_Product_With_Success()
        {
            #region Create

            var createProductModelRequest = new CreateProductModelRequest
            {
                Name = "Echo Dot 2",
                Description = "A new virtual assistant",
                Price = 100
            };

            var productResponse = _productController.Create(createProductModelRequest);

            var productObjectResponse = productResponse.Result as ObjectResult;

            var product = productObjectResponse.Value as AppServiceResponse<CreateProductModelResponse>;

            #endregion

            var updateProductModelRequest = new UpdateProductModelRequest()
            {
                Name = "Alexa",
                Description = "A virtual assistant 2.0",
                Price = 200
            };

            ModelStateValidation(updateProductModelRequest, _productController);

            var modelState = _productController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _productController.Update(updateProductModelRequest, product.Data.Id);

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<UpdateProductModelResponse>>(objectResponse.Value);

            var productUpdated = objectResponse.Value as AppServiceResponse<UpdateProductModelResponse>;

            Assert.Equal(productUpdated.Data.Id, productUpdated.Data.Id);
            Assert.Equal(productUpdated.Data.Name, productUpdated.Data.Name);
            Assert.Equal(productUpdated.Data.Description, productUpdated.Data.Description);
            Assert.Equal(productUpdated.Data.Price, productUpdated.Data.Price);
        }

        [Fact]
        public void Update_Product_Fail_Model_State_Validation()
        {
            var updateProductModelRequest = new UpdateProductModelRequest();

            ModelStateValidation(updateProductModelRequest, _productController);

            var modelState = _productController.ModelState;

            Assert.True(modelState.Any());
        }

        [Fact]
        public void Update_Product_Fail_Product_Not_Found()
        {
            #region Update

            var updateProductModelRequest = new UpdateProductModelRequest()
            {
                Name = "Alexa",
                Description = "A virtual assistant 2.0",
                Price = 200
            };

            ModelStateValidation(updateProductModelRequest, _productController);

            var modelState = _productController.ModelState;

            Assert.True(modelState.Any() is false);

            var response = _productController.Update(updateProductModelRequest, Guid.NewGuid());

            Assert.IsType<ObjectResult>(response.Result);

            var objectResponse = response.Result as ObjectResult;

            Assert.IsType<AppServiceResponse<ICollection<Notification>>>(objectResponse.Value);

            var errorList = objectResponse.Value as AppServiceResponse<ICollection<Notification>>;

            Assert.True(errorList.Success is false);

            #endregion Update
        }

        #endregion Public Methods
    }
}