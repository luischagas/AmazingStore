using AmazingStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazingStore.Domain.Shared.Enums;

namespace AmazingStore.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IDisposable
    {
        #region Public Methods

        Task<Order> GetAsync(Guid id);

        Task<IEnumerable<Order>> GetAllByUserAsync(Guid userId);

        Task<IEnumerable<Order>> GetAllAsync(Guid orderId, Guid userId, DateTime? startDate, DateTime? endDate,
            string sort, ESortDirection sortDirection);

        Task AddAsync(Order order);

        #endregion Public Methods
    }
}