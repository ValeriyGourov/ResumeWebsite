using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class SkillItemValidatorTests : ModelValidatorTestsBase<SkillItem, SkillItemValidator>
{
	protected override SkillItem CreateValidModel() => new(
		Title: Fixture.Create<DataString>(),
		Percent: 50);

	public static IEnumerable<(SkillItem, Expression<Func<SkillItem, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new SkillItem(
				Title: new(
					En: null!,
					Ru: Fixture.Create<string>()),
				Percent: 50),
			item => item.Title.En,
			NotEmptyValidatorName);

		yield return (
			new SkillItem(
				Title: Fixture.Create<DataString>(),
				Percent: 0),
			item => item.Percent,
			InclusiveBetweenValidatorName);

		yield return (
			new SkillItem(
				Title: Fixture.Create<DataString>(),
				Percent: 101),
			item => item.Percent,
			InclusiveBetweenValidatorName);
	}

	[TestMethod(DisplayName = ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		SkillItem value,
		Expression<Func<SkillItem, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);
}
