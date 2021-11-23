using AmazingStore.Domain.Shared.Enums;
using System;

namespace AmazingStore.Application.Models.Order.Request
{
    public class SearchOrderModelRequest
    {
        #region Private Fields

        private string _sort = "createdOn";

        private ESortDirection _sortDirection = ESortDirection.Desc;

        #endregion Private Fields

        #region Public Properties

        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
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