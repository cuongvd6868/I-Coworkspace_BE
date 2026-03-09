using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.AIChat;

namespace WebApi.Controllers
{
    [Route("api/ai-chat")]
    [ApiController]
    public class AIChatController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIChatController(IAIService aiService)
        {
            _aiService = aiService;
        }


        [HttpPost("ask")]
        public async Task<IActionResult> AskAI([FromBody] AIChatRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new { message = "Vui lòng nhập nội dung câu hỏi." });
            }

            try
            {
                var answer = await _aiService.GetAIResponseAsync(request.Prompt);

                return Ok(new AIChatResponseDto
                {
                    Answer = answer,
                    SentAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi xử lý yêu cầu của bạn.",
                    detail = ex.Message
                });
            }
        }
    }

}