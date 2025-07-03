#pragma warning disable CA1515

using Application.Infrastructure.Validation;

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Событие на временной линии с периодом в виде месяца и года.
/// </summary>
/// <param name="Institution">
/// <inheritdoc cref="TimeLineItemBase{T}" path="/param[@name='Institution']"/>
/// </param>
/// <param name="Position">
/// <inheritdoc cref="TimeLineItemBase{T}" path="/param[@name='Position']"/>
/// </param>
/// <param name="Location">
/// <inheritdoc cref="TimeLineItemBase{T}" path="/param[@name='Location']"/>
/// </param>
/// <param name="Projects">Описание деятельности в организации или учреждении.</param>
/// <param name="StartDate"><inheritdoc cref="StartDate" path="/summary"/></param>
/// <param name="EndDate"><inheritdoc cref="EndDate" path="/summary"/></param>
public sealed record MonthYearTimeLineItem(
	DataString Institution,
	DataString Position,
	DataString Location,
	IEnumerable<ExperienceProject> Projects,
	DateOnly StartDate,
	DateOnly? EndDate)
	: TimeLineItemBase<MonthYearTimeLineItem>(Institution, Position, Location)
{
	/// <summary>
	/// Дата начала события. При присваивании значения дата преобразуется к началу месяца.
	/// </summary>
	public DateOnly StartDate { get; init; } = MonthBeginning(StartDate);

	/// <summary>
	/// Дата окончания события. Если не указана, то событие считается активным.
	/// При присваивании значения дата преобразуется к началу месяца.
	/// </summary>
	public DateOnly? EndDate { get; init; } = EndDate.HasValue
		? MonthBeginning(EndDate.Value)
		: null;

	/// <inheritdoc/>
	public override int CompareTo(MonthYearTimeLineItem? other)
	{
		if (other is null)
		{
			return 1;
		}

		DateOnly now = DateOnly.FromDateTime(DateTime.UtcNow);
		DateOnly otherEndDate = other.EndDate ?? now;
		DateOnly endDate = EndDate ?? now;

		int compareResult = otherEndDate.CompareTo(endDate);
		if (compareResult == 0)
		{
			compareResult = other.StartDate.CompareTo(StartDate);
		}

		return compareResult;
	}

	/// <summary>
	/// Преобразует дату к началу месяца.
	/// </summary>
	/// <param name="date">Исходная дата.</param>
	/// <returns>Дата первого числа месяца.</returns>
	private static DateOnly MonthBeginning(DateOnly date)
		=> new(date.Year, date.Month, 1);
}

/// <summary>
/// Отдельный проект в рамках опыта работы.
/// </summary>
/// <param name="Description">Общее описание проекта.</param>
/// <param name="Details">Перечень подробных сведений о проекте при их наличии.</param>
public sealed record ExperienceProject(
	DataString Description,
	IEnumerable<DataString>? Details = null);

internal sealed class MonthYearTimeLineItemValidator : AbstractValidator<MonthYearTimeLineItem>
{
	public MonthYearTimeLineItemValidator()
	{
		Include(new TimeLineItemBaseValidator<MonthYearTimeLineItem>());

		RuleFor(item => item.StartDate)
			.NotEmpty();

		RuleFor(item => item.Projects)
			.NotEmpty();

		RuleForEach(item => item.Projects)
			.ChildRules(static childValidator =>
			{
				childValidator.SetDataStringRule(item => item.Description);

				childValidator.When(
					static item => item.Details is not null,
					() => childValidator.SetDataStringRuleForEach(item => item.Details!));
			});

		When(
			item => item.EndDate is not null,
			() => RuleFor(item => item.EndDate)
				.GreaterThanOrEqualTo(item => item.StartDate));
	}
}
