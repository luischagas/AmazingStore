using AmazingStore.Application.Models.Order.Request;
using System.Threading.Tasks;

namespace AmazingStore.Application.Interfaces
{
    public interface IOrderService
    {

        #region Public Methods

        Task<IAppServiceResponse> Create(CreateOrderModelRequest request);

        Task<IAppServiceResponse> GetAll(SearchOrderModelRequest request);

        #endregion Public Methods

    }
}