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

        /// <summary>
        /// Gửi câu hỏi cho trợ lý ảo tư vấn Workspace
        /// </summary>
        /// <param name="request">Chứa nội dung câu hỏi (prompt)</param>
        [HttpPost("ask")]
        public async Task<IActionResult> AskAI([FromBody] AIChatRequestDto request)
        {
            // Kiểm tra đầu vào
            if (request == null || string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new { message = "Vui lòng nhập nội dung câu hỏi." });
            }

            try
            {
                // Gọi dịch vụ AI (Service này sẽ tự truy xuất DB và gọi Gemini)
                var answer = await _aiService.GetAIResponseAsync(request.Prompt);

                // Trả về kết quả cho Frontend
                return Ok(new AIChatResponseDto
                {
                    Answer = answer,
                    SentAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần và trả về thông báo lỗi thân thiện
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi xử lý yêu cầu của bạn.",
                    detail = ex.Message
                });
            }
        }
    }

}