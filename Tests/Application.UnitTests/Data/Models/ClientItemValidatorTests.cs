using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class ClientItemValidatorTests : ModelValidatorTestsBase<ClientItem, ClientItemValidator>
{
	public static IEnumerable<(ClientItem, Expression<Func<ClientItem, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new ClientItem(
				Name: null!,
				Uri: Fixture.Create<Uri>(),
				Logo: Fixture.Create<Uri>()),
			item => item.Name,
			NotNullValidatorName);

		yield return (
			new ClientItem(
				Name: Fixture.Create<DataString>(),
				Uri: null!,
				Logo: Fixture.Create<Uri>()),
			item => item.Uri,
			NotNullValidatorName);

		yield return (
			new ClientItem(
				Name: Fixture.Create<DataString>(),
				Uri: Fixture.Create<Uri>(),
				Logo: null!),
			item => item.Logo,
			NotNullValidatorName);

		yield return (
			new ClientItem(
				Name: new DataString(
					En: null!,
					Ru: Fixture.Create<string>()),
				Uri: Fixture.Create<Uri>(),
				Logo: Fixture.Create<Uri>()),
			item => item.Name.En,
			NotEmptyValidatorName);
	}

	[TestMethod(DisplayName = ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		ClientItem value,
		Expression<Func<ClientItem, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);
}
