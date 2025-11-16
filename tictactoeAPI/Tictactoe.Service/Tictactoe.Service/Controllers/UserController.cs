using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tictactoe.Service.DAL.Interface;
using Tictactoe.Service.Models.Entities;
using Tictactoe.Service.Models.Request;

namespace Tictactoe.Service.Controllers
{
    [ApiController]
    [Route("UserController")]
    public class UserController : BaseController
    {
        private readonly IUserDAL _userDAL;
        private readonly IConfiguration _configuration;

        public UserController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserController> logger,
            IUserDAL userDAL,
            IConfiguration configuration
        ) : base(httpContextAccessor, logger)
        {
            _userDAL = userDAL;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUser")]
        public async Task<List<MUser>> GetUser(string email = "")
        {
            try
            {
                var result = await _userDAL.GetUser(email: email);

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetLeaderBoard")]
        public async Task<List<MUser>> GetLeaderBoard()
        {
            try
            {
                var result = (await _userDAL.GetUser()).OrderByDescending(t => t.Score).OrderBy(t => t.Email);

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("CreateUser")]
        public async Task<int> CreateUser([FromBody] UserRequest request)
        {
            try
            {
                var result = await _userDAL.CreateUser(request.Email);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
