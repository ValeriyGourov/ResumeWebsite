using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class ProfileItemValidatorTests : ModelValidatorTestsBase<ProfileItem, ProfileItemValidator>
{
	public static IEnumerable<(ProfileItem, Expression<Func<ProfileItem, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new ProfileItem(
				Title: Fixture.Create<DataString>(),
				Description: Fixture.Create<DataString>(),
				Uri: null!,
				IconName: Fixture.Create<string>()),
			item => item.Uri,
			NotNullValidatorName);

		yield return (
			new ProfileItem(
				Title: Fixture.Create<DataString>(),
				Description: Fixture.Create<DataString>(),
				Uri: Fixture.Create<Uri>(),
				IconName: null!),
			item => item.IconName,
			NotEmptyValidatorName);

		yield return (
			new ProfileItem(
				Title: Fixture.Create<DataString>(),
				Description: Fixture.Create<DataString>(),
				Uri: Fixture.Create<Uri>(),
				IconName: string.Empty),
			item => item.IconName,
			NotEmptyValidatorName);
	}

	[TestMethod(DisplayName = ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		ProfileItem value,
		Expression<Func<ProfileItem, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);

	[TestMethod(DisplayName = ValidateBaseClassDisplayName)]
	public void ValidateBaseClass()
	{
		// Arrange.
		DataString invalidDataString = new(
			En: null!,
			Ru: Fixture.Create<string>());

		ProfileItem value = new(
			Title: invalidDataString,
			Description: invalidDataString,
			Uri: Fixture.Create<Uri>(),
			IconName: Fixture.Create<string>());

		// Act/Assert.
		ValidateWithInvalidValue(
			value,
			(item => item.Title.En, NotEmptyValidatorName),
			(item => item.Description.En, NotEmptyValidatorName));
	}
}
