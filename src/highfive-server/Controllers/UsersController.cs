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
        public string Get(string email)
        {
            return "value";
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/users/cstrong@arielpartners.com
        [HttpPut("{email}")]
        public void Put(string email, [FromBody]string value)
        {
        }

        // DELETE api/users/cstrong@arielpartners.com
        [HttpDelete("{email}")]
        public void Delete(string email)
        {
        }
    }
}
