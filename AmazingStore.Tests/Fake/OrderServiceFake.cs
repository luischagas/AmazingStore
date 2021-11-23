using System;
using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Login.Request;
using AmazingStore.Application.Models.Login.Response;
using AmazingStore.Application.Models.Order.Request;
using AmazingStore.Application.Models.Product.Request;
using AmazingStore.Application.Models.Product.Response;
using AmazingStore.Application.Services;
using AmazingStore.Domain.Shared.Entities;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Infra.CrossCutting.Services.Communication;
using AmazingStore.Infra.Data.Repositories;
using AmazingStore.Infra.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AmazingStore.Tests.Fake
{
    public class OrderServiceFake : BaseFake, IOrderService
    {
        #region Private Fields

        private readonly IOrderService _orderService;

        #endregion Private Fields

        #region Public Constructors

        public OrderServiceFake()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var dbContext = CreateInMemoryDatabaseOptional();

            var mockUnitOfWork = new Mock<UnitOfWork>(dbContext).Object;

            var mockOrderRepository = new Mock<OrderRepository>(dbContext).Object;
            var mockUserRepository = new Mock<UserRepository>(dbContext).Object;
            var mockProductRepository = new Mock<ProductRepository>(dbContext).Object;
            var mockCommunicationApiService = new Mock<CommunicationApiService>().Object;
            var mockAuthOptions = Options.Create(configuration.GetSection("Auth0").Get<Auth0>());
            var mockSecurityOptions = Options.Create(configuration.GetSection("Security").Get<Security>());
            var mockNotifier = new Mock<INotifier>().Object;

            _orderService = new OrderService(mockUnitOfWork, mockNotifier, mockOrderRepository, mockUserRepository, mockProductRepository);

            ILoginService loginService = new LoginService(mockUnitOfWork, mockNotifier, mockUserRepository, mockCommunicationApiService, mockAuthOptions, mockSecurityOptions);

            IProductService productService = new ProductService(mockUnitOfWork, mockNotifier, mockProductRepository);

            var signUpModelRequest = new SignUpModelRequest
            {
                Name = "Teste Testando",
                Email = "eu@teste.dev",
                Username = "teste",
                Password = "Test@123"
            };

            var userResponse = loginService.SignUp(signUpModelRequest);

            var user = userResponse.Result as AppServiceResponse<SignUpResponse>;

            var createProductModelRequest = new CreateProductModelRequest
            {
                Name = "Funk Pop",
                Description = "Collectible item",
                Price = 120
            };

            var productResponse = productService.Create(createProductModelRequest);

            var product = productResponse.Result as AppServiceResponse<CreateProductModelResponse>;

            var orderModelRequest = new CreateOrderModelRequest()
            {
                UserId = user?.Data?.Id ?? Guid.Empty,
                Products = new List<CreateOrderProductModelRequest>()
                   {
                       new()
                       {
                           Id = product?.Data?.Id ?? Guid.Empty,
                           CurrentPrice = 150,
                           Quantity = 2
                       }
                   }
            };

            _orderService.Create(orderModelRequest);
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IAppServiceResponse> GetAll(SearchOrderModelRequest request)
        {
            return await _orderService.GetAll(request);
        }

        public async Task<IAppServiceResponse> Create(CreateOrderModelRequest request)
        {
            return await _orderService.Create(request);
        }

        #endregion Public Methods
    }
}