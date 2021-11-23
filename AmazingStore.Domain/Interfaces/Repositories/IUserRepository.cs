using AmazingStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazingStore.Domain.Shared.Enums;

namespace AmazingStore.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IDisposable
    {
        #region Public Methods

        Task<User> GetAsync(Guid id);
        Task<User> GetByUsernameAsync(string username);

        Task<IEnumerable<User>> GetAllAsync(string name, string username, string email, DateTime? startDate,
            DateTime? endDate, string sort, ESortDirection sortDirection);
        Task AddAsync(User user);
        void Update(User user);

        #endregion Public Methods
    }
}