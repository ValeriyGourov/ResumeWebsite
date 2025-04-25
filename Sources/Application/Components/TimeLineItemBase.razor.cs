using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Application.Components;

/// <summary>
/// Базовый класс для событий на временной линии.
/// </summary>
public abstract partial class TimeLineItemBase
{
	private string? _endPeriod;

	/// <summary>
	/// Локализатор строк.
	/// </summary>
	[Inject] private IStringLocalizer<TimeLineItemBase> Localizer { get; set; } = null!;

	/// <summary>
	/// Строковое представление периода начала события.
	/// </summary>
	protected string StartPeriod { get; set; } = null!;

	/// <summary>
	/// Строковое представление периода окончания события. Если не указана, то событие считается активным.
	/// </summary>
	protected string? EndPeriod
	{
		get => !string.IsNullOrWhiteSpace(_endPeriod) ? _endPeriod : Localizer["EndPeriodPresent"];
		set => _endPeriod = value;
	}

	/// <summary>
	/// Организация или учреждение, в котором происходило событие.
	/// </summary>
	[Parameter]
	public string Institution { get; set; } = null!;

	/// <summary>
	/// Занимаемая позиция в организации или учреждении.
	/// </summary>
	[Parameter]
	public string Position { get; set; } = null!;

	/// <summary>
	/// Местоположение организации или учреждении.
	/// </summary>
	[Parameter]
	public string Location { get; set; } = null!;

	/// <summary>
	/// Описание деятельности в организации или учреждении.
	/// </summary>
	[Parameter]
	public string Description { get; set; } = null!;
}