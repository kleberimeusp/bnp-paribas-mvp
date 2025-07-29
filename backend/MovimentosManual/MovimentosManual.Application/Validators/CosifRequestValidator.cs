using FluentValidation;
using MovimentosManual.Application.Models.Request;

namespace MovimentosManual.Application.Validators;

public class CosifRequestValidator : AbstractValidator<CosifRequest>
{
    public CosifRequestValidator()
    {
        RuleFor(x => x.CodigoCosif)
            .NotEmpty().WithMessage("Código COSIF é obrigatório.")
            .MaximumLength(20);

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MaximumLength(100);

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status é obrigatório.")
            .Must(s => s == "A" || s == "I").WithMessage("Status deve ser 'A' ou 'I'.");
    }
}
