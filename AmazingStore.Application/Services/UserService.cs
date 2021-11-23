using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.User.Request;
using AmazingStore.Application.Models.User.Response;
using AmazingStore.Domain.Interfaces.Repositories;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Domain.Shared.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingStore.Application.Services
{
    public class UserService : AppService, IUserService
    {
        #region Private Fields

        private readonly IUserRepository _userRepository;

        #endregion Private Fields

        #region Public Constructors

        public UserService(IUnitOfWork unitOfWork,
            INotifier notifier,
            IUserRepository userRepository)
            : base(unitOfWork, notifier)
        {
            _userRepository = userRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IAppServiceResponse> GetAll(SearchUserModelRequest request)
        {
            var users = await _userRepository.GetAllAsync(request.Name, request.UserName, request.Email, request.StartDate, request.EndDate, request.Sort, request.SortDirection);

            var searcUserModelResponse = new List<SearchUserModelResponse>();

            foreach (var user in users)
                searcUserModelResponse.Add(new SearchUserModelResponse(user.Id, user.Name, user.Username, user.Email.Address, user.CreatedOn));

            if (searcUserModelResponse.Any() is false)
                return await Task.FromResult(new AppServiceResponse<List<SearchUserModelResponse>>(searcUserModelResponse, "No Users Found", true));

            return await Task.FromResult(new AppServiceResponse<List<SearchUserModelResponse>>(searcUserModelResponse, "Users obtained successfully", true));
        }

        #endregion Public Methods
    }
}