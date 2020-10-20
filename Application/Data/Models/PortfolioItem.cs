using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Data.Models
{
	/// <summary>
	/// Модель для представления данных раздела "Портфолио".
	/// </summary>
	public sealed class PortfolioItem : TitleElement
	{
		/// <summary>
		/// Адрес полноразмерной картинки.
		/// </summary>
		[Required]
		public Uri FullImageUri { get; set; } = null!;

		/// <summary>
		/// Адрес картинки для предпросмотра.
		/// </summary>
		[Required]
		public Uri ThumbImageUri { get; set; } = null!;
	}
}
