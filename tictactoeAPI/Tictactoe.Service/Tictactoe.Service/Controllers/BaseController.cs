using Microsoft.AspNetCore.Mvc;

namespace Tictactoe.Service.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
        {

            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
    }
}
