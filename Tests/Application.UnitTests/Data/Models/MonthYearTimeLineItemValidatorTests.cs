using System.Linq.Expressions;
using System.Security.Cryptography;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class MonthYearTimeLineItemValidatorTests
	: ModelValidatorTestsBase<MonthYearTimeLineItem, MonthYearTimeLineItemValidator>
{
	protected override MonthYearTimeLineItem CreateValidModel()
	{
		DateOnly startDate = Fixture.Create<DateOnly>();
		DateOnly endDate = startDate.AddYears(RandomNumberGenerator.GetInt32(1, 100));

		return new(
			Institution: Fixture.Create<DataString>(),
			Position: Fixture.Create<DataString>(),
			Location: Fixture.Create<DataString>(),
			Projects: Fixture.CreateMany<ExperienceProject>(),
			StartDate: startDate,
			EndDate: endDate);
	}

	public static IEnumerable<(MonthYearTimeLineItem, Expression<Func<MonthYearTimeLineItem, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new MonthYearTimeLineItem(
				Institution: Fixture.Create<DataString>(),
				Position: Fixture.Create<DataString>(),
				Location: Fixture.Create<DataString>(),
				Projects: Fixture.CreateMany<ExperienceProject>(),
				StartDate: DateOnly.MinValue,
				EndDate: null),
			item => item.StartDate,
			NotEmptyValidatorName);

		DateOnly startDate = Fixture.Create<DateOnly>();
		DateOnly endDate = startDate.AddYears(-1);

		yield return (
			new MonthYearTimeLineItem(
				Institution: Fixture.Create<DataString>(),
				Position: Fixture.Create<DataString>(),
				Location: Fixture.Create<DataString>(),
				Projects: Fixture.CreateMany<ExperienceProject>(),
				StartDate: startDate,
				EndDate: endDate),
			item => item.EndDate!,
			GreaterThanOrEqualValidatorName);
	}

	[TestMethod(ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		MonthYearTimeLineItem value,
		Expression<Func<MonthYearTimeLineItem, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);

	[TestMethod(ValidateBaseClassDisplayName)]
	public void ValidateBaseClass()
	{
		// Arrange.
		DataString invalidDataString = new(
			En: null!,
			Ru: Fixture.Create<string>());

		MonthYearTimeLineItem value = new(
			Institution: invalidDataString,
			Position: invalidDataString,
			Location: invalidDataString,
			Projects: [new(
				invalidDataString,
				[invalidDataString])],
			StartDate: Fixture.Create<DateOnly>(),
			EndDate: null);

		// Act/Assert.
		ValidateWithInvalidValue(
			value,
			(item => item.Institution.En, NotEmptyValidatorName),
			(item => item.Position.En, NotEmptyValidatorName),
			(item => item.Location.En, NotEmptyValidatorName));

		ValidateWithInvalidValue(
			value,
			GetCollectionItemPropertyName(
				item => item.Projects,
				item => item.Description.En),
			NotEmptyValidatorName);

		// TODO: Добавить проверку для свойства Projects.Details[].En.
	}
}
