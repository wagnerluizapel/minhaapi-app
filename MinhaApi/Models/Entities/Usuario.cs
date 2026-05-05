namespace MinhaApi.Models.Entities;

public enum Role
{
    User,
    Admin
}

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
    public Role Role { get; set; }
}

