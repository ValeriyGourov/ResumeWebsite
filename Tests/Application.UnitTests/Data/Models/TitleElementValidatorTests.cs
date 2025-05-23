using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class TitleElementValidatorTests : ModelValidatorTestsBase<TitleElement, TitleElementValidator>
{
	public static IEnumerable<(TitleElement, Expression<Func<TitleElement, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new TitleElement(
				Title: null!,
				Description: Fixture.Create<DataString>()),
			item => item.Title,
			NotNullValidatorName);

		yield return (
			new TitleElement(
				Title: Fixture.Create<DataString>(),
				Description: null!),
			item => item.Description,
			NotNullValidatorName);

		yield return (
			new TitleElement(
				Title: new DataString(
					En: null!,
					Ru: Fixture.Create<string>()),
				Description: Fixture.Create<DataString>()),
			item => item.Title.En,
			NotEmptyValidatorName);

		yield return (
			new TitleElement(
				Title: Fixture.Create<DataString>(),
				Description: new DataString(
					En: null!,
					Ru: Fixture.Create<string>())),
			item => item.Description.En,
			NotEmptyValidatorName);
	}

	[TestMethod(ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		TitleElement value,
		Expression<Func<TitleElement, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);
}
