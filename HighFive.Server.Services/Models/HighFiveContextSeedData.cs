using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighFive.Server.Services.Models
{
    public class HighFiveContextSeedData
    {
        private HighFiveContext _context;
        private UserManager<HighFiveUser> _userManager;

        public HighFiveContextSeedData(HighFiveContext context, UserManager<HighFiveUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

                _context.Organizations.Add(org);
                await _context.SaveChangesAsync();

                
            }

            Organization existingOrg = _context.Organizations.FirstOrDefault();

            if (await _userManager.FindByEmailAsync("test.user@email.com") == null)
            {
                var user = new HighFiveUser()
                {
                    UserName = "Bob",
                    Email = "test.user@email.com",
                    Organization = existingOrg
                };
                //TODO Mark - adjust password requirements
                IdentityResult result = await _userManager.CreateAsync(user, "$*Uhhdddddoiu6667");
            }
        }
    }
}

  
