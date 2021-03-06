﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Data.Models
{
	/// <summary>
	/// Модель для представления данных раздела "Портфолио".
	/// </summary>
	public sealed class PortfolioItem : TitleElement
	{
		/// <summary>
		/// Адрес работы.
		/// </summary>
		[Required]
		public Uri Uri { get; set; } = null!;

		/// <summary>
		/// Адрес картинки для предпросмотра.
		/// </summary>
		[Required]
		public Uri ImageUri { get; set; } = null!;
	}
}
