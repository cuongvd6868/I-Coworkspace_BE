using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Extensions;
using Application.DTOs.HostProfile;
using Application.Mappings; 
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/host-profile")]
    [ApiController]
    [Authorize] 
    public class HostProfileController : ControllerBase
    {
        private readonly IHostProfileService _hostProfileService;
        private readonly ILogger<HostProfileController> _logger;

        public HostProfileController(IHostProfileService hostProfileService, ILogger<HostProfileController> logger)
        {
            _hostProfileService = hostProfileService;
            _logger = logger;
        }

        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userId = User.GetUserId();
                var profile = await _hostProfileService.GetHostProfileByUserIdAsync(userId);

                if (profile == null) return NotFound("Host profile not found.");

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting profile");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetProfileById(int id)
        {
            var profile = await _hostProfileService.GetHostProfileByIdAsync(id);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateHostProfileRequest request)
        {
            try
            {
                var userId = User.GetUserId();

                var hostProfile = request.ToHostProfileCreateDTO();

                await _hostProfileService.CreateHostProfileAsync(hostProfile, userId);

                return Ok(new { Message = "Host profile created successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating host profile");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateHostProfileRequest request)
        {
            try
            {
                var userId = User.GetUserId();

                var hostProfile = request.ToHostProfileUpdateDTO();

                await _hostProfileService.UpdateHostProfileAsync(hostProfile, userId);

                return Ok(new { Message = "Host profile updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating host profile");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}