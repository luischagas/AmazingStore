using AmazingStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazingStore.Domain.Shared.Enums;

namespace AmazingStore.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IDisposable
    {
        #region Public Methods

        Task<Product> GetAsync(Guid id);

        Task<Product> GetByNameAsync(string name);

        Task<IEnumerable<Product>> GetAllAsync(string name, string description, decimal? price, DateTime? startDate,
            DateTime? endDate, string sort, ESortDirection sortDirection);

        Task AddAsync(Product product);

        void Update(Product product);

        #endregion Public Methods
    }
}