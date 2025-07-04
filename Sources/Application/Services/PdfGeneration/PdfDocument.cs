using System.Globalization;
using System.Xml;

using Application.Data.Models;
using Application.Services.PdfGeneration.Components;

using Localization.Infrastructure;

using Microsoft.Extensions.Localization;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using IndexPage = Application.Components.Pages.Index;

namespace Application.Services.PdfGeneration;

/// <summary>
/// Представляет собой PDF-документ, который может быть составлен и отображён с
/// использованием предоставленных данных резюме.
/// </summary>
/// <remarks>
/// Этот класс реализует интерфейс <see cref="IDocument"/> для создания PDF-документа на
/// основе предоставленных данных о резюме. Он поддерживает настройку метаданных, макета
/// и содержания документа, включая такие разделы, как знания, навыки, опыт и т. д.
/// </remarks>
internal class PdfDocument : IDocument
{
	private readonly IWebHostEnvironment _webHostEnvironment;
	private readonly IStringLocalizer<IndexPage> _localizerIndex;
	private readonly IStringLocalizer<Application.Components.TimeLineItem> _localizerTimeLineItem;
	private readonly ResumeData _resumeData;

	private readonly string _title;
	private readonly XmlWriterSettings _svgWriterSettings = new()
	{
		OmitXmlDeclaration = true
	};

	private static readonly Dictionary<string, SvgImage> _svgCache = [];

	public PdfDocument(
		IWebHostEnvironment webHostEnvironment,
		IStringLocalizer<IndexPage> localizerIndex,
		IStringLocalizer<Application.Components.TimeLineItem> localizerTimeLineItem,
		ResumeData resumeData)
	{
		_webHostEnvironment = webHostEnvironment;
		_localizerIndex = localizerIndex;
		_localizerTimeLineItem = localizerTimeLineItem;
		_resumeData = resumeData;

		_title = $"{_resumeData.Name} {_resumeData.Surname} - {_resumeData.Title}";
	}

	DocumentMetadata IDocument.GetMetadata()
	{
		string author = $"{_resumeData.Name} {_resumeData.Surname}";

		return new()
		{
			Author = author,
			Title = _title,
			Keywords = author + ", Resume, резюме, developer, разработчик, C#, .NET, ASP.NET, Blazor",
			Language = CultureChanger.CurrentUICulture.Name,
			Producer = nameof(QuestPDF),
			CreationDate = DateTimeOffset.Now,
			ModifiedDate = DateTimeOffset.Now
		};
	}

	public void Compose(IDocumentContainer container)
	{
		_ = container.Page(page =>
		{
			page.ContinuousSize(PageSizes.A4.Width);
			page.MarginHorizontal(1f, Unit.Centimetre);
			page.DefaultTextStyle(Theme.TextStyles.Default);
			page.PageColor(Theme.Colors.Background);

			page.Content().Element(Content);
		});
	}

	private void Content(IContainer container)
	{
		container
			.PaddingVertical(10f)
			.Column(column =>
			{
				column.Spacing(20f);

				column.Item().Element(Title);
				column.Item().Element(SocialButtons);
				column.Item().Element(Contacts);
				column.Item().Element(Intro);
				column.Item().Element(Expertise);
				column.Item().Element(Skills);
				column.Item().Element(Experience);
				column.Item().Element(Education);
				column.Item().Element(Profiles);
				// TODO: Вывести награды (Awards).
				column.Item().Element(Portfolio);
			});
	}

	private void Title(IContainer container)
	{
		container
			.BorderBottom(1f)
			.BorderColor(Theme.Colors.Border)
			.PaddingBottom(10f)
			.Column(column =>
			{
#pragma warning disable CA1308 // Нормализуйте строки до прописных букв
				column.Item()
					.Text(_resumeData.Name.ToString().ToLowerInvariant())
					.Style(Theme.TextStyles.Title)
					.FontColor(Theme.Colors.TitleFirstWord)
					.Light();
#pragma warning restore CA1308 // Нормализуйте строки до прописных букв

				column.Item()
					.PaddingTop(-10f)
					.Text(_resumeData.Surname.ToString().ToUpperInvariant())
					.Style(Theme.TextStyles.Title)
					.FontColor(Theme.Colors.TitleSecondWord)
					.Bold();

				column.Item()
					.PaddingTop(10f)
					.Text(_resumeData.Title);
			});
	}

