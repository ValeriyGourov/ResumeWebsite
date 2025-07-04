using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfGeneration.Components;

/// <summary>
/// Представляет собой динамический компонент индикатора прогресса навыков, который
/// отображает прогресс в процентах.
/// </summary>
/// <remarks>
/// Этот компонент создаёт индикатор выполнения с шириной, пропорциональной указанному
/// проценту. Прогресс-бар оформляется с использованием цветов и размеров текущей темы.
/// </remarks>
/// <param name="percent">Отображаемый процент прогресса, в диапазоне от 0 до 100.</param>
internal class SkillProgressBarComponent(byte percent) : IDynamicComponent
{
	private readonly byte _percent = percent;

	public DynamicComponentComposeResult Compose(DynamicContext context)
	{
		float width = context.AvailableSize.Width / 100 * _percent;

		IDynamicElement content = context.CreateElement(element => _ = element
			.Background(Theme.Colors.Border)
			.Height(Theme.Sizes.ProgressBarHeight)
			.Width(width)
			.Background(Theme.Colors.Primary));

		return new DynamicComponentComposeResult
		{
			Content = content,
			HasMoreContent = false
		};
	}
}
