using System.ComponentModel.DataAnnotations;

namespace Application.Data.Models;

/// <summary>
/// Описание кнопки, ссылающейся на профиль социальной сети.
/// </summary>
public sealed class SocialButton
{
	/// <summary>
	/// Ссылка на профиль социальной сети.
	/// </summary>
	[Required]
	public Uri Uri { get; set; } = null!;

	/// <summary>
	/// Используемый для визуализации CSS-класс символа из шрифта "FontAwesome".
	/// </summary>
	[Required]
	public string FontClass { get; set; } = null!;
}