#region references

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace HighFive.Server.Services.Models
{
    public class Organization
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        [StringLength(100)]
        public string UrlPath { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public ICollection<CorporateValue> Values { get; set; }
    }
}
