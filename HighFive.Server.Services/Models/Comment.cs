using System;

namespace HighFive.Server.Services.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public HighFiveUser Sender { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
