using Application.DTOs.Review;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService) => _reviewService = reviewService;

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "USER-TEST-ID";

        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewRequest request)
        {
            try
            {
                var result = await _reviewService.CreateReviewAsync(CurrentUserId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}