using MinhaApi.Data;
using MinhaApi.Models.Entities;
using System.Text.Json;

public class AuditService
{
    private readonly AppDbContext _db;

    public AuditService(AppDbContext db)
    {
        _db = db;
    }

    public async Task RegistrarAsync(string usuario, string metodo, string endpoint, string payload, string requestId)
    {
        var log = new AuditLog
        {
            Usuario = usuario,
            Metodo = metodo,
            Endpoint = endpoint,
            Payload = payload,
            RequestId = requestId
        };

        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync();
    }

}
