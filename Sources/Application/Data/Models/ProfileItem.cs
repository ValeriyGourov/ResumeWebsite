#pragma warning disable CA1515

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Модель для представления данных раздела "Профили".
/// </summary>
/// <param name="Title">
/// <inheritdoc cref="TitleElement" path="/param[@name='Title']"/>
/// </param>
/// <param name="Description">
/// <inheritdoc cref="TitleElement" path="/param[@name='Description']"/>
/// </param>
/// <param name="Uri">Ссылка на профиль.</param>
/// <param name="FontClass">
/// Используемый для визуализации CSS-класс символа из шрифта "FontAwesome".
/// </param>
public sealed record ProfileItem(
	DataString Title,
	DataString Description,
	Uri Uri,
	string FontClass)
	: TitleElement(Title, Description);

internal sealed class ProfileItemValidator : AbstractValidator<ProfileItem>
{
	public ProfileItemValidator()
	{
		Include(new TitleElementValidator());

		RuleFor(item => item.Uri)
			.NotNull();

		RuleFor(item => item.FontClass)
			.NotEmpty();
	}
}
