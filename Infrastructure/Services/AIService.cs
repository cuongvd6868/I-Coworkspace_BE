using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Infrastructure.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IWorkSpaceRepository _workspaceRepo;

        public AIService(HttpClient httpClient, IConfiguration configuration, IWorkSpaceRepository workspaceRepo)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"] ?? "";
            _workspaceRepo = workspaceRepo;
        }

        public async Task<string> GetAIResponseAsync(string userPrompt)
        {
            // 1. Lấy dữ liệu Workspace kèm theo Room và Amenity
            var workspaces = await _workspaceRepo.GetAllWorkSpacesAsync();

            // 2. Xây dựng chuỗi dữ liệu chi tiết cho AI
            var sb = new StringBuilder();
            sb.AppendLine("Dưới đây là danh sách các Workspace hiện có:");

            foreach (var w in workspaces)
            {
                sb.AppendLine($"--- Workspace: {w.Title} ---");
                sb.AppendLine($"- Địa chỉ: {w.Address?.Street}, {w.Address?.Ward}, {w.Address?.State}");
                sb.AppendLine("- Các phòng trống:");

                foreach (var room in w.WorkSpaceRooms.Where(r => r.IsActive))
                {
                    // Lấy danh sách tiện ích của từng phòng
                    var amenities = string.Join(", ", room.WorkSpaceRoomAmenities
                        .Select(a => a.Amenity?.Name));

                    sb.AppendLine($"  + Phòng: {room.Title}");
                    sb.AppendLine($"    * Giá: {room.PricePerHour:N0}đ/giờ, {room.PricePerDay:N0}đ/ngày");
                    sb.AppendLine($"    * Sức chứa: {room.Capacity} người - Diện tích: {room.Area}m2");
                    sb.AppendLine($"    * Tiện ích: {amenities}");
                }
            }

            string systemInstruction = $@"
                Bạn là trợ lý ảo chuyên nghiệp của hệ thống đặt văn phòng CoSpace.
                Sử dụng dữ liệu thực tế sau đây để tư vấn:
                {sb}

                Quy tắc:
                1. Nếu khách hỏi về giá, hãy tìm phòng có 'PricePerHour' hoặc 'PricePerDay' thấp nhất.
                2. Nếu khách hỏi về số lượng người (Capacity), hãy gợi ý phòng có sức chứa phù hợp.
                3. Nếu khách hỏi tiện ích (như Wifi, máy chiếu), hãy kiểm tra phần 'Tiện ích' của từng phòng.
                4. Trả lời ngắn gọn, lịch sự, có cấu trúc rõ ràng.";

            var url = $"https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key={_apiKey}";
            var requestBody = new
            {
                contents = new[] {
                    new { parts = new[] { new { text = $"{systemInstruction}\n\nNgười dùng: {userPrompt}" } } }
                }
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, requestBody);
                if (!response.IsSuccessStatusCode)
                {
                    var errorDetail = await response.Content.ReadAsStringAsync();
                    return $"Lỗi Google API: {response.StatusCode} - Chi tiết: {errorDetail}";
                }

                var result = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                return result?.Candidates?[0].Content?.Parts?[0].Text ?? "Tôi chưa tìm thấy lựa chọn nào phù hợp với yêu cầu của bạn.";
            }
            catch
            {
                return "Kết nối tới hệ thống AI bị gián đoạn.";
            }
        }
    }

    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<Candidate>? Candidates { get; set; }
    }

    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content? Content { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part>? Parts { get; set; }
    }

    public class Part
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}