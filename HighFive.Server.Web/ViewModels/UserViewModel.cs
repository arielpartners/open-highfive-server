#region references

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace HighFive.Server.Web.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [Required]
        public string OrganizationName { get; set; }
    }
}
