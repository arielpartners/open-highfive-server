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
        public string Organization { get; set; }
        public string ReportsTo { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        [Phone]
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
    }
}
