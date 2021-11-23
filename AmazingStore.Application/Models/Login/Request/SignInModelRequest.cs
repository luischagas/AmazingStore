using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AmazingStore.Application.Models.Login.Request
{
    public class SignInModelRequest
    {
        #region Public Properties

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Username must contain between 5 and 100 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must contain between 5 and 100 characters")]
        public string Password { get; set; }

        #endregion Public Properties
    }
}