using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfGeneration;

/// <summary>
/// Обеспечивает централизованное определение констант, связанных с темой, включая цвета,
/// размеры и стили текста, для обеспечения согласованного визуального оформления во всем
/// документе.
/// </summary>
/// <remarks>
/// Класс <see cref="Theme"/> организует элементы темы во вложенные статические классы:
/// <list type="bullet">
///		<item>
///			<description>
///			<see cref="Colors"/> определяет палитру цветов, используемых для различных
///			элементов документа, таких как основные цвета, границы и текст.
///			</description>
///		</item>
///		<item>
///			<description>
///			<see cref="Sizes"/> определяет стандартные размеры, такие как размер шрифта и
///			высота элементов, для согласованной верстки и типографики.
///			</description>
///		</item>
///		<item>
///			<description>
///			<see cref="TextStyles"/> предоставляет предопределённые стили текста, сочетающие
///			размеры шрифтов, цвета и другие атрибуты для таких распространённых текстовых
///			элементов, как заголовки, названия и гиперссылки.
///			</description>
///		</item>
///	</list>
/// Этот класс предназначен для внутреннего использования и гарантирует, что все компоненты
/// пользовательского интерфейса будут придерживаться единой темы.
/// </remarks>
internal static class Theme
{
	internal static class Colors
	{
		public static readonly Color Primary = "#e0a80d";
		public static readonly Color Border = "#404242";
		public static readonly Color Background = "#2d2e2e";
		public static readonly Color MainText = "#979899";
		public static readonly Color Hyperlink = Primary;
		public static readonly Color SectionHeaderBackground = "#2b2c2c";
		public static readonly Color SectionHeaderText = "#a4a5a6";
		public static readonly Color TitleFirstWord = "#a4a5a6";
		public static readonly Color TitleSecondWord = Primary;
	}

	internal static class Sizes
	{
		public const float ProgressBarHeight = 5f;
		public const float DefaultFont = 12f;
		public const float PageNumberFont = 10f;
		public const float SectionHeaderFont = 18f;
		public const float SectionSubHeaderFont = 8f;
		public const float TitleFont = 36f;
		public const float InfoBlockTitleFont = 14f;
		public const float TimeLineTimeFrameFont = 8f;
		public const float TimeLinePositionFont = 10f;
		public const float TimeLineLocationFont = 8f;
	}

	internal static class TextStyles
	{
		public static readonly TextStyle Default = TextStyle.Default
			.DisableFontFeature(FontFeatures.StandardLigatures)
			.FontSize(Sizes.DefaultFont)
			.FontColor(Colors.MainText);

		public static readonly TextStyle PageNumber = Default
			.FontSize(Sizes.PageNumberFont);

		public static readonly TextStyle Hyperlink = Default
			.FontColor(Colors.Hyperlink)
			.Underline();

		public static readonly TextStyle SectionHeader = Default
			.FontSize(Sizes.SectionHeaderFont)
			.FontColor(Colors.SectionHeaderText);

		public static readonly TextStyle SectionSubHeader = SectionHeader
			.FontSize(Sizes.SectionSubHeaderFont);

		public static readonly TextStyle Title = Default
			.FontSize(Sizes.TitleFont);

		public static readonly TextStyle InfoBlockTitle = Default
			.FontSize(Sizes.InfoBlockTitleFont)
			.Bold();

		public static readonly TextStyle SkillTitle = Default
			.Bold();

		public static readonly TextStyle SkillPercent = SkillTitle
			.FontColor(Colors.Primary);

		public static readonly TextStyle TimeLineTimeFrame = Default
			.FontSize(Sizes.TimeLineTimeFrameFont)
			.Light();

		public static readonly TextStyle TimeLinePosition = Default
			.FontSize(Sizes.TimeLinePositionFont);

		public static readonly TextStyle TimeLineLocation = Default
			.FontSize(Sizes.TimeLineLocationFont)
			.Light();
	}
}
