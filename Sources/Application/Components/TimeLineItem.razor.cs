#pragma warning disable CA1515

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Application.Components;

/// <summary>
/// Базовый класс для событий на временной линии.
/// </summary>
/// <param name="localizer">Локализатор строк.</param>
public partial class TimeLineItem(IStringLocalizer<TimeLineItem> localizer)
{
	/// <summary>
	/// Строковое представление периода начала события.
	/// </summary>
	[Parameter]
	public string StartPeriod { get; set; } = null!;

	/// <summary>
	/// Строковое представление периода окончания события. Если не указана, то событие считается активным.
	/// </summary>
	[Parameter]
	public string? EndPeriod { get; set; }

	/// <summary>
	/// Организация или учреждение, в котором происходило событие.
	/// </summary>
	[Parameter]
	[EditorRequired]
	public string Institution { get; set; }

	/// <summary>
	/// Занимаемая позиция в организации или учреждении.
	/// </summary>
	[Parameter]
	[EditorRequired]
	public string Position { get; set; }

	/// <summary>
	/// Местоположение организации или учреждении.
	/// </summary>
	[Parameter]
	[EditorRequired]
	public string Location { get; set; }

	/// <summary>
	/// Содержимое элемента временной линии, которое будет отображаться внутри карточки события.
	/// </summary>
	[Parameter]
	public RenderFragment Content { get; set; } = null!;

	private string GetEndPeriod()
		=> !string.IsNullOrWhiteSpace(EndPeriod)
			? EndPeriod
			: localizer["EndPeriodPresent"];
}
