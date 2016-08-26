#region references

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

#endregion

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
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_repository.GetAllUsers().ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get users: {0}", ex);
            }

            return BadRequest(new { Message = "Failed to get users" });
        }

        // GET api/users/cstrong@arielpartners.com
        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            try
            {
                var user = _repository.GetUserByEmail(email);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound(new { Message = $"User {email} not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get user: {0}", ex);
            }

            return BadRequest(new { Message = "Failed to get user" });
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
                        return NotFound(new { Message = $"Unable to find organization {newUser.OrganizationName}" });
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
                    return BadRequest(new { Message = $"Failed to add new user {newUser.Email}" });
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
                    var userToUpdate = _repository.GetUserByEmail(email);
                    if (userToUpdate == null)
                    {
                        return NotFound(new { Message = $"User {email} not found" });
                    }

                    if (userToUpdate.Organization.Name != updatedUserVM.OrganizationName)
                    {
                        // the organization changed, so we must retrieve it from the DB and set the new one
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

                        return Ok(new { Message = $"User {userToUpdate.Email} updated successfully" });
                    }
                    else
                    {
                        return Ok(new { Message = $"User {email} was not changed" });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to update user {email} : {0}", ex);
                    return BadRequest(new { Message = $"Failed to update user {email}" });
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
                    return NotFound(new { Message = $"User {email} not found" });
                }
                _repository.DeleteUser(user);
                _repository.SaveChangesAsync();

                return Ok(new { Message = $"User {email} record deleted" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete user: {0}", ex);
            }

            return BadRequest(new { Message = "Failed to delete user {email}" });
        }
    }
}
