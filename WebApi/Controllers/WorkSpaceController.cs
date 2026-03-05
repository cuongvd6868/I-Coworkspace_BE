using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/workspace")]
    [ApiController]
    public class WorkSpaceController : ControllerBase
    {
        private readonly IWorkSpaceService _workSpaceService;
        private readonly ILogger<WorkSpaceController> _logger;

        public WorkSpaceController(IWorkSpaceService workSpaceService, ILogger<WorkSpaceController> logger)
        {
            _workSpaceService = workSpaceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkSpace()
        {
            try
            {
                var workSpaces = await _workSpaceService.GetAllAsync();
                return Ok(workSpaces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching workspaces");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkSpaceById(int workspaceId)
        {
            try
            {
                var workSpace = await _workSpaceService.GetWorkSpaceDetailsAsync(workspaceId);
                return Ok(workSpace);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching workspaces");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
