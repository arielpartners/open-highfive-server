using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace highfive_server.Models
{
    public class HighFiveContextSeedData
    {
        private HighFiveContext _context;
        //private UserManager<HighFiveUser> _userManager;

        public HighFiveContextSeedData(HighFiveContext context)
        {
            _context = context;
            //_userManager = userManager;
        }
        public async Task EnsureSeedData()
        {
            if (!_context.Organizations.Any())
            {
                var org = new Organization()
                {
                    Name = "Ariel Partners",
                    Values = new List<CorporateValue>()
                    {
                        new CorporateValue() { Name="Commitment", Description="Committed to the long term success and happiness of our customers, our people, and our partners" },
                        new CorporateValue() { Name="Courage", Description="To take on difficult challenges, to accept new ideas, to accept incremental failure" },
                        new CorporateValue() { Name="Excellence", Description="Always strive to exceed expectations and continuously improve" },
                        new CorporateValue() { Name="Integrity", Description="Always act honestly, ethically, and do the right thing even when it hurts " }
                    }
                };
                var user = new HighFiveUser()
                {
                    Email = "sam.hastings@arielpartners.com",
                    Organization = org
                };
                //await _userManager.CreateAsync(user, "password");
                _context.Users.Add(user);
                _context.Organizations.Add(org);
                await _context.SaveChangesAsync();
            }
        }
    }
}

  
