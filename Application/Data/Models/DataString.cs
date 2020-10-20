using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Localization.Infrastructure;

namespace Application.Data.Models
{
	/// <summary>
	/// Представляет строковое значение на различных языках.
	/// </summary>
	public sealed class DataString
	{
		private static readonly CultureInfo _ruCultureInfo = CultureInfo.GetCultureInfo("ru");

		/// <summary>
		/// Текст значения на английском языке.
		/// </summary>
		[Required]
		public string En { get; set; } = null!;

		/// <summary>
		/// Текст значения на русском языке.
		/// </summary>
		[Required]
		public string Ru { get; set; } = null!;

		/// <inheritdoc/>
		public override string ToString()
		{
			if (CultureChanger.CurrentUICulture.LCID == _ruCultureInfo.LCID)
			{
				return Ru;
			}
			else
			{
				return En;
			}
		}

		public static implicit operator string(DataString? dataString) =>
			dataString is null
				? string.Empty
				: dataString.ToString();
	}
}
