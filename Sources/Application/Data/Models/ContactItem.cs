#pragma warning disable CA1515

namespace Application.Data.Models;

/// <summary>
/// Модель для представления данных раздела "Контакты".
/// </summary>
/// <param name="Title">
/// <inheritdoc cref="TitleElement" path="/param[@name='Title']"/>
/// </param>
/// <param name="Description">
/// <inheritdoc cref="TitleElement" path="/param[@name='Description']"/>
/// </param>
/// <param name="Hyperlink">
/// Гиперссылка контакта. Поддерживается любой тип специализированных ссылок, таких как
/// "mailto:", "tel:" и т.п.
/// </param>
public sealed record ContactItem(
	DataString Title,
	DataString Description,
	string? Hyperlink)
	: TitleElement(Title, Description);
