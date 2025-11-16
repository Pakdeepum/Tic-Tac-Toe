using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tictactoe.Service.DAL.Interface;
using Tictactoe.Service.Models.Base;
using Tictactoe.Service.Models.Entities;

namespace Tictactoe.Service.DAL
{
    public class UserDAL : BaseDAL, IUserDAL
    {
        private IConfiguration Configuration;
        private readonly AppDbContext _context;


        public UserDAL(IHttpContextAccessor httpContextAccessor, IOptions<ConnectionStringSettings> connectionStringSettings, AppDbContext context)
       : base(httpContextAccessor, connectionStringSettings)
        {
            _context = context;
        }

        public async Task<IEnumerable<MUser>> GetUser(Guid? id = null, string email = "")
        {
            var query = _context.MUser.AsQueryable();

            if(id != null)
            {
                query = query.Where(t => t.Id == id);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(t => t.Email == email);
            }

            return await query.ToListAsync();
        }

        public async Task<int> CreateUser(string email)
        {
            var new_user = new MUser();
            new_user.Email = email;

            _context.MUser.Add(new_user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateScore(Guid id, int summaryScore)
        {
            var user = _context.MUser.Where(t => t.Id == id).FirstOrDefault();

            if (user != null)
            {
                user.Score = summaryScore;
                _context.MUser.Update(user);
            }

            return await _context.SaveChangesAsync();
        }
    }
}