	private void SocialButtons(IContainer container)
	{
		container.Column(column =>
		{
			foreach (SocialButton socialButton in _resumeData.SocialButtons)
			{
				string uri = socialButton.Uri.ToString();

				column.Item()
					.Hyperlink(uri)
					.Row(row =>
					{
						_ = row.AutoItem()
							.Width(16f)
							.Svg(GetSvgImage(socialButton.IconName, Theme.Colors.Primary));

						_ = row.ConstantItem(5f);

						row.AutoItem()
							.AlignMiddle()
							.Text(uri)
							.Style(Theme.TextStyles.Hyperlink);
					});
			}
		});
	}

	private void Contacts(IContainer container)
	{
		container.Column(column =>
		{
			foreach (ContactItem contact in _resumeData.Contacts)
			{
				column.Item().Text(text =>
				{
					_ = text.Span($"{contact.Title}: ")
						.Bold();

					_ = contact.Hyperlink is null
						? text.Span(contact.Description)
						: text.Hyperlink(
							contact.Description,
							contact.Hyperlink.AbsoluteUri)
							.Style(Theme.TextStyles.Hyperlink);
				});
			}
		});
	}

	private void Intro(IContainer container) => _ = container.Text(_resumeData.Intro);

	private void Expertise(IContainer container)
	{
		if (_resumeData.Expertise?.Any() != true)
		{
			return;
		}

		const float counterItemSize = 30f;

		ResumeSection(
			container,
			"ExpertiseTitle",
			"ExpertiseDescription",
			contentColumn =>
			{
				contentColumn.Spacing(15f);

				byte counter = 0;

				foreach (TitleElement expertiseItem in _resumeData.Expertise)
				{
					counter++;

					contentColumn.Item()
						.ShowEntire()
						.Decoration(decoration =>
						{
							decoration.Before()
								.PaddingBottom(5f)
								.DefaultTextStyle(Theme.TextStyles.InfoBlockTitle)
								.Row(row =>
								{
									row.ConstantItem(counterItemSize)
										.Text(counter.ToString("D2", CultureInfo.InvariantCulture))
										.FontColor(Theme.Colors.Primary);

									row.RelativeItem()
										.Text(expertiseItem.Title);
								});

							decoration.Content()
								.Row(row =>
								{
									row.ConstantItem(counterItemSize);
									row.RelativeItem()
										.Text(expertiseItem.Description);
								});
						});
				}
			});
	}

	private void Skills(IContainer container)
	{
		if (_resumeData.Skills?.Any() != true)
		{
			return;
		}

		static void CreateProgressBarElement(RowDescriptor row, SkillItem? skillItem) => row
			.RelativeItem()
			.Column(innerColumn =>
			{
				if (skillItem is null)
				{
					return;
				}

				innerColumn.Spacing(5f);

				innerColumn.Item().Row(innerRow =>
				{
					_ = innerRow.ConstantItem(24f)
						.Text($"{skillItem.Percent.ToString(CultureChanger.CurrentUICulture)}%")
						.Style(Theme.TextStyles.SkillPercent);

					_ = innerRow.ConstantItem(10f);

					_ = innerRow.RelativeItem()
						.Text(skillItem.Title)
						.Style(Theme.TextStyles.SkillTitle);
				});

				innerColumn.Item().Dynamic(new SkillProgressBarComponent(skillItem.Percent));
			});

		const int columnNumber = 2;

		ResumeSection(
			container,
			"SkillsTitle",
			"SkillsDescription",
			contentColumn =>
			{
				foreach (SkillItem[] skillItems in _resumeData.Skills.Chunk(columnNumber))
				{
					contentColumn.Spacing(20f);

					contentColumn.Item().Row(row =>
					{
						row.Spacing(10f);

						CreateProgressBarElement(row, skillItems[0]);

						CreateProgressBarElement(
							row,
							skillItems.Length == columnNumber
								? skillItems[1]
								: null);
					});
				}
			});
	}

