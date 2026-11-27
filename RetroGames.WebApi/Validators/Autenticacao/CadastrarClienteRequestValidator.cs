using FluentValidation;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Validators.Autenticacao;

public class CadastrarClienteRequestValidator : AbstractValidator<CadastrarClienteRequestApiModel>
{
    public CadastrarClienteRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Informe o seu nome.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Informe o e-mail.")
            .EmailAddress().WithMessage("E-mail em formato inválido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Informe a senha.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
    }
}