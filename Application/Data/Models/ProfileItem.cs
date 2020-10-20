using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Data.Models
{
	/// <summary>
	/// Модель для представления данных раздела "Профили".
	/// </summary>
	public sealed class ProfileItem : TitleElement
	{
		/// <summary>
		/// Ссылка на профиль.
		/// </summary>
		[Required]
		public Uri Uri { get; set; } = null!;

		/// <summary>
		/// Используемый для визуализации CSS-класс символа из шрифта "FontAwesome".
		/// </summary>
		[Required]
		public string FontClass { get; set; } = null!;
	}
}
