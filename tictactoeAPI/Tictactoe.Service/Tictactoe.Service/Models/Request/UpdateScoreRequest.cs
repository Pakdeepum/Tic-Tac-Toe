namespace Tictactoe.Service.Models.Request
{
    public class UpdateScoreRequest
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
    }
}
