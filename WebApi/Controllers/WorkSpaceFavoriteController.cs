using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization; 

namespace WebApi.Controllers
{
    [Route("api/favorite")]
    [ApiController]
    [Authorize] 
    public class WorkSpaceFavoriteController : ControllerBase
    {
        private readonly IWorkSpaceFavoriteService _favoriteService;
        private readonly ILogger<WorkSpaceFavoriteController> _logger;

        public WorkSpaceFavoriteController(IWorkSpaceFavoriteService favoriteService, ILogger<WorkSpaceFavoriteController> logger)
        {
            _favoriteService = favoriteService;
            _logger = logger;
        }

        [HttpPost("toggle/{workSpaceId}")] 
        public async Task<IActionResult> ToggleFavorite(int workSpaceId)
        {
            try
            {
                var uid = User.GetUserId();

                if (string.IsNullOrEmpty(uid))
                    return Unauthorized("User ID not found in token.");

                await _favoriteService.ToggleFavoriteAsync(workSpaceId, uid);

                return Ok(new { Message = "Successfully updated favorite status." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling favorite for WorkSpace {Id}", workSpaceId);
                return StatusCode(500, "Something went wrong on our end.");
            }
        }
    }
}