using System.Threading.Tasks;

namespace MagickyBoardGames.Services {
    public interface ISmsSender {
        Task SendSmsAsync(string number, string message);
    }
}