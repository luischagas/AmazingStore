using System;
using AmazingStore.Domain.Shared.Notifications;
using AmazingStore.Domain.Shared.UnitOfWork;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingStore.Application
{
    public abstract class AppService
    {
        #region Fields

        private readonly INotifier _notifier;

        private readonly IUnitOfWork _unitOfWork;

        #endregion Fields

        #region Constructors

        public AppService(IUnitOfWork unitOfWork, INotifier notifier)
        {
            _unitOfWork = unitOfWork;
            _notifier = notifier;
        }

        #endregion Constructors

        #region Methods

        public async Task<bool> CommitAsync()
        {
            try
            {
                if (await _unitOfWork.CommitAsync())
                    return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

            return await Task.FromResult(false);
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                Notify(error.PropertyName, error.ErrorMessage);
        }

        protected void Notify(string key, string mensagem)
        {
            _notifier.Handle(new Notification(key, mensagem));
        }

        protected List<Notification> GetAllNotifications()
        {
            return _notifier.GetAllNotifications();
        }

        #endregion Methods
    }
}