#pragma warning disable CA1515

using System.Globalization;

using FluentValidation;

using Localization.Infrastructure;

namespace Application.Data.Models;

/// <summary>
/// Представляет строковое значение на различных языках.
/// </summary>
/// <param name="En">Текст значения на английском языке.</param>
/// <param name="Ru">Текст значения на русском языке.</param>
public sealed record DataString(string En, string Ru)
{
	private static readonly CultureInfo _ruCultureInfo = CultureInfo.GetCultureInfo("ru");

	/// <inheritdoc/>
	public override string ToString() => CultureChanger.CurrentUICulture.TwoLetterISOLanguageName == _ruCultureInfo.TwoLetterISOLanguageName ? Ru : En;

#pragma warning disable CS1591
	public static implicit operator string?(DataString? dataString) => dataString?.ToString();
#pragma warning restore CS1591
}

internal sealed class DataStringValidator : AbstractValidator<DataString>
{
	public DataStringValidator()
	{
		_ = RuleFor(item => item.En)
			.NotEmpty();

		_ = RuleFor(item => item.Ru)
			.NotEmpty();
	}
}
