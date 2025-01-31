using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace ECPC.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Dummy implementation for development purposes
            return Task.CompletedTask;
        }
    }
}
