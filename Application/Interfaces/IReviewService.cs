using Application.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(string userId, CreateReviewRequest request);
    }
}
