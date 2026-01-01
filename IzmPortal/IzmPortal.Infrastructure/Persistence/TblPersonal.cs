using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzmPortal.Infrastructure.Persistence;

[Table("Tbl_Personal")]
public class TblPersonal
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    public string Username { get; set; } = null!;

    [Column("card")]
    public string? Card { get; set; }

    [Column("departman")]
    public string? Departman { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("TcNumber")]
    public string TcNumber { get; set; } = null!;
}


