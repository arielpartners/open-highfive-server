using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace highfive_server.Models
{
    public class HighFiveUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public Organization Organization { get; set; }
    }
}
