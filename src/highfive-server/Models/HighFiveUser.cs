﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace highfive_server.Models
{
    public class HighFiveUser
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
