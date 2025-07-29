using FluentValidation;
using MovimentosManual.Application.Models.Request;

namespace MovimentosManual.Application.Validators
{
    public class ProdutoCosifRequestValidator : AbstractValidator<ProdutoCosifRequest>
    {
        public ProdutoCosifRequestValidator()
        {
            RuleFor(x => x.CodigoProduto)
                .NotEmpty().WithMessage("O código do produto é obrigatório.")
                .MaximumLength(10);

            RuleFor(x => x.CodigoCosif)
                .NotEmpty().WithMessage("O código COSIF é obrigatório.")
                .MaximumLength(10);

            RuleFor(x => x.CodigoClassificacao)
                .NotEmpty().WithMessage("O código de classificação é obrigatório.")
                .MaximumLength(10);

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status é obrigatório.")
                .Length(1).WithMessage("O status deve conter apenas 1 caractere.");
        }
    }
}
