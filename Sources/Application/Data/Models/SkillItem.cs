#pragma warning disable CA1515

using Application.Infrastructure.Validation;

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Описание навыка, измеренного в процентах.
/// </summary>
/// <param name="Title">Название навыка.</param>
/// <param name="Percent">Степень владения навыком, выраженная в процентах от 0 до 100.</param>
public sealed record SkillItem(DataString Title, byte Percent);

internal sealed class SkillItemValidator : AbstractValidator<SkillItem>
{
	public SkillItemValidator()
	{
		this.SetDataStringRule(item => item.Title);

		RuleFor(item => (int)item.Percent)
			.InclusiveBetween(1, 100);
	}
}
