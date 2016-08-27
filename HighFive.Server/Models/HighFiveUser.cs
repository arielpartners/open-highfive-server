using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Models
{
    public class HighFiveUser: IdentityUser
    {
        //[Required]
        //[Index("UserEmailIndex", IsUnique = true)]
        //[Remote("IsUserExists", "Home", ErrorMessage = "User Name already in use")]
        //[StringLength(100, MinimumLength = 5)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Organization Organization { get; set; }
    }
}
