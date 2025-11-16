using Tictactoe.Service.Models.Entities;

namespace Tictactoe.Service.DAL.Interface
{
    public interface IUserDAL
    {
        Task<IEnumerable<MUser>> GetUser(Guid? id = null, string email = "");
        Task<int> CreateUser(string email);
        Task<int> UpdateScore(Guid id, int summaryScore);
    }
}
