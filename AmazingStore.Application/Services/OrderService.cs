using AmazingStore.Application.Interfaces;
using AmazingStore.Application.Models.Common;
using AmazingStore.Application.Models.Order.Request;
using AmazingStore.Domain.Entities;
using AmazingStore.Domain.Enums.Order;
using AmazingStore.Domain.Interfaces.Repositories;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Domain.Shared.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazingStore.Application.Models.Order.Response;

namespace AmazingStore.Application.Services
{
    public class OrderService : AppService, IOrderService
    {

        #region Private Fields

        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        #endregion Private Fields

        #region Public Constructors

        public OrderService(IUnitOfWork unitOfWork,
            INotifier notifier,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository)
            : base(unitOfWork, notifier)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IAppServiceResponse> Create(CreateOrderModelRequest request)
        {
            var user = await _userRepository.GetAsync(request.UserId);

            if (user is null)
            {
                Notify("UserId", "User not found.");

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Order", false));
            }

            if (request.Products.Any() is false)
            {
                Notify("Products", "Order Requires Products");

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Order", false));
            }

            var order = new Order(request.UserId, EOrderStatus.Received);

            foreach (var product in request.Products)
            {
                var productdata = await _productRepository.GetAsync(product.Id);

                if (productdata is null)
                {
                    Notify("ProductId", $"The product of id {product.Id} was not found");

                    return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Order", false));
                }

                order.AddOrderProduct(new OrderProduct(order.Id, product.Id, product.Quantity, product.Quantity));
            }

            if (order.IsValid())
                await _orderRepository.AddAsync(order);
            else
            {
                Notify(order.ValidationResult);

                return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Order", false));
            }

            if (await CommitAsync())
                return await Task.FromResult(new AppServiceResponse<CreateOrderModelResponse>(new CreateOrderModelResponse(order.Id), "Order Created Successfully", true));

            return await Task.FromResult(new AppServiceResponse<ICollection<Notification>>(GetAllNotifications(), "Error Creating Order", false));
        }

        public async Task<IAppServiceResponse> GetAll(SearchOrderModelRequest request)
        {
            var orders = await _orderRepository.GetAllAsync(request.OrderId, request.UserId, request.StartDate, request.EndDate, request.Sort, request.SortDirection);

            var searchOrderModelResponse = new List<SearchOrderModelResponse>();

            foreach (var order in orders)
            {
                var searchOrderModel = new SearchOrderModelResponse(order.Id, order.UserId, order.User.Name, order.Status);

                foreach (var orderProduct in order.OrderProducts)
                    searchOrderModel.Products.Add(new SearchOrderProductModelResponse(orderProduct.Product.Id, orderProduct.Product.Name, orderProduct.CurrentPrice, orderProduct.Quantity));

                searchOrderModelResponse.Add(searchOrderModel);
            }

            if (searchOrderModelResponse.Any() is false)
                return await Task.FromResult(new AppServiceResponse<List<SearchOrderModelResponse>>(searchOrderModelResponse, "No Orders Found", true));

            return await Task.FromResult(new AppServiceResponse<List<SearchOrderModelResponse>>(searchOrderModelResponse, "Orders obtained successfully", true));
        }

        #endregion Public Methods

    }
}