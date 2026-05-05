using MinhaApi.Models.Dtos;
using FluentValidation;

namespace MinhaApi.Validators;

public class PessoaUpdateValidator : AbstractValidator<PessoaUpdateDto>
{
    public PessoaUpdateValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(2).WithMessage("O nome deve ter pelo menos 2 caracteres.")
            .MaximumLength(50).WithMessage("O nome deve ter no máximo 50 caracteres.")
            .Matches("^[a-zA-ZÀ-ÿ0-9 ]+$")
            .WithMessage("O nome contém caracteres inválidos.");

        RuleFor(x => x.Idade)
            .NotNull().WithMessage("A idade é obrigatória.")
            .InclusiveBetween(1, 120)
            .WithMessage("A idade deve estar entre 1 e 120 anos.");
    }
}
