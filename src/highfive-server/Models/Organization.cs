using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace highfive_server.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<CorporateValue> Values { get; set; }
    }
}
