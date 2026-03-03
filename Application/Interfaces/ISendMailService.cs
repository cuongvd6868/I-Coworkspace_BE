using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ISendMailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
