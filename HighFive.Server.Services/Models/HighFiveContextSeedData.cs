using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace HighFive.Server.Services.Models
{
    [ExcludeFromCodeCoverage]
    public class HighFiveContextSeedData
    {
        private readonly HighFiveContext _context;
        private readonly UserManager<HighFiveUser> _userManager;

        public HighFiveContextSeedData(HighFiveContext context, UserManager<HighFiveUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task EnsureSeedData()
        {
            try
            {
                var org = _context.Organizations.FirstOrDefault(x => x.Name == "Ariel Partners");
                var userLst = new List<HighFiveUser>();
                if (org == null)
                {
                    _context.Organizations.Add(new Organization
                    {
                        Name = "Ariel Partners",
                        WebPath = "ariel-partners",
                        Values = new List<CorporateValue>
                            {
                                new CorporateValue { Name="Commitment", Description="Committed to the long term success and happiness of our customers, our people, and our partners" },
                                new CorporateValue { Name="Courage", Description="To take on difficult challenges, to accept new ideas, to accept incremental failure" },
                                new CorporateValue { Name="Excellence", Description="Always strive to exceed expectations and continuously improve" },
                                new CorporateValue { Name="Integrity", Description="Always act honestly, ethically, and do the right thing even when it hurts" },
                                new CorporateValue { Name="Honesty", Description="Always act honestly, ethically, and do the right thing even when it hurts" },
                                new CorporateValue { Name="Vigilance", Description="Always act honestly, ethically, and do the right thing even when it hurts" },
                                new CorporateValue { Name="Respect", Description="Always act honestly, ethically, and do the right thing even when it hurts" }
                            }
                    });
                    await _context.SaveChangesAsync();
                }
                //Users
                org = _context.Organizations.FirstOrDefault();
                var testUserLst1 = new List<HighFiveUser>
                            {
                                new HighFiveUser{UserName = "test.user@email.com",Email = "test.user@email.com",FirstName = "Test",LastName = "User",Organization = org},
                                new HighFiveUser{UserName = "John.Doe@email.com",Email = "John.Doe@email.com",FirstName = "John",LastName = "Doe",Organization = org},
                                new HighFiveUser{UserName = "Joe.Blah@email.com",Email = "Joe.Blah@email.com",FirstName = "Joe",LastName = "Blah",Organization = org},
                                new HighFiveUser{UserName = "Jane.Doe@email.com",Email = "Jane.Doe@email.com",FirstName = "Jane",LastName = "Doe",Organization = org},
                                new HighFiveUser{UserName = "Mathew.Anderson@email.com",Email = "Mathew.Anderson@email.com",FirstName = "Mathew",LastName = "Anderson",Organization = org},
                                new HighFiveUser{UserName = "Kate.Boxed@email.com",Email = "Kate.Boxed@email.com",FirstName = "Kate",LastName = "Boxed",Organization = org},
                                new HighFiveUser{UserName = "Nova.Down@email.com",Email = "Nova.Down@email.com",FirstName = "Nova",LastName = "Down",Organization = org}
                            };
                userLst.AddRange(testUserLst1);
                org = _context.Organizations.FirstOrDefault(x => x.Name == "DHS");
                if (org == null)
                {
                    _context.Organizations.Add(new Organization
                    {
                        Name = "DHS",
                        WebPath = "dhs",
                        Values = new List<CorporateValue>
                            {
                                new CorporateValue { Name="Teamwork", Description="the combined action of a group of people, especially when effective and efficient" },
                                new CorporateValue { Name="Improvement", Description="a thing that makes something better or is better than something else" },
                                new CorporateValue { Name="Delivery", Description="the action of delivering letters, packages, or ordered goods" },
                                new CorporateValue { Name="Experiment", Description="a scientific procedure undertaken to make a discovery, test a hypothesis, or demonstrate a known fact" }
                            }
                    });
                    await _context.SaveChangesAsync();

                }
                testUserLst1 = new List<HighFiveUser>
                            {
                                new HighFiveUser{UserName = "test.user@hq.dhs.gov",Email = "test.user@hq.dhs.gov",FirstName = "Test",LastName = "User",Organization = org},
                                new HighFiveUser{UserName = "John.Doe@hq.dhs.gov",Email = "John.Doe@hq.dhs.gov",FirstName = "John",LastName = "Doe",Organization = org},
                                new HighFiveUser{UserName = "Joe.Blah@hq.dhs.gov",Email = "Joe.Blah@hq.dhs.gov",FirstName = "Joe",LastName = "Blah",Organization = org},
                                new HighFiveUser{UserName = "Jane.Doe@hq.dhs.gov",Email = "Jane.Doe@hq.dhs.gov",FirstName = "Jane",LastName = "Doe",Organization = org},
                                new HighFiveUser{UserName = "Mathew.Anderson@hq.dhs.gov",Email = "Mathew.Anderson@hq.dhs.gov",FirstName = "Mathew",LastName = "Anderson",Organization = org},
                                new HighFiveUser{UserName = "Kate.Boxed@hq.dhs.gov",Email = "Kate.Boxed@hq.dhs.gov",FirstName = "Kate",LastName = "Boxed",Organization = org},
                                new HighFiveUser{UserName = "Nova.Down@hq.dhs.gov",Email = "Nova.Down@hq.dhs.gov",FirstName = "Nova",LastName = "Down",Organization = org}
                            };
                userLst.AddRange(testUserLst1);
                foreach (var usr in userLst)
                {
                    if (await _userManager.FindByEmailAsync(usr.UserName) != null) continue;
                    await _userManager.CreateAsync(usr, "password");
                }

                //Recognitions
                //if (!_context.Recognitions.Any())
                //{
                //    var test = await _userManager.FindByEmailAsync("test.user@email.com");
                //    var john = await _userManager.FindByEmailAsync("John.Doe@email.com");
                //    var joe = await _userManager.FindByEmailAsync("Joe.Blah@email.com");
                //    var jane = await _userManager.FindByEmailAsync("Jane.Doe@email.com");
                //    var mathew = await _userManager.FindByEmailAsync("Mathew.Anderson@email.com");
                //    var kate = await _userManager.FindByEmailAsync("Kate.Boxed@email.com");
                //    var nova = await _userManager.FindByEmailAsync("Nova.Down@email.com");

                //    var commitment = _context.CorporateValues.First(x => x.Name == "Commitment");
                //    var courage = _context.CorporateValues.First(x => x.Name == "Courage");
                //    var excellence = _context.CorporateValues.First(x => x.Name == "Excellence");
                //    var integrity = _context.CorporateValues.First(x => x.Name == "Integrity");
                //    var honesty = _context.CorporateValues.First(x => x.Name == "Honesty");
                //    var vigilance = _context.CorporateValues.First(x => x.Name == "Vigilance");
                //    var respect = _context.CorporateValues.First(x => x.Name == "Respect");

                //    var recognitionLst = new List<Recognition>
                //        {
                //            new Recognition{Sender = test,Receiver = john,Organization = org,Value = commitment,DateCreated = Convert.ToDateTime("8/2/2016 4:0:9 PM").ToUniversalTime(),Description = "you are awesome",Points = 10},
                //            new Recognition{Sender = joe,Receiver = john,Organization = org,Value = courage,DateCreated = Convert.ToDateTime("8/4/2016 8:10:15 AM").ToUniversalTime(),Description = "Great job!",Points = 20},
                //            new Recognition{Sender = jane,Receiver = john,Organization = org,Value = excellence,DateCreated = Convert.ToDateTime("8/7/2016 10:10:19 AM").ToUniversalTime(),Description = "fantastic!",Points = 30},
                //            new Recognition{Sender = mathew,Receiver = john,Organization = org,Value = honesty,DateCreated = Convert.ToDateTime("8/7/2016 10:10:19 AM").ToUniversalTime(),Description = "like your honesty. Keep it flowing",Points = 40},
                //            new Recognition{Sender = kate,Receiver = john,Organization = org,Value = vigilance,DateCreated = Convert.ToDateTime("8/7/2016 10:10:19 AM").ToUniversalTime(),Description = "very alert",Points = 60},
                //            new Recognition{Sender = nova,Receiver = john,Organization = org,Value = respect,DateCreated = Convert.ToDateTime("8/7/2016 10:10:19 AM").ToUniversalTime(),Description = "bow down!",Points = 50},
                //            new Recognition{Sender = john,Receiver = test,Organization = org,Value = integrity,DateCreated = Convert.ToDateTime("8/5/2016 11:8:9 AM").ToUniversalTime(),Description = "don't know what i would do without you",Points = 70},
                //            new Recognition{Sender = joe,Receiver = test,Organization = org,Value = commitment,DateCreated = Convert.ToDateTime("8/3/2016 9:0:9 AM").ToUniversalTime(),Description = "ipsum laurem",Points = 10},
                //            new Recognition{Sender = jane,Receiver = test,Organization = org,Value = courage,DateCreated = Convert.ToDateTime("8/12/2016 3:15:9 PM").ToUniversalTime(),Description = "masha alla",Points = 20},
                //            new Recognition{Sender = john,Receiver = joe,Organization = org,Value = excellence,DateCreated = Convert.ToDateTime("8/8/2016 2:17:19 PM").ToUniversalTime(),Description = "Super maan",Points = 30},
                //            new Recognition{Sender = jane,Receiver = joe,Organization = org,Value = integrity,DateCreated = Convert.ToDateTime("8/10/2016 1:12:59 PM").ToUniversalTime(),Description = "Thats great",Points = 70},
                //            new Recognition{Sender = test,Receiver = joe,Organization = org,Value = commitment,DateCreated = Convert.ToDateTime("8/15/2016 3:45:48 PM").ToUniversalTime(),Description = "aha",Points = 10},
                //            new Recognition{Sender = test,Receiver = jane,Organization = org,Value = courage,DateCreated = Convert.ToDateTime("8/20/2016 5:0:19 PM").ToUniversalTime(),Description = "Whaaaassssuuupppp",Points = 20},
                //            new Recognition{Sender = john,Receiver = jane,Organization = org,Value = excellence,DateCreated = Convert.ToDateTime("8/17/2016 3:10:19 PM").ToUniversalTime(),Description = "oh yeah",Points = 30},
                //            new Recognition{Sender = joe,Receiver = jane,Organization = org,Value = integrity,DateCreated = Convert.ToDateTime("8/18/2016 4:10:8 PM").ToUniversalTime(),Description = "oyee maaa",Points = 70}
                //        };
                //    _context.Recognitions.AddRange(recognitionLst);
                //    await _context.SaveChangesAsync();
                //}

                var test = await _userManager.FindByEmailAsync("test.user@hq.dhs.gov");
                var john = await _userManager.FindByEmailAsync("John.Doe@hq.dhs.gov");
                var joe = await _userManager.FindByEmailAsync("Joe.Blah@hq.dhs.gov");
                var jane = await _userManager.FindByEmailAsync("Jane.Doe@hq.dhs.gov");
                var mathew = await _userManager.FindByEmailAsync("Mathew.Anderson@hq.dhs.gov");
                var kate = await _userManager.FindByEmailAsync("Kate.Boxed@hq.dhs.gov");
                var nova = await _userManager.FindByEmailAsync("Nova.Down@hq.dhs.gov");

                var teamwork = _context.CorporateValues.First(x => x.Name == "Teamwork");
                var improvement = _context.CorporateValues.First(x => x.Name == "Improvement");
                var delivery = _context.CorporateValues.First(x => x.Name == "Delivery");
                var experiment = _context.CorporateValues.First(x => x.Name == "Experiment");

                var recognitionLst = new List<Recognition>
                        {
                            new Recognition{Sender = test,Receiver = john,Organization = org,Value = teamwork,DateCreated = Convert.ToDateTime("8/2/2016 4:0:9 PM").ToUniversalTime(),Description = "you are awesome",Points = 1},
                            new Recognition{Sender = joe,Receiver = john,Organization = org,Value = improvement,DateCreated = Convert.ToDateTime("8/4/2016 8:10:15 AM").ToUniversalTime(),Description = "Great job!",Points = 1},
                            new Recognition{Sender = jane,Receiver = john,Organization = org,Value = delivery,DateCreated = Convert.ToDateTime("8/7/2016 10:10:19 AM").ToUniversalTime(),Description = "fantastic!",Points = 1},
                            new Recognition{Sender = mathew,Receiver = john,Organization = org,Value = experiment,DateCreated = Convert.ToDateTime("8/7/2016 10:10:19 AM").ToUniversalTime(),Description = "like it love it",Points = 1},
                            new Recognition{Sender = kate,Receiver = john,Organization = org,Value = experiment,DateCreated = Convert.ToDateTime("8/7/2016 10:10:19 AM").ToUniversalTime(),Description = "very alert",Points = 1},
                            new Recognition{Sender = nova,Receiver = john,Organization = org,Value = experiment,DateCreated = Convert.ToDateTime("8/7/2016 10:10:19 AM").ToUniversalTime(),Description = "bow down!",Points = 1},
                            new Recognition{Sender = john,Receiver = test,Organization = org,Value = experiment,DateCreated = Convert.ToDateTime("8/5/2016 11:8:9 AM").ToUniversalTime(),Description = "don't know what i would do without you",Points = 1},
                            new Recognition{Sender = joe,Receiver = test,Organization = org,Value = teamwork,DateCreated = Convert.ToDateTime("8/3/2016 9:0:9 AM").ToUniversalTime(),Description = "ipsum laurem",Points = 1},
                            new Recognition{Sender = jane,Receiver = test,Organization = org,Value = improvement,DateCreated = Convert.ToDateTime("8/12/2016 3:15:9 PM").ToUniversalTime(),Description = "masha alla",Points = 1},
                            new Recognition{Sender = john,Receiver = joe,Organization = org,Value = delivery,DateCreated = Convert.ToDateTime("8/8/2016 2:17:19 PM").ToUniversalTime(),Description = "Super maaan",Points = 1},
                            new Recognition{Sender = jane,Receiver = joe,Organization = org,Value = experiment,DateCreated = Convert.ToDateTime("8/10/2016 1:12:59 PM").ToUniversalTime(),Description = "Thats great",Points = 1},
                            new Recognition{Sender = test,Receiver = joe,Organization = org,Value = teamwork,DateCreated = Convert.ToDateTime("8/15/2016 3:45:48 PM").ToUniversalTime(),Description = "aha",Points = 1},
                            new Recognition{Sender = test,Receiver = jane,Organization = org,Value = improvement,DateCreated = Convert.ToDateTime("8/20/2016 5:0:19 PM").ToUniversalTime(),Description = "Whaaaassssuuupppp",Points = 1},
                            new Recognition{Sender = john,Receiver = jane,Organization = org,Value = delivery,DateCreated = Convert.ToDateTime("8/17/2016 3:10:19 PM").ToUniversalTime(),Description = "oh yeah",Points = 1},
                            new Recognition{Sender = joe,Receiver = jane,Organization = org,Value = experiment,DateCreated = Convert.ToDateTime("8/18/2016 4:10:8 PM").ToUniversalTime(),Description = "oyi maaa",Points = 1}
                        };
                _context.Recognitions.AddRange(recognitionLst);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
    }
}