using AmazingStore.Application.Models.User.Request;
using System.Threading.Tasks;

namespace AmazingStore.Application.Interfaces
{
    public interface IUserService
    {
        #region Public Methods

        Task<IAppServiceResponse> GetAll(SearchUserModelRequest request);

        #endregion Public Methods
    }
}