	private void Experience(IContainer container)
	{
		if (_resumeData.Experience is null
			|| _resumeData.Experience.Count == 0)
		{
			return;
		}

		TimeLine(
			container,
			"ExperienceTitle",
			"ExperienceDescription",
			_resumeData.Experience,
			item => item.StartDate,
			item => item.EndDate,
			"Y",
			(contentColumn, timeLineItem)
				=>
			{
				contentColumn.Item().Column(column =>
				{
					column.Spacing(5f);

					foreach (ExperienceProject project in timeLineItem.Projects)
					{
						column.Item().Text(project.Description);

						if (project.Details?.Any() != true)
						{
							continue;
						}

						foreach (DataString detail in project.Details)
						{
							column.Item().Row(row =>
							{
								row.ConstantItem(20);
								row.AutoItem().Text("●");
								row.ConstantItem(5);
								row.RelativeItem().Text(detail);
							});
						}
					}
				});
			});
	}

	private void Education(IContainer container)
	{
		if (_resumeData.Education is null
			|| _resumeData.Education.Count == 0)
		{
			return;
		}

		TimeLine(
			container,
			"EducationTitle",
			"EducationDescription",
			_resumeData.Education,
			item => item.StartYear,
			item => item.EndYear,
			"",
			(contentColumn, timeLineItem)
				=> contentColumn.Item().Text(timeLineItem.Description));
	}

	private void Profiles(IContainer container)
	{
		if (_resumeData.Profiles?.Any() != true)
		{
			return;
		}

		const float
			imageItemSize = 16f,
			spaceBetweenColumns = 10f;

		ResumeSection(
			container,
			"ProfilesTitle",
			"ProfilesDescription",
			contentColumn =>
			{
				contentColumn.Spacing(10f);

				foreach (ProfileItem profileItem in _resumeData.Profiles)
				{
					contentColumn.Item()
						.ShowEntire()
						.Decoration(decoration =>
						{
							decoration.Before()
								.DefaultTextStyle(Theme.TextStyles.InfoBlockTitle)
								.Row(row =>
								{
									row.ConstantItem(imageItemSize)
										.Svg(GetSvgImage(profileItem.IconName, Theme.Colors.MainText));

									_ = row.ConstantItem(spaceBetweenColumns);

									row.RelativeItem()
										.Text(text => text
											.Hyperlink(
												profileItem.Title,
												profileItem.Uri.AbsoluteUri)
											.Style(Theme.TextStyles.Hyperlink));
								});

							decoration.Content()
								.Row(row =>
								{
									row.ConstantItem(imageItemSize + spaceBetweenColumns);

									row.RelativeItem()
										.Text(profileItem.Description);
								});
						});
				}
			});
	}

	private void Portfolio(IContainer container)
	{
		if (_resumeData.Portfolio?.Any() != true)
		{
			return;
		}

		ResumeSection(
			container,
			"PortfolioTitle",
			"PortfolioDescription",
			contentColumn =>
			{
				contentColumn.Spacing(10f);

				foreach (PortfolioItem portfolioItem in _resumeData.Portfolio)
				{
					contentColumn.Item()
						.ShowEntire()
						.Decoration(decoration =>
						{
							decoration.Before()
								.DefaultTextStyle(Theme.TextStyles.InfoBlockTitle)
								.Text(text => text
									.Hyperlink(
										portfolioItem.Title,
										portfolioItem.Uri.AbsoluteUri)
									.Style(Theme.TextStyles.Hyperlink));

							decoration.Content()
								.PaddingTop(5f).Text(portfolioItem.Description);
						});
				}
			});
	}

