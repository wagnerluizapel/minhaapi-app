namespace MinhaApi.Models.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string Usuario { get; set; } = "anonymous";
    public string Metodo { get; set; }
    public string Endpoint { get; set; }
    public string Payload { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string RequestId { get; set; }
}
