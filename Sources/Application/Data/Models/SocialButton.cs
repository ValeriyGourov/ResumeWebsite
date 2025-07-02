#pragma warning disable CA1515

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Описание кнопки, ссылающейся на профиль социальной сети.
/// </summary>
/// <param name="Uri">Ссылка на профиль социальной сети.</param>
/// <param name="IconName">
/// Имя файла SVG (без расширения файла), используемого для визуализации элемента.
/// Файл должен находиться в папке "wwwroot/images/icons" приложения.
/// </param>
public sealed record SocialButton(Uri Uri, string IconName);

internal sealed class SocialButtonValidator : AbstractValidator<SocialButton>
{
	public SocialButtonValidator()
	{
		RuleFor(item => item.Uri)
			.NotNull();

		RuleFor(item => item.IconName)
			.NotEmpty();
	}
}
