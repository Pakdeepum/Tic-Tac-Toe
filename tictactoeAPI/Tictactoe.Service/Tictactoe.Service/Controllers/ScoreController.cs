using Azure.Core;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tictactoe.Service.DAL.Interface;
using Tictactoe.Service.Models.Entities;
using Tictactoe.Service.Models.Request;

namespace Tictactoe.Service.Controllers
{
    [ApiController]
    [Route("ScoreController")]
    public class ScoreController : BaseController
    {
        private readonly IUserDAL _userDAL;
        private readonly IUserScoreLogDAL _userScoreLogDAL;
        private readonly IConfiguration _configuration;

        public ScoreController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserController> logger,
            IUserDAL userDAL,
            IUserScoreLogDAL userScoreLogDAL,
            IConfiguration configuration
        ) : base(httpContextAccessor, logger)
        {
            _userDAL = userDAL;
            _userScoreLogDAL = userScoreLogDAL;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        [Route("CheckWinCount")]
        public async Task<int> CheckWinCount(Guid userId)
        {
            try
            {
                var last2Log = (await _userScoreLogDAL.GetLogUser(userId)).OrderByDescending(x => x.CreateDate).Take(2);

                var winCount = 0;
                foreach (var item in last2Log)
                {
                    if (item.WinningStatus == "Winner")
                    {
                        winCount += 1;
                    }
                }

                return winCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateScore")]
        public async Task<int> UpdateScore([FromBody] UpdateScoreRequest request)
        {
            try
            {
                var user = (await _userDAL.GetUser(id: request.Id)).FirstOrDefault();

                if (user != null)
                {
                    var sumScore = (user.Score ?? 0) + request.Score;

                    var result = await _userDAL.UpdateScore(request.Id, sumScore);

                    await _userScoreLogDAL.InsertLog(request.Id, user.Score ?? 0, sumScore, request.Score == 1 ? "Winner" : "Loser");

                    return result;
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
