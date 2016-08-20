using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using highfive_server.Models;

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
        public IActionResult Post([FromBody]string value)
        {
            // get the organization name from JSON
            // lookup the organization by name in the DB
            // create the user with the organization in DB
            throw new NotImplementedException();
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
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete user: {0}", ex);
            }

            return BadRequest("Failed to delete user");
        }
    }
}
