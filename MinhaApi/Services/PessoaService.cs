using MinhaApi.Models.Entities;
using MinhaApi.Models.Dtos;
using MinhaApi.Repositories;
using Microsoft.Extensions.Logging;

namespace MinhaApi.Services;

public class PessoaService
{
    private readonly IPessoaRepository _repository;
    private readonly ILogger<PessoaService> _logger;

    public PessoaService(IPessoaRepository repository, ILogger<PessoaService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public ApiResponse<List<PessoaResponseDto>> Listar()
    {
        _logger.LogInformation("Listando todas as pessoas...");

        var pessoas = _repository.GetAll()
            .Select(p => new PessoaResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade
            })
            .ToList();

        return ApiResponse<List<PessoaResponseDto>>.Ok(pessoas);
    }

    public ApiResponse<PessoaResponseDto> Criar(PessoaCreateDto dto)
    {
        _logger.LogInformation("Criando pessoa: {Nome}", dto.Nome);

        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };

        _repository.Add(pessoa);

        _logger.LogInformation("Pessoa criada com ID={Id}", pessoa.Id);

        return ApiResponse<PessoaResponseDto>.Ok(new PessoaResponseDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade
        });
    }

    public ApiResponse<PessoaResponseDto> Buscar(string nome)
    {
        _logger.LogInformation("Buscando pessoa: {Nome}", nome);

        var pessoa = _repository.GetByName(nome);
        if (pessoa is null)
        {
            _logger.LogWarning("Pessoa não encontrada: {Nome}", nome);
            return ApiResponse<PessoaResponseDto>.Fail(new List<string> { "Pessoa não encontrada" });
        }

        return ApiResponse<PessoaResponseDto>.Ok(new PessoaResponseDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade
        });
    }

    public ApiResponse<PessoaResponseDto> Atualizar(string nome, PessoaUpdateDto dto)
    {
        _logger.LogInformation("Atualizando pessoa: {Nome}", nome);

        var pessoa = _repository.GetByName(nome);
        if (pessoa is null)
        {
            _logger.LogWarning("Pessoa não encontrada para atualização: {Nome}", nome);
            return ApiResponse<PessoaResponseDto>.Fail(new List<string> { "Pessoa não encontrada" });
        }

        pessoa.Nome = dto.Nome;
        pessoa.Idade = dto.Idade;

        _repository.Update(pessoa);

        _logger.LogInformation("Pessoa atualizada: {Nome}", nome);

        return ApiResponse<PessoaResponseDto>.Ok(new PessoaResponseDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade
        });
    }

    public ApiResponse<bool> Remover(string nome)
    {
        _logger.LogInformation("Removendo pessoa: {Nome}", nome);

        var pessoa = _repository.GetByName(nome);
        if (pessoa is null)
        {
            _logger.LogWarning("Pessoa não encontrada para remoção: {Nome}", nome);
            return ApiResponse<bool>.Fail(new List<string> { "Pessoa não encontrada" });
        }

        _repository.Delete(pessoa);

        _logger.LogInformation("Pessoa removida: {Nome}", nome);

        return ApiResponse<bool>.Ok(true);
    }
}
