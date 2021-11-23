using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazingStore.Domain.Entities;
using AmazingStore.Domain.Interfaces.Repositories;
using AmazingStore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AmazingStore.Infra.Data.Repositories
{
    public class OrderProductRepository : IOrderProductRepository
    {

        #region Private Fields

        private readonly AmazingStoreContext _db;
        private readonly DbSet<OrderProduct> _orderProducts;

        #endregion Private Fields

        #region Public Constructors

        public OrderProductRepository(AmazingStoreContext db)
        {
            _db = db;
            _orderProducts = _db.Set<OrderProduct>();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<OrderProduct> GetAsync(Guid id)
        {
            return await _orderProducts
                .FirstOrDefaultAsync(op => op.Id == id);
        }

        public async Task<IEnumerable<OrderProduct>> GetAllByOrderAsync(Guid orderId)
        {
            return await _orderProducts
                .AsNoTrackingWithIdentityResolution()
                .Where(op => op.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderProduct>> GetAllAsync()
        {
            return await _orderProducts
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();
        }

        public async Task AddAsync(OrderProduct orderProduct)
        {
            await _orderProducts
                .AddAsync(orderProduct);
        }

        public void Update(OrderProduct orderProduct)
        {
            _orderProducts
                .Update(orderProduct);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods
    }
}
