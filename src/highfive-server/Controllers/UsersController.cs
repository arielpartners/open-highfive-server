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
                    if (organization == null)
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
        public IActionResult Put(string email, [FromBody]UserViewModel updatedUserVM)
        {
            bool changed = false;

            if (ModelState.IsValid)
            {
                try
                {
                    // retrieve the existing user by the email
                    var userToUpdate = _repository.GetUserByEmail(email);
                    if (userToUpdate == null)
                    {
                        return NotFound(new { Message = $"User {email} not found" });
                    }

                    if (userToUpdate.Organization.Name != updatedUserVM.OrganizationName)
                    {
                        // the orgnization changed, so we must retrieve it from the DB and set the new one
                        var organization = _repository.GetOrganizationByName(updatedUserVM.OrganizationName);
                        if (organization == null)
                        {
                            return NotFound(new { Message = $"Organization {updatedUserVM.OrganizationName} not found" });
                        }
                        changed = true;
                        userToUpdate.Organization = organization;
                    }

                    // see if the email has changed. if not, return NoChange()
                    // if so, change the email and save the object in the context
                    if (userToUpdate.Email != updatedUserVM.Email)
                    {
                        userToUpdate.Email = updatedUserVM.Email;
                        changed = true;
                    }
                    if (changed)
                    {
                        _repository.UpdateUser(userToUpdate);
                        _repository.SaveChangesAsync();

                        return Ok(new { Message = $"User {email} updated successfully" });
                    }
                    else
                    {
                        return Ok(new { Message = $"User {email} has not changed" });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to update user: {0}", ex);
                    return BadRequest("Failed to update new user");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
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

                return Ok(new { Message = $"User {user.Email} record deleted" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete user: {0}", ex);
            }

            return BadRequest("Failed to delete user");
        }
    }
}
