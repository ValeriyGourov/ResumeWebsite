using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

namespace Application.Data.Models;

/// <summary>
/// Данные для отображения в резюме.
/// </summary>
public sealed class ResumeData
{
	/// <summary>
	///	Имя владельца резюме.
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Name { get; set; } = null!;

	/// <summary>
	///	Фамилия владельца резюме.
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Surname { get; set; } = null!;

	/// <summary>
	/// Заголовок (профессия).
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Title { get; set; } = null!;

	/// <summary>
	/// Кнопки со ссылками на профили социальных сетей.
	/// </summary>
	[ValidateComplexType]
	public IEnumerable<SocialButton> SocialButtons { get; set; } = Enumerable.Empty<SocialButton>();

	/// <summary>
	/// Контактная информация.
	/// </summary>
	[ValidateComplexType]
	public IEnumerable<ContactItem> Contacts { get; set; } = Enumerable.Empty<ContactItem>();

	/// <summary>
	/// Общая информация о себе в произвольной форме.
	/// </summary>
	[ValidateComplexType]
	public DataString? Intro { get; set; }

	/// <summary>
	/// Перечень компетенций.
	/// </summary>
	[ValidateComplexType]
	public IEnumerable<TitleElement>? Expertise { get; set; }

	/// <summary>
	/// Перечень навыков.
	/// </summary>
	[ValidateComplexType]
	public IEnumerable<SkillItem>? Skills { get; set; }

	/// <summary>
	/// Перечень опыта работы.
	/// </summary>
	[ValidateComplexType]
	public SortedSet<MonthYearTimeLineItem>? Experience { get; set; }

	/// <summary>
	/// Перечень учебных заведений.
	/// </summary>
	[ValidateComplexType]
	public SortedSet<YearTimeLineItem>? Education { get; set; }

	/// <summary>
	/// Профессиональные профили.
	/// </summary>
	[ValidateComplexType]
	public IEnumerable<ProfileItem>? Profiles { get; set; }

	/// <summary>
	/// Полученные награды.
	/// </summary>
	[ValidateComplexType]
	public IEnumerable<TitleElement>? Awards { get; set; }

	/// <summary>
	/// Портфолио выполненных работ.
	/// </summary>
	[ValidateComplexType]
	public IEnumerable<PortfolioItem>? Portfolio { get; set; }

	/// <summary>
	/// Клиенты, которым оказывались услуги.
	/// </summary>
	[ValidateComplexType]
	public IEnumerable<ClientItem>? Clients { get; set; }
}