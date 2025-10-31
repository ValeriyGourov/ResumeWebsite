using System.Linq.Expressions;
using System.Security.Cryptography;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class YearTimeLineItemValidatorTests : ModelValidatorTestsBase<YearTimeLineItem, YearTimeLineItemValidator>
{
	protected override YearTimeLineItem CreateValidModel()
	{
		int startYear = Fixture.Create<int>();
		int endYear = startYear + RandomNumberGenerator.GetInt32(1, 100);

		return new(
			Institution: Fixture.Create<DataString>(),
			Position: Fixture.Create<DataString>(),
			Location: Fixture.Create<DataString>(),
			Description: Fixture.Create<DataString>(),
			StartYear: startYear,
			EndYear: endYear);
	}

	public static IEnumerable<(YearTimeLineItem, Expression<Func<YearTimeLineItem, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new YearTimeLineItem(
				Institution: Fixture.Create<DataString>(),
				Position: Fixture.Create<DataString>(),
				Location: Fixture.Create<DataString>(),
				Description: Fixture.Create<DataString>(),
				StartYear: 0,
				EndYear: null),
			item => item.StartYear,
			GreaterThanValidatorName);

		int startYear = Fixture.Create<int>();
		int endYear = startYear - 1;

		yield return (
			new YearTimeLineItem(
				Institution: Fixture.Create<DataString>(),
				Position: Fixture.Create<DataString>(),
				Location: Fixture.Create<DataString>(),
				Description: Fixture.Create<DataString>(),
				StartYear: startYear,
				EndYear: endYear),
			item => item.EndYear!,
			GreaterThanOrEqualValidatorName);
	}

	[TestMethod(DisplayName = ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		YearTimeLineItem value,
		Expression<Func<YearTimeLineItem, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);

	[TestMethod(DisplayName = ValidateBaseClassDisplayName)]
	public void ValidateBaseClass()
	{
		// Arrange.
		DataString invalidDataString = new(
			En: null!,
			Ru: Fixture.Create<string>());

		YearTimeLineItem value = new(
			Institution: invalidDataString,
			Position: invalidDataString,
			Location: invalidDataString,
			Description: invalidDataString,
			StartYear: Fixture.Create<int>(),
			EndYear: null);

		// Act/Assert.
		ValidateWithInvalidValue(
			value,
			(item => item.Institution.En, NotEmptyValidatorName),
			(item => item.Position.En, NotEmptyValidatorName),
			(item => item.Location.En, NotEmptyValidatorName),
			(item => item.Description.En, NotEmptyValidatorName));
	}
}
