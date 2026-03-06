using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public partial interface IAIService
    {
        Task<string> GetAIResponseAsync(string userPrompt);
    }
}