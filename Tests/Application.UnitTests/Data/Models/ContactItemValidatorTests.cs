using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class ContactItemValidatorTests : ModelValidatorTestsBase<ContactItem, ContactItemValidator>
{
	public static IEnumerable<(ContactItem, Expression<Func<ContactItem, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new ContactItem(
				Title: Fixture.Create<DataString>(),
				Description: Fixture.Create<DataString>(),
				Hyperlink: new Uri(Fixture.Create<string>(), UriKind.Relative)),
			item => item.Hyperlink!,
			PredicateValidatorName);
	}

	[TestMethod(ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		ContactItem value,
		Expression<Func<ContactItem, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);

	[TestMethod(ValidateBaseClassDisplayName)]
	public void ValidateBaseClass()
	{
		// Arrange.
		DataString invalidDataString = new(
			En: null!,
			Ru: Fixture.Create<string>());

		ContactItem value = new(
			Title: invalidDataString,
			Description: invalidDataString,
			Hyperlink: null);

		// Act/Assert.
		ValidateWithInvalidValue(
			value,
			(item => item.Title.En, NotEmptyValidatorName),
			(item => item.Description.En, NotEmptyValidatorName));
	}
}
