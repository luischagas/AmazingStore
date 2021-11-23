using System.IO;
using System.Reflection.Metadata.Ecma335;
using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Login.Request;
using AmazingStore.Application.Services;
using AmazingStore.Domain.Shared.Entities;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Domain.Shared.Services;
using AmazingStore.Infra.Data.Repositories;
using AmazingStore.Infra.Data.UnitOfWork;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using AmazingStore.Infra.CrossCutting.Services.Communication;
using AmazingStore.Infra.Data.Context;
using Microsoft.Extensions.Configuration;

namespace AmazingStore.Tests.Fake
{
    public class LoginServiceFake : BaseFake, ILoginService
    {
        #region Private Fields

        private readonly ILoginService _loginService;

        #endregion Private Fields

        #region Public Constructors

        public LoginServiceFake(bool keepDatabase = false)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            AmazingStoreContext dbContext;

            if (keepDatabase)
                dbContext = CreateInMemoryDatabaseOptional();
            else
                dbContext = CreateInMemoryDatabase();

            var mockUnitOfWork = new Mock<UnitOfWork>(dbContext).Object;

            var mockUserRepository = new Mock<UserRepository>(dbContext).Object;
            var mockCommunicationApiService = new Mock<CommunicationApiService>().Object;
            
            var mockAuthOptions = Options.Create(configuration.GetSection("Auth0").Get<Auth0>());
            var mockSecurityOptions = Options.Create(configuration.GetSection("Security").Get<Security>());

            var mockNotifier = new Mock<INotifier>().Object;

            _loginService = new LoginService(mockUnitOfWork, mockNotifier, mockUserRepository, mockCommunicationApiService, mockAuthOptions, mockSecurityOptions);

            var signUpModelRequest = new SignUpModelRequest()
            {
                Name = "Luis Felipe",
                Email = "eu@luisfelipe.dev",
                Password = "Test@321",
                Username = "luisfelipe"
            };

            _loginService.SignUp(signUpModelRequest);
        }

        #endregion Public Constructors

        public async Task<IAppServiceResponse> SignUp(SignUpModelRequest request)
        {
            return await _loginService.SignUp(request);
        }

        public async Task<IAppServiceResponse> SignIn(SignInModelRequest request)
        {
            return await _loginService.SignIn(request);
        }
    }
}