using MinhaApi.Models.Entities;

namespace MinhaApi.Repositories;

public interface IPessoaRepository
{
    List<Pessoa> GetAll();
    Pessoa? GetByName(string nome);
    void Add(Pessoa pessoa);
    void Update(Pessoa pessoa);
    void Delete(Pessoa pessoa);
}
