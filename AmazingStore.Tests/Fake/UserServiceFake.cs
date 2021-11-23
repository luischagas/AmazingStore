using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Login.Request;
using AmazingStore.Application.Models.User.Request;
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
    public class UserServiceFake : BaseFake, IUserService
    {
        #region Private Fields

        private readonly IUserService _userService;

        #endregion Private Fields

        #region Public Constructors

        public UserServiceFake()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var dbContext = CreateInMemoryDatabase();

            var mockUnitOfWork = new Mock<UnitOfWork>(dbContext).Object;

            var mockUserRepository = new Mock<UserRepository>(dbContext).Object;
            var mockCommunicationApiService = new Mock<CommunicationApiService>().Object;

            var mockAuthOptions = Options.Create(configuration.GetSection("Auth0").Get<Auth0>());
            var mockSecurityOptions = Options.Create(configuration.GetSection("Security").Get<Security>());

            var mockNotifier = new Mock<INotifier>().Object;

            var loginService = new LoginService(mockUnitOfWork, mockNotifier, mockUserRepository, mockCommunicationApiService, mockAuthOptions, mockSecurityOptions);

            _userService = new UserService(mockUnitOfWork, mockNotifier, mockUserRepository);

            var listUsersModelRequest = new List<SignUpModelRequest>
            {
                new()
                {
                    Name = "Luis Chagas",
                    Email = "eu@luischagas.dev",
                    Username = "luischagas",
                    Password = "Test@123"
                },

                new()
                {
                    Name = "Théo Silva",
                    Email = "eu@theo.dog",
                    Username = "theo",
                    Password = "Fake@123"
                },
            };

            foreach (var userModelRequest in listUsersModelRequest)
                loginService.SignUp(userModelRequest);
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IAppServiceResponse> GetAll(SearchUserModelRequest request)
        {
            return await _userService.GetAll(request);
        }

        #endregion Public Methods
    }
}