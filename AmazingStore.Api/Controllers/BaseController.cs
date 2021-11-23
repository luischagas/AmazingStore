using AmazingStore.Domain.Shared.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace AmazingStore.Api.Controllers
{
    public class BaseController : Controller
    {

        #region Private Fields

        private readonly INotifier _notifier;

        #endregion Private Fields

        #region Protected Constructors

        protected BaseController(INotifier notifier)
        {
            _notifier = notifier;
        }

        #endregion Protected Constructors

        #region Protected Methods

        protected IActionResult GenerateResponse(HttpStatusCode statusCode, object result)
            => StatusCode((int)statusCode, result);

        protected void NotifyModelStateErrors()
        {
            var modelErrors = ModelState.Select(m => new { m.Key, Error = m.Value.Errors.Select(e => e.ErrorMessage).ToList() }).ToList();

            var keys = ModelState.Keys.ToList();

            foreach (var model in modelErrors)
            {
                foreach (var error in model.Error)
                {
                    NotifyError(model.Key, error);
                }
            }
        }

        protected void NotifyError(string key, string message)
        {
            _notifier.Handle(new Notification(key, message));
        }

        protected List<Notification> GetAllNotifications()
        {
            return _notifier.GetAllNotifications();
        }

        #endregion Protected Methods

    }
}