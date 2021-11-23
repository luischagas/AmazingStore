using AmazingStore.Application.Models.Login.Request;
using System.Threading.Tasks;

namespace AmazingStore.Application.Interfaces
{
    public interface ILoginService
    {

        #region Public Methods

        Task<IAppServiceResponse> SignUp(SignUpModelRequest request);

        Task<IAppServiceResponse> SignIn(SignInModelRequest request);

        #endregion Public Methods

    }
}