#pragma warning disable CA1515

using System.ComponentModel.DataAnnotations;

namespace Application.Data.Models;

/// <summary>
/// Описание кнопки, ссылающейся на профиль социальной сети.
/// </summary>
/// <param name="Uri">Ссылка на профиль социальной сети.</param>
/// <param name="FontClass">Используемый для визуализации CSS-класс символа из шрифта "FontAwesome".</param>
public sealed record SocialButton(
	[property: Required] Uri Uri,
	[property: Required] string FontClass);
