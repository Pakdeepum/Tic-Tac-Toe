using System.ComponentModel.DataAnnotations.Schema;

namespace Tictactoe.Service.Models.Entities
{
    [Table("M_USER")]
    public class MUser
    {
        public MUser()
        {
            this.Id = Guid.NewGuid();
            this.Score = 0;
            this.CreateDate = DateTime.Now;
        }

        [Column("ID")]
        public Guid Id { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        [Column("SCORE")]
        public int? Score { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
    }
}
