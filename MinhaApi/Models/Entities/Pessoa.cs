namespace MinhaApi.Models.Entities;

public class Pessoa
{
    public int Id { get; set; } // chave primária obrigatória
    public string Nome { get; set; }
    public int? Idade { get; set; }
}
