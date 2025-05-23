#pragma warning disable CA1515

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Событие на временной линии с периодом в виде года.
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
/// <param name="Description">
/// <inheritdoc cref="TimeLineItemBase{T}" path="/param[@name='Description']"/>
/// </param>
/// <param name="StartYear">Год начала события.</param>
/// <param name="EndYear">
/// Год окончания события. Если не указан, то событие считается активным.
/// </param>
public sealed record YearTimeLineItem(
	DataString Institution,
	DataString Position,
	DataString Location,
	DataString Description,
	int StartYear,
	int? EndYear)
	: TimeLineItemBase<YearTimeLineItem>(Institution, Position, Location, Description)
{
	/// <inheritdoc/>
	public override int CompareTo(YearTimeLineItem? other)
	{
		if (other is null)
		{
			return -1;
		}

		int yearNow = DateTimeOffset.UtcNow.Year;
		int otherEndYear = other.EndYear ?? yearNow;
		int endYear = EndYear ?? yearNow;

		int compareResult = otherEndYear.CompareTo(endYear);
		if (compareResult == 0)
		{
			compareResult = other.StartYear.CompareTo(StartYear);
		}

		return compareResult;
	}
}

internal sealed class YearTimeLineItemValidator : AbstractValidator<YearTimeLineItem>
{
	public YearTimeLineItemValidator()
	{
		Include(new TimeLineItemBaseValidator<YearTimeLineItem>());

		RuleFor(item => item.StartYear)
			.GreaterThan(0);

		When(
			item => item.EndYear is not null,
			() => RuleFor(item => item.EndYear)
				.GreaterThanOrEqualTo(item => item.StartYear));
	}
}
