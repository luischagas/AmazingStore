using System;
using AmazingStore.Application.Models.Product.Request;
using System.Threading.Tasks;

namespace AmazingStore.Application.Interfaces
{
    public interface IProductService
    {
        #region Public Methods

        Task<IAppServiceResponse> GetAll(SearchProductModelRequest request);

        Task<IAppServiceResponse> Create(CreateProductModelRequest request);

        Task<IAppServiceResponse> Update(UpdateProductModelRequest request, Guid id);

        #endregion Public Methods
    }
}