	private void ResumeSection(
		IContainer container,
		string headerResourceName,
		string subHeaderResourceName,
		Action<ColumnDescriptor> contentColumnHandler)
	{
		container.Decoration(decoration =>
		{
			decoration.Before()
				.ShowEntire()
				.ExtendHorizontal()
				.BorderHorizontal(1f)
				.BorderColor(Theme.Colors.Border)
				.Background(Theme.Colors.SectionHeaderBackground)
				.PaddingVertical(10f)
				.Column(column =>
				{
					column.Item()
						.Text(_localizerIndex[headerResourceName])
						.Style(Theme.TextStyles.SectionHeader);

					column.Item()
						.Text(_localizerIndex[subHeaderResourceName])
						.Style(Theme.TextStyles.SectionSubHeader);
				});

			decoration.Content()
				.PaddingTop(10f)
				.Column(contentColumnHandler);
		});
	}

	private void TimeLine<T, TProperty>(
		IContainer container,
		string headerResourceName,
		string subHeaderResourceName,
		IEnumerable<T> timeLineItems,
		Func<T, TProperty> periodStartProperty,
		Func<T, TProperty?> periodEndProperty,
		string periodFormat,
		Action<ColumnDescriptor, T> contentColumnHandler)
		where T : TimeLineItemBase<T>
	{
		ResumeSection(
			container,
			headerResourceName,
			subHeaderResourceName,
			contentColumn =>
			{
				contentColumn.Spacing(20f);

				foreach (T timeLineItem in timeLineItems)
				{
					contentColumn.Item().Row(row =>
					{
						row.Spacing(10f);

						row.RelativeItem(1)
							.Column(column =>
							{
								column.Spacing(5f);

								// TODO: Вынести форматирование дат периодов в отдельный модуль и использовать его везде, где нужно форматировать даты периодов.

								string? FormatPeriod(Func<T, TProperty?> periodProperty)
								{
									TProperty? periodValue = periodProperty(timeLineItem);

									// TODO: Выяснить почему не изменяется CultureInfo.CurrentCulture при изменение языка интерфейса. Соответственно неправильно форматируются даты в формируемом файле.
									return periodValue is null
										? null
										: string.Format(
											CultureInfo.CurrentCulture,
											$"{{0:{periodFormat}}}",
											periodValue);
								}

								string?
									startDate = FormatPeriod(periodStartProperty),
									endDate = FormatPeriod(periodEndProperty) ?? _localizerTimeLineItem["EndPeriodPresent"];

								_ = column.Item()
									.Text($"{startDate} - {endDate}")
									.Style(Theme.TextStyles.TimeLineTimeFrame);

								_ = column.Item()
									.Text(timeLineItem.Institution)
									.Style(Theme.TextStyles.InfoBlockTitle);

								_ = column.Item()
									.Text(timeLineItem.Position)

									.Style(Theme.TextStyles.TimeLinePosition);

								_ = column.Item()
									.Text(timeLineItem.Location)

									.Style(Theme.TextStyles.TimeLineLocation);
							});

						row.RelativeItem(2)
							.Column(column => contentColumnHandler(column, timeLineItem));
					});
				}
			});
	}

	private SvgImage GetSvgImage(string iconName, string fillColor)
	{
		if (_svgCache.TryGetValue(iconName, out SvgImage? svgImage))
		{
			return svgImage;
		}

		// TODO: Вынести формирование имени файла во вспомогательный класс. Использовать его для формирования данных и страниц веб-приложения.

		string imagePath = Path.Combine(
			_webHostEnvironment.WebRootPath,
			$@"images\icons\{iconName}.svg");

		string svgContent = File.ReadAllText(imagePath);
		svgContent = SetSvgFillColor(svgContent, fillColor);

		svgImage = SvgImage.FromText(svgContent);
		_svgCache[iconName] = svgImage;

		return svgImage;
	}

	private string SetSvgFillColor(string svgContent, string color)
	{
		XmlDocument document = new();
		document.LoadXml(svgContent);
		document.GetElementsByTagName("path")
			.Cast<XmlElement>()
			.First()
			.SetAttribute("fill", color);

		using StringWriter stringWriter = new();
		using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, _svgWriterSettings);
		document.WriteTo(xmlWriter);
		xmlWriter.Flush();

		return stringWriter.ToString();
	}
}
