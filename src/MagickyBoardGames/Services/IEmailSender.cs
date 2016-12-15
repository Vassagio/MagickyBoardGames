using System.Threading.Tasks;

namespace MagickyBoardGames.Services {
    public interface IEmailSender {
        Task SendEmailAsync(string email, string subject, string message);
    }
}