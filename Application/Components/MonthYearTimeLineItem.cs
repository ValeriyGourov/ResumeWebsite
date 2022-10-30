using System.Globalization;

using Microsoft.AspNetCore.Components;

namespace Application.Components;

/// <summary>
/// Событие на временной линии с периодом в виде месяца и года.
/// </summary>
public class MonthYearTimeLineItem : TimeLineItemBase
{
	/// <summary>
	/// Дата начала события.
	/// </summary>
	[Parameter]
	public DateTimeOffset StartDate { get; set; }

	/// <summary>
	/// Дата окончания события. Если не указана, то событие считается активным.
	/// </summary>
	[Parameter]
	public DateTimeOffset? EndDate { get; set; }

	/// <inheritdoc/>
	protected override void OnParametersSet()
	{
		StartPeriod = StartDate.ToString("Y", CultureInfo.CurrentCulture);
		EndPeriod = EndDate?.ToString("Y", CultureInfo.CurrentCulture);
	}
}