using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Services.Models
{
    public class Recognition
    {
        public int Id { get; set; }
        public HighFiveUser Sender { get; set; }
        public HighFiveUser Receiver { get; set; }
        public Organization Organization { get; set; }
        public CorporateValue Value { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [StringLength(1000)]
        public string Description { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
