using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

using FluentValidation;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class TimeLineItemBaseValidatorTests
	: ModelValidatorTestsBase<TestTimeLineItemBase, TestTimeLineItemBaseValidator>
{
	public static IEnumerable<(TestTimeLineItemBase, Expression<Func<TestTimeLineItemBase, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new TestTimeLineItemBase(
				Institution: null!,
				Position: Fixture.Create<DataString>(),
				Location: Fixture.Create<DataString>()),
			item => item.Institution,
			NotNullValidatorName);

		yield return (
			new TestTimeLineItemBase(
				Institution: Fixture.Create<DataString>(),
				Position: null!,
				Location: Fixture.Create<DataString>()),
			item => item.Position,
			NotNullValidatorName);

		yield return (
			new TestTimeLineItemBase(
				Institution: Fixture.Create<DataString>(),
				Position: Fixture.Create<DataString>(),
				Location: null!),
			item => item.Location,
			NotNullValidatorName);

		yield return (
			new TestTimeLineItemBase(
				Institution: new DataString(
					En: null!,
					Ru: Fixture.Create<string>()),
				Position: Fixture.Create<DataString>(),
				Location: Fixture.Create<DataString>()),
			item => item.Institution.En,
			NotEmptyValidatorName);

		yield return (
			new TestTimeLineItemBase(
				Institution: Fixture.Create<DataString>(),
				Position: new DataString(
					En: null!,
					Ru: Fixture.Create<string>()),
				Location: Fixture.Create<DataString>()),
			item => item.Position.En,
			NotEmptyValidatorName);

		yield return (
			new TestTimeLineItemBase(
				Institution: Fixture.Create<DataString>(),
				Position: Fixture.Create<DataString>(),
				Location: new DataString(
					En: null!,
					Ru: Fixture.Create<string>())),
			item => item.Location.En,
			NotEmptyValidatorName);
	}

	[TestMethod(ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		TestTimeLineItemBase value,
		Expression<Func<TestTimeLineItemBase, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);
}

internal sealed record TestTimeLineItemBase(
	DataString Institution,
	DataString Position,
	DataString Location)
	: TimeLineItemBase<TestTimeLineItemBase>(Institution, Position, Location)
{
	public override int CompareTo(TestTimeLineItemBase? other) => throw new NotImplementedException();
}

internal sealed class TestTimeLineItemBaseValidator : AbstractValidator<TestTimeLineItemBase>
{
	public TestTimeLineItemBaseValidator() => Include(new TimeLineItemBaseValidator<TestTimeLineItemBase>());
}
