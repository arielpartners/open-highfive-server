using System;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Services.Models
{
    public class CorporateValue
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
    }
}