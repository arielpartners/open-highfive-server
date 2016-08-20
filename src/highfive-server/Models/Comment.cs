using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace highfive_server.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public HighFiveUser Sender { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
