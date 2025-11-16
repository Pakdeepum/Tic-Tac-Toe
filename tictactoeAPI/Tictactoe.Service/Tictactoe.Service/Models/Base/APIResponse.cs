namespace Tictactoe.Service.Models.Base
{
    public class APIResponse
    {
        public APIResponse() 
        { 
            this.success = false;
        }

        public APIResponse(bool success, object? data, int status)
        {
            this.success = success;
            this.data = data;
            this.status = status;
        }

        public bool success { get; set; }
        public object? data { get; set; }
        public int status { get; set; }
    }
}
