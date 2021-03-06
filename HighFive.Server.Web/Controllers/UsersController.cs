﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using HighFive.Server.Services.Utils;

namespace HighFive.Server.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<UsersController> _logger;

        #region Constructor

        public UsersController(IHighFiveRepository repository, ILogger<UsersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #endregion

        #region GetAll - GET api/users

        // GET api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var users = _repository.GetAllUsers().ToList();
                if (users.Any()) return Ok(Mapper.Map<List<UserViewModel>>(users));
                return NoContent();
            }
            catch (HighFiveException ex)
            {
                _logger.LogError("Failed to get users: {0}", ex);
            }
            return BadRequest(new { Message = "Failed to get users" });
        }

        #endregion

        #region GetByEmail- GET api/users/cstrong@arielpartners.com

        // GET api/users/cstrong@arielpartners.com
        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            try
            {
                var user = _repository.GetUserByEmail(email);
                if (user != null) return Ok(Mapper.Map<UserViewModel>(user));
                return NotFound(new { Message = $"User {email} not found" });
            }
            catch (HighFiveException ex)
            {
                _logger.LogError("Failed to get user: {0}", ex);
            }
            return BadRequest(new { Message = "Failed to get user" });
        }

        #endregion

        #region Post - POST api/users

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]UserViewModel newUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // create user in repo
            try
            {
                var theNewUser = Mapper.Map<HighFiveUser>(newUser);
                var organization = _repository.GetOrganizationByName(newUser.OrganizationName);
                if (organization == null) return NotFound(new { Message = $"Unable to find organization {newUser.OrganizationName}" });
                theNewUser.Organization = organization;

                _repository.AddUser(theNewUser);
                _repository.SaveChangesAsync();
            }
            catch (HighFiveException ex)
            {
                _logger.LogError("Failed to add new user: {0}", ex);
                return BadRequest(new { Message = $"Failed to add new user {newUser.Email}" });
            }

            return Created($"api/users/{newUser.Email}", newUser);
        }

        #endregion

        #region Put - PUT api/users/cstrong@arielpartners.com

        // PUT api/users/cstrong@arielpartners.com
        [HttpPut("{email}")]
        public IActionResult Put(string email, [FromBody]UserViewModel updatedUserViewModel)
        {
            var changed = false;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var userToUpdate = _repository.GetUserByEmail(email);
                if (userToUpdate == null) return NotFound(new { Message = $"User {email} not found" });

                if (userToUpdate.Organization.Name != updatedUserViewModel.OrganizationName)
                {
                    // the organization changed, so we must retrieve it from the DB and set the new one
                    var organization = _repository.GetOrganizationByName(updatedUserViewModel.OrganizationName);
                    if (organization == null)
                    {
                        return NotFound(new { Message = $"Organization {updatedUserViewModel.OrganizationName} not found" });
                    }
                    changed = true;
                    userToUpdate.Organization = organization;
                }

                // see if the email has changed. if not, return NoChange()
                // if so, change the email and save the object in the context
                if (userToUpdate.Email != updatedUserViewModel.Email)
                {
                    userToUpdate.Email = updatedUserViewModel.Email;
                    changed = true;
                }
                if (!changed) return Ok(new { Message = $"User {email} was not changed" });
                _repository.UpdateUser(userToUpdate);
                _repository.SaveChangesAsync();

                return Ok(new { Message = $"User {userToUpdate.Email} updated successfully" });
            }
            catch (HighFiveException ex)
            {
                _logger.LogError($"Failed to update user {email} : {0}", ex);
                return BadRequest(new { Message = $"Failed to update user {email}" });
            }
        }

        // DELETE api/users/cstrong@arielpartners.com
        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            try
            {
                var user = _repository.GetUserByEmail(email);
                if (user == null) return NotFound(new { Message = $"User {email} not found" });
                _repository.DeleteUser(user);
                _repository.SaveChangesAsync();
                return Ok(new { Message = $"User {email} record deleted" });
            }
            catch (HighFiveException ex)
            {
                _logger.LogError("Failed to delete user: {0}", ex);
            }
            return BadRequest(new { Message = $"Failed to delete user {email}" });
        }
        #endregion
    }
}
