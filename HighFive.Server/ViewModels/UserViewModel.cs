#region references

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace highfive_server.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [Required]
        public string OrganizationName { get; set; }
    }
}
