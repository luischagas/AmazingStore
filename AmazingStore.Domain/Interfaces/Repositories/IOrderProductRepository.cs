using AmazingStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingStore.Domain.Interfaces.Repositories
{
    public interface IOrderProductRepository : IDisposable
    {
        #region Public Methods

        Task<OrderProduct> GetAsync(Guid id);

        Task<IEnumerable<OrderProduct>> GetAllByOrderAsync(Guid orderId);

        Task<IEnumerable<OrderProduct>> GetAllAsync();

        Task AddAsync(OrderProduct orderProduct);

        void Update(OrderProduct orderProduct);

        #endregion Public Methods
    }
}