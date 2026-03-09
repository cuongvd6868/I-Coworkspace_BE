using Application.DTOs.WorkSpaceRoom;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkSpaceRoomsController : ControllerBase
    {
        private readonly IWorkSpaceRoomService _roomService;

        public WorkSpaceRoomsController(IWorkSpaceRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roomService.GetByIdAsync(id);
            if (result == null) return NotFound(new { Message = "Không tìm thấy phòng." });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkSpaceRoomRequest request)
        {
            try
            {
                var result = await _roomService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateWorkSpaceRoomRequest request)
        {
            var success = await _roomService.UpdateAsync(request);
            if (!success) return NotFound(new { Message = "Cập nhật thất bại." });
            return Ok(new { Message = "Cập nhật phòng thành công." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _roomService.DeleteAsync(id);
                if (!success) return NotFound();
                return Ok(new { Message = "Đã xóa phòng thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}