#pragma warning disable CA1515

using Application.Infrastructure.Validation;

using FluentValidation;

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
/// <param name="Achievements">Перечень достижений.</param>
/// <param name="Expertise">Перечень компетенций.</param>
/// <param name="Skills">Перечень навыков.</param>
/// <param name="Experience">Перечень опыта работы.</param>
/// <param name="Education">Перечень учебных заведений.</param>
/// <param name="Profiles">Профессиональные профили.</param>
/// <param name="Awards">Полученные награды.</param>
/// <param name="Portfolio">Портфолио выполненных работ.</param>
/// <param name="Clients">Клиенты, которым оказывались услуги.</param>
public sealed record ResumeData(
	DataString Name,
	DataString Surname,
	DataString Title,
	IEnumerable<SocialButton>? SocialButtons = null,
	IEnumerable<ContactItem>? Contacts = null,
	DataString? Intro = null,
	IEnumerable<DataString>? Achievements = null,
	IEnumerable<TitleElement>? Expertise = null,
	IEnumerable<SkillItem>? Skills = null,
	SortedSet<MonthYearTimeLineItem>? Experience = null,
	SortedSet<YearTimeLineItem>? Education = null,
	IEnumerable<ProfileItem>? Profiles = null,
	IEnumerable<TitleElement>? Awards = null,
	IEnumerable<PortfolioItem>? Portfolio = null,
	IEnumerable<ClientItem>? Clients = null)
{
	/// <summary>
	/// Кнопки со ссылками на профили социальных сетей.
	/// </summary>
	public IEnumerable<SocialButton> SocialButtons { get; init; } = SocialButtons ?? [];

	/// <summary>
	/// Контактная информация.
	/// </summary>
	public IEnumerable<ContactItem> Contacts { get; init; } = Contacts ?? [];
}

internal sealed class ResumeDataValidator : AbstractValidator<ResumeData>
{
	public ResumeDataValidator()
	{
		DataStringValidator dataStringValidator = new();

		this.SetDataStringRule(
			dataStringValidator,
			item => item.Name,
			item => item.Surname,
			item => item.Title);

		When(
			static item => item.Achievements is not null,
			() => this.SetDataStringRuleForEach(
				item => item.Achievements!,
				dataStringValidator));

		RuleForEach(item => item.SocialButtons)
			.SetValidator(new SocialButtonValidator());

		RuleForEach(item => item.Contacts)
			.SetValidator(new ContactItemValidator());

		When(
			static item => item.Intro is not null,
			() => this.SetDataStringRule(
				dataStringValidator,
				item => item.Intro!));

		RuleForEach(item => item.Expertise)
			.SetValidator(new TitleElementValidator());

		RuleForEach(item => item.Skills)
			.SetValidator(new SkillItemValidator());

		RuleForEach(item => item.Experience)
			.SetValidator(new MonthYearTimeLineItemValidator());

		RuleForEach(item => item.Education)
			.SetValidator(new YearTimeLineItemValidator());

		RuleForEach(item => item.Profiles)
			.SetValidator(new ProfileItemValidator());

		RuleForEach(item => item.Awards)
			.SetValidator(new TitleElementValidator());

		RuleForEach(item => item.Portfolio)
			.SetValidator(new PortfolioItemValidator());

		RuleForEach(item => item.Clients)
			.SetValidator(new ClientItemValidator());
	}
}
