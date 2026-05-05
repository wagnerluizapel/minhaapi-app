using MinhaApi.Data;
using MinhaApi.Models.Entities;
using Microsoft.Extensions.Logging;

namespace MinhaApi.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PessoaRepository> _logger;

    public PessoaRepository(AppDbContext context, ILogger<PessoaRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public List<Pessoa> GetAll() =>
        _context.Pessoas.ToList();

    public Pessoa? GetByName(string nome) =>
        _context.Pessoas
            .FirstOrDefault(p => p.Nome.ToLower() == nome.ToLower());

    public void Add(Pessoa pessoa)
    {
        _logger.LogInformation("Salvando pessoa no banco: {@Pessoa}", pessoa);

        _context.Pessoas.Add(pessoa);
        _context.SaveChanges();

        _logger.LogInformation("Pessoa salva com sucesso no banco");
    }

    public void Update(Pessoa pessoa)
    {
        _context.Pessoas.Update(pessoa);
        _context.SaveChanges();
    }

    public void Delete(Pessoa pessoa)
    {
        _context.Pessoas.Remove(pessoa);
        _context.SaveChanges();
    }
}
