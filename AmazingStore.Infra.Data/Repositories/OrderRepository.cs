using AmazingStore.Domain.Entities;
using AmazingStore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AmazingStore.Domain.Interfaces.Repositories;
using AmazingStore.Domain.Shared.Enums;
using AmazingStore.Domain.Shared.Extensions;

namespace AmazingStore.Infra.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        #region Private Fields

        private readonly AmazingStoreContext _db;
        private readonly DbSet<Order> _orders;

        #endregion Private Fields

        #region Public Constructors

        public OrderRepository(AmazingStoreContext db)
        {
            _db = db;
            _orders = _db.Set<Order>();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Order> GetAsync(Guid id)
        {
            return await _orders
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllByUserAsync(Guid userId)
        {
            return await _orders
                .AsNoTrackingWithIdentityResolution()
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync(Guid orderId, Guid userId, DateTime? startDate, DateTime? endDate, string sort, ESortDirection sortDirection)
        {
            List<Expression<Func<Order, bool>>> expressionWhere = new();
            Expression<Func<Order, object>> expressionSort = null;

            if (orderId != Guid.Empty)
                expressionWhere.Add(p => p.Id == orderId);

            if (userId != Guid.Empty)
                expressionWhere.Add(p => p.UserId == userId);

            if (startDate is not null && endDate is null)
                expressionWhere.Add(p => p.CreatedOn.Date >= startDate.Value.Date);

            if (endDate is not null && startDate is null)
                expressionWhere.Add(p => p.CreatedOn.Date >= endDate.Value.Date);

            if (startDate is not null && endDate is not null)
                expressionWhere.Add(p => p.CreatedOn.Date >= startDate.Value.Date && p.CreatedOn.Date <= endDate.Value.Date);

            if (string.IsNullOrEmpty(sort) is false)
            {
                switch (sort.ToLower())
                {
                    case "orderid":
                        expressionSort = p => p.Id;
                        break;

                    case "userid":
                        expressionSort = p => p.UserId;
                        break;

                    case "createdon":
                        expressionSort = p => p.CreatedOn;
                        break;

                    default:
                        expressionSort = p => p.CreatedOn;
                        break;

                }
            }

            var query = _orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .AsNoTrackingWithIdentityResolution();

            if (expressionWhere.Any())
            {
                var combinedExpression = expressionWhere.Aggregate((x, y) => x.AndAlso(y));

                query = query.Where(combinedExpression);
            }

            query = query
                .OrderBy(expressionSort,
                    sortDirection == ESortDirection.Asc
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending);

            return await query.ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _orders
                .AddAsync(order);
        }

        public void Update(Order order)
        {
            _orders
                .Update(order);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

    }
}