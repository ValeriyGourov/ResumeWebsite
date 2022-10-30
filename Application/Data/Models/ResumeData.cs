using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

namespace Application.Data.Models;

/// <summary>
/// Данные для отображения в резюме.
/// </summary>
/// <param name="Name">Имя владельца резюме.</param>
/// <param name="Surname">Фамилия владельца резюме.</param>
/// <param name="Title">Заголовок (профессия).</param>
/// <param name="SocialButtons"><inheritdoc cref="SocialButtons" path="/summary"/></param>
/// <param name="Contacts"><inheritdoc cref="Contacts" path="/summary"/></param>
/// <param name="Intro">Общая информация о себе в произвольной форме.</param>
/// <param name="Expertise">Перечень компетенций.</param>
/// <param name="Skills">Перечень навыков.</param>
/// <param name="Experience">Перечень опыта работы.</param>
/// <param name="Education">Перечень учебных заведений.</param>
/// <param name="Profiles">Профессиональные профили.</param>
/// <param name="Awards">Полученные награды.</param>
/// <param name="Portfolio">Портфолио выполненных работ.</param>
/// <param name="Clients">Клиенты, которым оказывались услуги.</param>
public sealed record ResumeData(
	[property: Required, ValidateComplexType] DataString Name,
	[property: Required, ValidateComplexType] DataString Surname,
	[property: Required, ValidateComplexType] DataString Title,
	[param: ValidateComplexType] IEnumerable<SocialButton>? SocialButtons,
	[param: ValidateComplexType] IEnumerable<ContactItem>? Contacts,
	[property: ValidateComplexType] DataString? Intro,
	[property: ValidateComplexType] IEnumerable<TitleElement>? Expertise,
	[property: ValidateComplexType] IEnumerable<SkillItem>? Skills,
	[property: ValidateComplexType] SortedSet<MonthYearTimeLineItem>? Experience,
	[property: ValidateComplexType] SortedSet<YearTimeLineItem>? Education,
	[property: ValidateComplexType] IEnumerable<ProfileItem>? Profiles,
	[property: ValidateComplexType] IEnumerable<TitleElement>? Awards,
	[property: ValidateComplexType] IEnumerable<PortfolioItem>? Portfolio,
	[property: ValidateComplexType] IEnumerable<ClientItem>? Clients)
{
	/// <summary>
	/// Кнопки со ссылками на профили социальных сетей.
	/// </summary>
	public IEnumerable<SocialButton> SocialButtons { get; init; } = SocialButtons ?? Enumerable.Empty<SocialButton>();

	/// <summary>
	/// Контактная информация.
	/// </summary>
	public IEnumerable<ContactItem> Contacts { get; init; } = Contacts ?? Enumerable.Empty<ContactItem>();
}