using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using highfive_server.Models;
using highfive_server.ViewModels;

namespace highfive_server.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IHighFiveRepository _repository;
        private ILogger<UsersController> _logger;

        public UsersController(IHighFiveRepository repository, ILogger<UsersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET api/users
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.GetAllUsers().ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get users: {0}", ex);
            }

            return BadRequest("Failed to get users");
        }

        // GET api/users/cstrong@arielpartners.com
        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            try
            {
                return Ok(_repository.GetUserByEmail(email));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get user: {0}", ex);
            }

            return BadRequest("Failed to get user");
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]UserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                // create user in repo
                try
                {
                    var theNewUser = Mapper.Map<HighFiveUser>(newUser);
                    var organization = _repository.GetOrganizationByName(newUser.OrganizationName);
                    if(organization == null)
                    {
                        return NotFound("Unable to find organization: " + organization);
                    }
                    else
                    {
                        theNewUser.Organization = organization;
                    }

                    _repository.AddUser(theNewUser);
                    _repository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to add new user: {0}", ex);
                    return BadRequest("Failed to add new user");
                }

                return Created($"api/users/{newUser.Email}", newUser);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/users/cstrong@arielpartners.com
        [HttpPut("{email}")]
        public IActionResult Put(string email, [FromBody]string value)
        {
            // get the user by email
            // update the user with the passed in parameters
            throw new NotImplementedException();
        }

        // DELETE api/users/cstrong@arielpartners.com
        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            try
            {
                var user = _repository.GetUserByEmail(email);
                if (user == null)
                {
                    return NotFound();
                }
                _repository.DeleteUser(user);
                _repository.SaveChangesAsync();

                HashSet<string> hashSet = new HashSet<string>();
                hashSet.Add("User: " + user.Email + "Record deleted.");

                return Ok(hashSet);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete user: {0}", ex);
            }

            return BadRequest("Failed to delete user");
        }
    }
}
