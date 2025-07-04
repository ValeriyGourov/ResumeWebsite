#pragma warning disable CA1515

using System.Globalization;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Application.Components;

/// <summary>
/// Событие на временной линии с периодом в виде месяца и года.
/// </summary>
/// <param name="localizer">
/// <inheritdoc
///		cref="TimeLineItem(IStringLocalizer{TimeLineItem})"
///		path="/param[@name='localizer']"/>
/// </param>
public sealed partial class MonthYearTimeLineItem(IStringLocalizer<TimeLineItem> localizer)
	: TimeLineItem(localizer)
{
	/// <summary>
	/// Дата начала события.
	/// </summary>
	[Parameter, EditorRequired]
	public required DateOnly StartDate { get; set; }

	/// <summary>
	/// Дата окончания события. Если не указана, то событие считается активным.
	/// </summary>
	[Parameter, EditorRequired]
	public DateOnly? EndDate { get; set; }

	/// <summary>
	/// Отдельные секции с описанием деятельности в организации или учреждении.
	/// </summary>
	[Parameter, EditorRequired]
	public required IEnumerable<MonthYearTimeLineItemSection> Sections { get; set; }

	private static string? FormatPeriod(DateOnly? date)
		=> date?.ToString("Y", CultureInfo.CurrentCulture);
}

/// <summary>
/// Отдельная секция с описанием деятельности в организации или учреждении.
/// </summary>
/// <param name="Description">Общее описание секции.</param>
/// <param name="Details">Перечень подробных сведений о секции при их наличии.</param>
public sealed record MonthYearTimeLineItemSection(
	string Description,
	IEnumerable<string>? Details = null);
