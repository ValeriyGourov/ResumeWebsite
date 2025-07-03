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
///		cref="TimeLineItem(IStringLocalizer{TimeLineItem})"
///		path="/param[@name='localizer']"/>
/// </param>
public sealed partial class YearTimeLineItem(IStringLocalizer<TimeLineItem> localizer)
	: TimeLineItem(localizer)
{
	/// <summary>
	/// Год начала события.
	/// </summary>
	[Parameter]
	[EditorRequired]
	public int StartYear { get; set; }

	/// <summary>
	/// Год окончания события. Если не указан, то событие считается активным.
	/// </summary>
	[Parameter]
	[EditorRequired]
	public int? EndYear { get; set; }

	/// <summary>
	/// Описание деятельности в организации или учреждении.
	/// </summary>
	[Parameter]
	[EditorRequired]
	public string Description { get; set; } = null!;

	private static string? FormatPeriod(int? year)
		=> year?.ToString(CultureInfo.CurrentCulture);
}
