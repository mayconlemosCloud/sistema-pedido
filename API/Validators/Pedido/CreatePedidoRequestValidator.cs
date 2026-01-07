using FluentValidation;
using API.Dtos.Pedido;

namespace API.Validators.Pedido;

/// <summary>
/// Validador para CreatePedidoRequest
/// </summary>
public class CreatePedidoRequestValidator : AbstractValidator<CreatePedidoRequest>
{
    public CreatePedidoRequestValidator()
    {
        RuleFor(p => p.ClienteId)
            .NotEmpty().WithMessage("ClienteId é obrigatório")
            .NotEqual(Guid.Empty).WithMessage("ClienteId não pode ser vazio");

        RuleFor(p => p.Itens)
            .NotEmpty().WithMessage("Pedido deve ter pelo menos um item")
            .NotNull().WithMessage("Itens é obrigatório");

        RuleForEach(p => p.Itens)
            .SetValidator(new CreatePedidoItemRequestValidator())
            .When(p => p.Itens != null);
    }
}

/// <summary>
/// Validador para CreatePedidoItemRequest
/// </summary>
public class CreatePedidoItemRequestValidator : AbstractValidator<CreatePedidoItemRequest>
{
    public CreatePedidoItemRequestValidator()
    {
        RuleFor(i => i.ProdutoId)
            .NotEmpty().WithMessage("ProdutoId é obrigatório")
            .NotEqual(Guid.Empty).WithMessage("ProdutoId não pode ser vazio");

        RuleFor(i => i.Quantidade)
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero")
            .LessThanOrEqualTo(int.MaxValue).WithMessage("Quantidade inválida");
    }
}
