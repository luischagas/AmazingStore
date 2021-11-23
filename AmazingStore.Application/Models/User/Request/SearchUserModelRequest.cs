using AmazingStore.Domain.Shared.Enums;
using System;

namespace AmazingStore.Application.Models.User.Request
{
    public class SearchUserModelRequest
    {
        #region Private Fields

        private string _sort = "createdOn";

        private ESortDirection _sortDirection = ESortDirection.Desc;

        #endregion Private Fields

        #region Public Properties

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Sort
        {
            get => _sort;
            set => _sort = string.IsNullOrEmpty(value) is false ? value : "createdOn";
        }

        public ESortDirection SortDirection
        {
            get => _sortDirection;
            set => _sortDirection = value != ESortDirection.Desc ? value : _sortDirection;
        }

        #endregion Public Properties

    }
}