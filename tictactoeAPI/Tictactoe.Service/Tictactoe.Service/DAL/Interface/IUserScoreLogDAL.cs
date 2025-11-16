using Tictactoe.Service.Models.Entities;

namespace Tictactoe.Service.DAL.Interface
{
    public interface IUserScoreLogDAL
    {
        Task<IEnumerable<UserScoreLog>> GetLogUser(Guid userId);
        Task<int> InsertLog(Guid userId, int lastScore, int currentScore, string winningStatus);
    }
}
