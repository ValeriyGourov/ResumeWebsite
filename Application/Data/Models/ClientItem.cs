using System;
using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

namespace Application.Data.Models
{
	/// <summary>
	/// Модель для представления данных раздела "Клиенты".
	/// </summary>

	public sealed class ClientItem
	{
		/// <summary>
		/// Название клиента.
		/// </summary>
		[ValidateComplexType]
		public DataString? Name { get; set; }

		/// <summary>
		/// Адрес веб-сайта клиента.
		/// </summary>
		[Required]
		public Uri Uri { get; set; } = null!;

		/// <summary>
		/// Адрес картинки логотипа клиента.
		/// </summary>
		[Required]
		public Uri Logo { get; set; } = null!;
	}
}
