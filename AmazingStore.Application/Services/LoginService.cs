using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Domain.Entities;
using AmazingStore.Domain.Interfaces.Repositories;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Domain.Shared.UnitOfWork;
using AmazingStore.Domain.Shared.Utils;
using AmazingStore.Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazingStore.Application.Models.Login;
using AmazingStore.Application.Models.Login.Request;
using AmazingStore.Application.Models.Login.Response;
using AmazingStore.Domain.Shared.Entities;
using AmazingStore.Domain.Shared.Services;
using AmazingStore.Infra.CrossCutting.Services.Communication.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace AmazingStore.Application.Services
{
    public class LoginService : AppService, ILoginService
    {
        #region Private Fields

        private readonly IUserRepository _userRepository;
        private readonly ICommunicationApiService _communicationApiService;
        private readonly Auth0 _authSettings;
        private readonly Security _securitySettings;

        #endregion Private Fields

        #region Public Constructors

        public LoginService(IUnitOfWork unitOfWork,
            INotifier notifier,
            IUserRepository userRepository, 
            ICommunicationApiService communicationApiService, 
            IOptions<Auth0> authSettings, 
            IOptions<Security> securitySettings)
            : base(unitOfWork, notifier)
        {
            _userRepository = userRepository;
            _communicationApiService = communicationApiService;
            _authSettings = authSettings.Value;
            _securitySettings = securitySettings.Value;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IAppServiceResponse> SignUp(SignUpModelRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user is not null)
            {
                Notify("Username", "User already exist.");

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating User", false));
            }

            var email = new Email(request.Email);

            var newUser = new User(request.Name, request.Username, Util.Encrypt(request.Password, _securitySettings.Key), email);

            if (newUser.IsValid())
                await _userRepository.AddAsync(newUser);
            else
            {
                Notify(newUser.ValidationResult);

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating User", false));
            }

            if (await CommitAsync())
                return await Task.FromResult(new AppServiceResponse<SignUpResponse>(new SignUpResponse(newUser.Id, newUser.Username), "User Created Successfully", true));

            return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating User", false));
        }

        public async Task<IAppServiceResponse> SignIn(SignInModelRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user is null)
            {
                Notify("Username", "Invalid username and password");

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error when logging in", false));

            }

            if (Util.Decrypt(user.Password, _securitySettings.Key) != request.Password)
            {
                Notify("Username", "Invalid username and password");

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error when logging in", false));
            }

            var authModelRequest = new AuthModelRequest()
            {
                ClientId = _authSettings.ClientId,
                ClientSecret = _authSettings.ClientSecret,
                Audience = _authSettings.Audience,
                GrantType = "client_credentials"
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true }
                },

                Formatting = Formatting.Indented
            };

            var body = JsonConvert.SerializeObject(authModelRequest, settings);

            var response = _communicationApiService.SendRequest($"https://{_authSettings.Domain}/oauth/token",
                Method.POST, body);

            if (response.IsSuccessful)
            {
                var authModelResponse = JsonConvert.DeserializeObject<AuthModelResponse>(response.Content, settings);

                if (authModelResponse is null)
                {
                    Notify("Token", "Failed to get token");
                    return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error when logging in", false));
                }
                
                return await Task.FromResult(new AppServiceResponse<SignInModelResponse>(new SignInModelResponse(authModelResponse.AccessToken), "Login Successfully", true));
            }

            Notify("Auth0", "Failed to authenticate");

            return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error when logging in", false));
        }

        #endregion Public Methods
    }
}