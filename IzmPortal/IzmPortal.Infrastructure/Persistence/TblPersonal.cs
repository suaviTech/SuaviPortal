namespace IzmPortal.Infrastructure.Persistence;

public class TblPersonal
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Card { get; set; } = null!;
    public string Departman { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string TcNumber { get; set; } = null!;
}

