using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HighFive.Server.Services.Utils;
using Microsoft.AspNetCore.Authorization;

namespace HighFive.Server.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class OrganizationsController : Controller 
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<OrganizationsController> _logger;

        public OrganizationsController(IHighFiveRepository repository, ILogger<OrganizationsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            try
            {
                var organizations = _repository.GetAllOrganizations().ToList();
                if (organizations.Count > 0) return Ok(Mapper.Map<List<OrganizationViewModel>>(organizations));
                return NoContent();
            }
            catch (HighFiveException ex)
            {
                _logger.LogError("Failed to get Organizations: {0}", ex);
            }
            return BadRequest(new { Message = "Failed to get Organizations" });
        }

        [HttpGet("{organizationname}")]
        public IActionResult GetOrganizationByName(string organizationName)
        {
            try
            {
                var organization = _repository.GetOrganizationByName(organizationName);
                if (organization == null) return NotFound(new { Message = $"Unable to find organization {organizationName}" });
                return Ok(Mapper.Map<OrganizationViewModel>(organization));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get Organization: {0}", ex);
            }
            return BadRequest(new { Message = $"Failed to get Organization {organizationName}" });
        }

        // POST api/organizations
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrganizationViewModel newOrganization)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // create organization in repo.
            try
            {
                var theNewOrganization = Mapper.Map<Organization>(newOrganization);
                _repository.AddOrganization(theNewOrganization);
                if (await _repository.SaveChangesAsync()) return Created($"api/organization/{newOrganization.Name}", newOrganization);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add new organization: {0}", ex);
                if (ex.GetType().Name.Equals("HighFiveException")) return BadRequest(new { ex.Message });
            }
            return BadRequest(new { Message = $"Failed to add new organization {newOrganization.Name}" });
        }

        [HttpPut("{organizationname}")]
        public async Task<IActionResult> Put(string organizationname, [FromBody]OrganizationViewModel updatedOrganizationViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var organizationToUpdate = _repository.GetOrganizationByName(organizationname);
                if (organizationToUpdate == null) return NotFound(new { Message = $"Unable to find organization {organizationname}" });

                if (organizationToUpdate.Name == updatedOrganizationViewModel.Name &&
                    organizationToUpdate.WebPath == updatedOrganizationViewModel.WebPath)
                    return Ok(new { Message = $"Organization {organizationname} was not changed" });

                //Update the name & WebPath values
                organizationToUpdate.Name = organizationname;
                organizationToUpdate.WebPath = updatedOrganizationViewModel.WebPath;
                foreach (var cv in updatedOrganizationViewModel.Values)
                {
                    //What needs to be done with Values? - Do we check each one and 
                    //update if found 
                    //add if not found 
                    //or replace all of them?
                    var corpval = _repository.GetCorporateValueByNameAndDescription(cv.Name, cv.Description);
                    if (corpval == null) organizationToUpdate.Values.Add(cv);
                }

                _repository.UpdateOrganization(organizationToUpdate);
                if (await _repository.SaveChangesAsync()) return Ok(new { Message = $"Organization {organizationname} updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update organization: {0}", ex);
                if (ex.GetType().Name.Equals("HighFiveException")) return BadRequest(new { ex.Message });
            }
            return BadRequest(new { Message = $"Failed to update organization {organizationname}" });
        }

        [HttpDelete("{organizationname}")]
        public async Task<IActionResult> Delete(string organizationName)
        {
            try
            {
                var organization = _repository.GetOrganizationByName(organizationName);
                if (organization == null) return NotFound(new { Message = $"Unable to find organization {organizationName}" });
                _repository.DeleteOrganization(organization);
                if (await _repository.SaveChangesAsync()) return Ok(new { Message = $"Organization {organizationName} record deleted" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete organization: {0}", ex);
            }
            return BadRequest(new { Message = $"Failed to delete organization {organizationName}" });
        }
    }
}
