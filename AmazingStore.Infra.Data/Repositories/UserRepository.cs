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
    public class UserRepository : IUserRepository
    {
        #region Private Fields

        private readonly AmazingStoreContext _db;
        private readonly DbSet<User> _users;

        #endregion Private Fields

        #region Public Constructors

        public UserRepository(AmazingStoreContext db)
        {
            _db = db;
            _users = _db.Set<User>();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<User> GetAsync(Guid id)
        {
            return await _users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllAsync(string name, string username, string email, DateTime? startDate, DateTime? endDate, string sort, ESortDirection sortDirection)
        {
            List<Expression<Func<User, bool>>> expressionWhere = new();
            Expression<Func<User, object>> expressionSort = null;

            if (string.IsNullOrEmpty(name) is false)
                expressionWhere.Add(p => p.Name.ToLower().Contains(name.ToLower()));

            if (string.IsNullOrEmpty(username) is false)
                expressionWhere.Add(p => p.Username.ToLower() == username.ToLower());

            if (email is not null)
                expressionWhere.Add(p => p.Email.Address == email);

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

                    case "username":
                        expressionSort = p => p.Username;
                        break;

                    case "email":
                        expressionSort = p => p.Email.Address;
                        break;

                    case "createdon":
                        expressionSort = p => p.CreatedOn;
                        break;

                    default:
                        expressionSort = p => p.CreatedOn;
                        break;

                }
            }

            var query = _users.AsNoTrackingWithIdentityResolution();

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

        public async Task AddAsync(User user)
        {
            await _users
                .AddAsync(user);
        }

        public void Update(User user)
        {
            _users
                .Update(user);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods
    }
}