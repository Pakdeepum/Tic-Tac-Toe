using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Tictactoe.Service.Models.Base;

namespace Tictactoe.Service.DAL
{
    public class BaseDAL
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly string dbConnectionString;
        //protected readonly string dbConnectionStringeBaoLSStaging;


        protected SqlConnection? _dbConnection;
        public BaseDAL(IHttpContextAccessor httpContextAccessor, IOptions<ConnectionStringSettings> ConnectionStrings)
        {
            _httpContextAccessor = httpContextAccessor;
            dbConnectionString = ConnectionStrings.Value.Default ?? "";
        }
    }
}
