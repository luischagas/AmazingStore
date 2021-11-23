using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Product.Request;
using AmazingStore.Application.Models.Product.Response;
using AmazingStore.Application.Services;
using AmazingStore.Domain.Entities;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Infra.Data.Repositories;
using AmazingStore.Infra.Data.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazingStore.Infra.Data.Context;

namespace AmazingStore.Tests.Fake
{
    public class ProductServiceFake : BaseFake, IProductService
    {
        #region Private Fields

        private readonly IProductService _productService;

        #endregion Private Fields

        #region Public Constructors

        public ProductServiceFake(bool keepDatabase = false)
        {
            AmazingStoreContext dbContext;

            if (keepDatabase)
                dbContext = CreateInMemoryDatabaseOptional();
            else
                dbContext = CreateInMemoryDatabase();

            var mockUnitOfWork = new Mock<UnitOfWork>(dbContext).Object;

            var mockProductRepository = new Mock<ProductRepository>(dbContext).Object;

            var mockNotifier = new Mock<INotifier>().Object;

            _productService = new ProductService(mockUnitOfWork, mockNotifier, mockProductRepository);

            var listProductModelRequest = new List<CreateProductModelRequest>
            {
                new()
                {
                    Name = "Echo Dot",
                    Description = "A virtual assistant",
                    Price = 100
                },

                new()
                {
                    Name = "Iphone 13",
                    Description = "The last generation",
                    Price = 1000
                }
            };

            foreach (var productModelRequest in listProductModelRequest)
                _productService.Create(productModelRequest);
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IAppServiceResponse> GetAll(SearchProductModelRequest request)
        {
            return await _productService.GetAll(request);
        }

        public async Task<IAppServiceResponse> Create(CreateProductModelRequest request)
        {
            return await _productService.Create(request);
        }

        public async Task<IAppServiceResponse> Update(UpdateProductModelRequest request, Guid id)
        {
            return await _productService.Update(request, id);
        }

        #endregion Public Methods
    }
}