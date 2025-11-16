namespace Tictactoe.Service.Models.Request
{
    public class UserRequest
    {
        public Guid? Id { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
