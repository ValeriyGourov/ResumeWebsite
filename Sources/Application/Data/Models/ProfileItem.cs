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
/// <param name="IconName">
/// Имя файла SVG (без расширения файла), используемого для визуализации элемента.
/// Файл должен находиться в папке "wwwroot/images/icons" приложения.
/// </param>
public sealed record ProfileItem(
	DataString Title,
	DataString Description,
	Uri Uri,
	string IconName)
	: TitleElement(Title, Description);

internal sealed class ProfileItemValidator : AbstractValidator<ProfileItem>
{
	public ProfileItemValidator()
	{
		Include(new TitleElementValidator());

		RuleFor(item => item.Uri)
			.NotNull();

		RuleFor(item => item.IconName)
			.NotEmpty();
	}
}
