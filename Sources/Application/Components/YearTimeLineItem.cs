#pragma warning disable CA1515

using System.Globalization;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Application.Components;

/// <summary>
/// Событие на временной линии с периодом в виде года.
/// </summary>
/// <param name="localizer">
/// <inheritdoc
///		cref="TimeLineItemBase(IStringLocalizer{TimeLineItemBase})"
///		path="/param[@name='localizer']"/>
/// </param>
public sealed class YearTimeLineItem(IStringLocalizer<TimeLineItemBase> localizer)
	: TimeLineItemBase(localizer)
{
	/// <summary>
	/// Год начала события.
	/// </summary>
	[Parameter]
	public int StartYear { get; set; }

	/// <summary>
	/// Год окончания события. Если не указан, то событие считается активным.
	/// </summary>
	[Parameter]
	public int? EndYear { get; set; }

	/// <inheritdoc/>
	protected override void OnParametersSet()
	{
		StartPeriod = StartYear.ToString(CultureInfo.CurrentCulture);
		EndPeriod = EndYear?.ToString(CultureInfo.CurrentCulture);
	}
}
