#pragma warning disable CA1515

using System.ComponentModel.DataAnnotations;

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
	[property: Required] Uri Uri,
	[property: Required] Uri ImageUri)
	: TitleElement(Title, Description);
