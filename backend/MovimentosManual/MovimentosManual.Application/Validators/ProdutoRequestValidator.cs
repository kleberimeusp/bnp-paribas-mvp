using FluentValidation;
using MovimentosManual.Application.Models.Request;

namespace MovimentosManual.Application.Validators
{
    public class ProdutoRequestValidator : AbstractValidator<ProdutoRequest>
    {
        public ProdutoRequestValidator()
        {
            RuleFor(x => x.CodigoProduto )
                .NotEmpty().WithMessage("O código do produto é obrigatório.")
                .MaximumLength(10).WithMessage("O código do produto deve ter no máximo 10 caracteres.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do produto é obrigatória.")
                .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status do produto é obrigatório.")
                .Length(1).WithMessage("O status deve conter apenas 1 caractere (A/I).");
        }
    }
}
