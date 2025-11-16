using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tictactoe.Service.Models.Entities
{
    [Table("USER_SCORE_LOG")]
    public class UserScoreLog
    {
        public UserScoreLog()
        {
            this.Id = Guid.NewGuid();
            this.LastScore = 0;
            this.CurrentScore = 0;
            this.CreateDate = DateTime.Now;
        }

        [Column("ID")]
        public Guid Id { get; set; }

        [Column("USER_ID")]
        public Guid UserId { get; set; }

        [Column("WINNING_STATUS")]
        public string WinningStatus { get; set; } = string.Empty;

        [Column("LAST_SCORE")]
        public int LastScore { get; set; }

        [Column("CURRENT_SCORE")]
        public int CurrentScore { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
    }
}
