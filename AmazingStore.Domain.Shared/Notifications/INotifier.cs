using System.Collections.Generic;

namespace AmazingStore.Domain.Shared.Notifications
{
    public interface INotifier
    {
        #region Methods

        List<Notification> GetAllNotifications();

        void Handle(Notification notification);

        bool HasNotification();

        #endregion Methods
    }
}