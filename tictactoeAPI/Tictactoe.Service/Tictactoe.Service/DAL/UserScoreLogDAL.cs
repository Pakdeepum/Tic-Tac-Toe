using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tictactoe.Service.DAL.Interface;
using Tictactoe.Service.Models.Base;
using Tictactoe.Service.Models.Entities;

namespace Tictactoe.Service.DAL
{
    public class UserScoreLogDAL : BaseDAL, IUserScoreLogDAL
    {
        private IConfiguration Configuration;
        private readonly AppDbContext _context;


        public UserScoreLogDAL(IHttpContextAccessor httpContextAccessor, IOptions<ConnectionStringSettings> connectionStringSettings, AppDbContext context)
       : base(httpContextAccessor, connectionStringSettings)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserScoreLog>> GetLogUser(Guid userId)
        {
            var query = _context.UserScoreLog.Where(t => t.UserId == userId).AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<int> InsertLog(Guid userId, int lastScore, int currentScore, string winningStatus)
        {
            var new_log = new UserScoreLog();
            new_log.UserId = userId;
            new_log.WinningStatus = winningStatus;
            new_log.LastScore = lastScore;
            new_log.CurrentScore = currentScore;

            _context.UserScoreLog.Add(new_log);

            return await _context.SaveChangesAsync();
        }
    }
}
