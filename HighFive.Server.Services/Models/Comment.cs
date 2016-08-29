using System;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Services.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public HighFiveUser Sender { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
