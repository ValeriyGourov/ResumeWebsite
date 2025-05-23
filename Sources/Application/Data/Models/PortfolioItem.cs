#pragma warning disable CA1515

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Модель для представления данных раздела "Портфолио".
/// </summary>
/// <param name="Title">
/// <inheritdoc cref="TitleElement" path="/param[@name='Title']"/>
/// </param>
/// <param name="Description">
/// <inheritdoc cref="TitleElement" path="/param[@name='Description']"/>
/// </param>
/// <param name="Uri">Адрес работы.</param>
/// <param name="ImageUri">Адрес картинки для предварительного просмотра.</param>
public sealed record PortfolioItem(
	DataString Title,
	DataString Description,
	Uri Uri,
	Uri ImageUri)
	: TitleElement(Title, Description);

internal sealed class PortfolioItemValidator : AbstractValidator<PortfolioItem>
{
	public PortfolioItemValidator()
	{
		Include(new TitleElementValidator());

		RuleFor(item => item.Uri)
			.NotNull();

		RuleFor(item => item.ImageUri)
			.NotNull();
	}
}
