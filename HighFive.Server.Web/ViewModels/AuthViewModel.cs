#region references

using System.ComponentModel.DataAnnotations;

#endregion

namespace HighFive.Server.Web.ViewModels
{
    public class AuthViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
