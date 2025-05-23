#pragma warning disable CA1515

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Описание кнопки, ссылающейся на профиль социальной сети.
/// </summary>
/// <param name="Uri">Ссылка на профиль социальной сети.</param>
/// <param name="FontClass">
/// Используемый для визуализации CSS-класс символа из шрифта "FontAwesome".
/// </param>
public sealed record SocialButton(Uri Uri, string FontClass);

internal sealed class SocialButtonValidator : AbstractValidator<SocialButton>
{
	public SocialButtonValidator()
	{
		RuleFor(item => item.Uri)
			.NotNull();

		RuleFor(item => item.FontClass)
			.NotEmpty();
	}
}
