using AmazingStore.Domain.Enums.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using AmazingStore.Domain.Shared.Entities;

namespace AmazingStore.Domain.Entities
{
    public class Order : Entity<Order>
    {
        #region Protected Fields

        protected IList<OrderProduct> _orderProducts;

        #endregion Protected Fields

        #region Public Constructors

        public Order(Guid userId, EOrderStatus status)
        {
            UserId = userId;
            Status = status;
            _orderProducts = new List<OrderProduct>();
        }

        #endregion Public Constructors

        protected Order()
        {
            _orderProducts = new List<OrderProduct>();
        }

        #region Public Properties

        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public EOrderStatus Status { get; private set; }
        public IEnumerable<OrderProduct> OrderProducts => _orderProducts;

        #endregion Public Properties

        #region Public Methods

        public void AddOrderProduct(OrderProduct orderProduct)
        {
            if (orderProduct.IsValid())
                _orderProducts.Add(orderProduct);
            else
                AddErrors(orderProduct.ValidationResult);
        }

        public void SetStatus(EOrderStatus status)
        {
            Status = status;
        }

        public override bool IsValid()
        {
            ValidateSurveyId();

            AddErrors(Validate(this));

            return ValidationResult.IsValid;
        }

        #endregion Public Methods

        #region Private Methods

        private void ValidateSurveyId()
        {
            RuleFor(i => i.UserId)
                .NotEmpty()
                .WithMessage("Id of User cannot be empty.");
        }

        #endregion Private Methods
    }
}