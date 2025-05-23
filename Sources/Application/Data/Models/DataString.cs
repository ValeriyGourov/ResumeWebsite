#pragma warning disable CA1515

using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Localization.Infrastructure;

namespace Application.Data.Models;

/// <summary>
/// Представляет строковое значение на различных языках.
/// </summary>
/// <param name="En">Текст значения на английском языке.</param>
/// <param name="Ru">Текст значения на русском языке.</param>
public sealed record DataString(
	[property: Required] string En,
	[property: Required] string Ru)
{
	private static readonly CultureInfo _ruCultureInfo = CultureInfo.GetCultureInfo("ru");

	/// <inheritdoc/>
	public override string ToString() => CultureChanger.CurrentUICulture.TwoLetterISOLanguageName == _ruCultureInfo.TwoLetterISOLanguageName ? Ru : En;

#pragma warning disable CS1591
	public static implicit operator string?(DataString? dataString) => dataString?.ToString();
#pragma warning restore CS1591
}
