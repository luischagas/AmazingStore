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
    public class ProductRepository : IProductRepository
    {
        #region Private Fields

        private readonly AmazingStoreContext _db;
        private readonly DbSet<Product> _products;

        #endregion Private Fields

        #region Public Constructors

        public ProductRepository(AmazingStoreContext db)
        {
            _db = db;
            _products = _db.Set<Product>();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Product> GetAsync(Guid id)
        {
            return await _products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _products
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string name, string description, decimal? price, DateTime? startDate, DateTime? endDate, string sort, ESortDirection sortDirection)
        {
            List<Expression<Func<Product, bool>>> expressionWhere = new();
            Expression<Func<Product, object>> expressionSort = null;

            if (string.IsNullOrEmpty(name) is false)
                expressionWhere.Add(p => p.Name.ToLower().Contains(name.ToLower()));

            if (string.IsNullOrEmpty(description) is false)
                expressionWhere.Add(p => p.Description.ToLower().Contains(description.ToLower()));

            if (price is not null)
                expressionWhere.Add(p => p.Price == price);

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
                    case "name":
                        expressionSort = p => p.Name;
                        break;

                    case "description":
                        expressionSort = p => p.Description;
                        break;

                    case "createdon":
                        expressionSort = p => p.CreatedOn;
                        break;


                    default:
                        expressionSort = p => p.CreatedOn;
                        break;

                }
            }

            var query = _products.AsNoTrackingWithIdentityResolution();

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

        public async Task AddAsync(Product product)
        {
            await _products
                .AddAsync(product);
        }

        public void Update(Product product)
        {
            _products
                .Update(product);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods
    }
}