using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Services;
using AmazingStore.Domain.Interfaces.Repositories;
using AmazingStore.Domain.Notifications;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Domain.Shared.Services;
using AmazingStore.Domain.Shared.UnitOfWork;
using AmazingStore.Infra.CrossCutting.AspNetFilters;
using AmazingStore.Infra.CrossCutting.Services.Communication;
using AmazingStore.Infra.Data.Context;
using AmazingStore.Infra.Data.Repositories;
using AmazingStore.Infra.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AmazingStore.Infra.CrossCutting.IoC
{
    public static class NativeInjector
    {
        #region Public Methods

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<AmazingStoreContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<ICommunicationApiService, CommunicationApiService>();
            services.AddScoped<GlobalExceptionHandlerFilter>();

            LoginIoCRegister(services);
            UserIoCRegister(services);
            ProductIoCRegister(services);
            OrderIoCRegister(services);
            OrderProductIoCRegister(services);

            return services;
        }

        #endregion Public Methods

        #region Private Methods

        private static void LoginIoCRegister(IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
        }

        private static void UserIoCRegister(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }

        private static void ProductIoCRegister(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
        }

        private static void OrderIoCRegister(IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
        }

        private static void OrderProductIoCRegister(IServiceCollection services)
        {
            services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        }

        #endregion Private Methods
    }
}