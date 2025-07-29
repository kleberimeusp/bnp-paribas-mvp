using FluentValidation;
using MovimentosManual.Application.Models.Request;

namespace MovimentosManual.Application.Validators
{
    public class MovimentoManualRequestValidator : AbstractValidator<MovimentoManualRequest>
    {
        public MovimentoManualRequestValidator()
        {
            RuleFor(x => x.CodigoProduto)
                .NotEmpty().WithMessage("O código do produto é obrigatório.")
                .MaximumLength(10);

            RuleFor(x => x.CodigoCosif)
                .NotEmpty().WithMessage("O código COSIF é obrigatório.")
                .MaximumLength(10);

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(100);

            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor do movimento deve ser maior que zero.");

            RuleFor(x => x.DataMovimento)
                .NotEmpty().WithMessage("A data do movimento é obrigatória.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("A data do movimento não pode ser futura.");
        }
    }
}
