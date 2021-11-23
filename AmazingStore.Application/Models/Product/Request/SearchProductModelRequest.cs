using System;
using AmazingStore.Domain.Shared.Enums;

namespace AmazingStore.Application.Models.Product.Request
{
    public class SearchProductModelRequest
    {
        #region Private Fields

        private string _sort = "createdOn";

        private ESortDirection _sortDirection = ESortDirection.Desc;

        #endregion Private Fields

        #region Public Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
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