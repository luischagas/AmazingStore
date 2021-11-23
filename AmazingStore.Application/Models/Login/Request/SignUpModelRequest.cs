using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AmazingStore.Application.Models.Login.Request
{
    public class SignUpModelRequest
    {
        #region Public Properties

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must contain between 5 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "User Name must contain between 5 and 100 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must contain between 5 and 100 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must contain between 5 and 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid e-mail")]
        public string Email { get; set; }

        #endregion Public Properties
    }
}