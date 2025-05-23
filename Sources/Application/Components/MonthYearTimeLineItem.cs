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
///		cref="TimeLineItemBase(IStringLocalizer{TimeLineItemBase})"
///		path="/param[@name='localizer']"/>
/// </param>
public sealed class MonthYearTimeLineItem(IStringLocalizer<TimeLineItemBase> localizer)
	: TimeLineItemBase(localizer)
{
	/// <summary>
	/// Дата начала события.
	/// </summary>
	[Parameter]
	public DateOnly StartDate { get; set; }

	/// <summary>
	/// Дата окончания события. Если не указана, то событие считается активным.
	/// </summary>
	[Parameter]
	public DateOnly? EndDate { get; set; }

	/// <inheritdoc/>
	protected override void OnParametersSet()
	{
		StartPeriod = StartDate.ToString("Y", CultureInfo.CurrentCulture);
		EndPeriod = EndDate?.ToString("Y", CultureInfo.CurrentCulture);
	}
}
