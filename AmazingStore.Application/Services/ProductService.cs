using System;
using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Product.Request;
using AmazingStore.Application.Models.Product.Response;
using AmazingStore.Domain.Entities;
using AmazingStore.Domain.Interfaces.Repositories;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Domain.Shared.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingStore.Application.Services
{
    public class ProductService : AppService, IProductService
    {

        #region Private Fields

        private readonly IProductRepository _productRepository;

        #endregion Private Fields

        #region Public Constructors

        public ProductService(IUnitOfWork unitOfWork,
            INotifier notifier,
            IProductRepository productRepository)
            : base(unitOfWork, notifier)
        {
            _productRepository = productRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IAppServiceResponse> Create(CreateProductModelRequest request)
        {
            var product = await _productRepository.GetByNameAsync(request.Name);

            if (product is not null)
            {
                Notify("Name", "Product already exist.");

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Product", false));
            }

            var newProduct = new Product(request.Name, request.Description, request.Price);

            if (newProduct.IsValid())
                await _productRepository.AddAsync(newProduct);
            else
            {
                Notify(newProduct.ValidationResult);

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Product", false));
            }

            if (await CommitAsync())
                return await Task.FromResult(new AppServiceResponse<CreateProductModelResponse>(new CreateProductModelResponse(newProduct.Id), "Product Created Successfully", true));

            return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Product", false));
        }

        public async Task<IAppServiceResponse> Update(UpdateProductModelRequest request, Guid id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product is null)
            {
                Notify("Id", "Product not found");

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Updating Product", false));
            }

            product.Update(request.Name, request.Description, request.Price);

            if (product.IsValid())
                _productRepository.Update(product);
            else
            {
                Notify(product.ValidationResult);

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Updating Product", false));
            }

            if (await CommitAsync())
                return await Task.FromResult(new AppServiceResponse<UpdateProductModelResponse>(new UpdateProductModelResponse(product.Id, product.Name, product.Description, product.Price), "Product Updated Successfully", true));

            return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Updating Product", false));
        }

        public async Task<IAppServiceResponse> GetAll(SearchProductModelRequest request)
        {
            var products = await _productRepository.GetAllAsync(request.Name, request.Description, request.Price, request.StartDate, request.EndDate, request.Sort, request.SortDirection);

            var searchProductModelResponse = new List<SearchProductModelResponse>();

            foreach (var product in products)
                searchProductModelResponse.Add(new SearchProductModelResponse(product.Id, product.Name, product.Description, product.Price, product.CreatedOn));

            if (searchProductModelResponse.Any() is false)
                return await Task.FromResult(new AppServiceResponse<List<SearchProductModelResponse>>(searchProductModelResponse, "No Products Found", true));

            return await Task.FromResult(new AppServiceResponse<List<SearchProductModelResponse>>(searchProductModelResponse, "Products obtained successfully", true));
        }

        #endregion Public Methods

    }
}