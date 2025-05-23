using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class PortfolioItemValidatorTests : ModelValidatorTestsBase<PortfolioItem, PortfolioItemValidator>
{
	public static IEnumerable<(PortfolioItem, Expression<Func<PortfolioItem, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new PortfolioItem(
				Title: Fixture.Create<DataString>(),
				Description: Fixture.Create<DataString>(),
				Uri: null!,
				ImageUri: Fixture.Create<Uri>()),
			item => item.Uri,
			NotNullValidatorName);

		yield return (
			new PortfolioItem(
				Title: Fixture.Create<DataString>(),
				Description: Fixture.Create<DataString>(),
				Uri: Fixture.Create<Uri>(),
				ImageUri: null!),
			item => item.ImageUri,
			NotNullValidatorName);
	}

	[TestMethod(ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		PortfolioItem value,
		Expression<Func<PortfolioItem, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);

	[TestMethod(ValidateBaseClassDisplayName)]
	public void ValidateBaseClass()
	{
		// Arrange.
		DataString invalidDataString = new(
			En: null!,
			Ru: Fixture.Create<string>());

		PortfolioItem value = new(
			Title: invalidDataString,
			Description: invalidDataString,
			Uri: Fixture.Create<Uri>(),
			ImageUri: Fixture.Create<Uri>());

		// Act/Assert.
		ValidateWithInvalidValue(
			value,
			(item => item.Title.En, NotEmptyValidatorName),
			(item => item.Description.En, NotEmptyValidatorName));
	}
}
