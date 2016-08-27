using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Models
{
    public class HighFiveUser: IdentityUser
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        //[Index("UserEmailIndex", IsUnique = true)]
        //[Remote("IsUserExists", "Home", ErrorMessage = "User Name already in use")]
        [StringLength(100, MinimumLength = 5)]
        public string Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Organization Organization { get; set; }
    }
}
