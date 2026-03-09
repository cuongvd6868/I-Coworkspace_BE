using Application.DTOs.WorkSpace;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkSpaceController : ControllerBase
    {
        private readonly IWorkSpaceService _workSpaceService;

        public WorkSpaceController(IWorkSpaceService workSpaceService)
        {
            _workSpaceService = workSpaceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkSpaceResponseDto>>> GetAll()
        {
            var response = await _workSpaceService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkSpaceResponseDto>> GetById(int id)
        {
            var response = await _workSpaceService.GetByIdAsync(id);
            if (response == null) return NotFound(new { Message = $"Không tìm thấy Workspace với ID {id}" });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<WorkSpaceResponseDto>> Create([FromBody] WorkSpaceCreateRequest request)
        {
            try
            {
                var response = await _workSpaceService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorkSpaceUpdateRequest request)
        {
            if (id != request.Id) return BadRequest(new { Message = "ID không khớp" });

            var updated = await _workSpaceService.UpdateAsync(request);
            if (!updated) return NotFound(new { Message = "Cập nhật thất bại, không tìm thấy Workspace" });

            return Ok(new { Message = "Cập nhật Workspace thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _workSpaceService.DeleteAsync(id);
                if (!deleted) return NotFound();
                return Ok(new { Message = "Xóa Workspace thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("search/location")]
        public async Task<ActionResult<IEnumerable<WorkSpaceResponseDto>>> GetByLocation([FromQuery] string location)
        {
            var response = await _workSpaceService.GetByLocationAsync(location);
            return Ok(response);
        }

        [HttpGet("search/type/{typeId}")]
        public async Task<ActionResult<IEnumerable<WorkSpaceResponseDto>>> GetByType(int typeId)
        {
            var response = await _workSpaceService.GetByTypeAsync(typeId);
            return Ok(response);
        }
    }
}