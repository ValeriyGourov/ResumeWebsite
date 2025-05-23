using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

using FluentValidation.TestHelper;
using FluentValidation.Validators;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class DataStringValidatorTests
{
	private static readonly Fixture _fixture = new();

	[TestMethod("При корректном значении ошибок проверки быть не должно")]
	public void Validate1()
	{
		// Arrange.
		DataString value = _fixture.Create<DataString>();
		DataStringValidator validator = new();

		// Act.
		TestValidationResult<DataString> result = validator.TestValidate(value);

		// Assert.
		result.ShouldNotHaveAnyValidationErrors();
	}

	public static IEnumerable<(DataString, Expression<Func<DataString, string>>)> Validate2TestData()
	{
		string en = _fixture.Create<string>();
		string ru = _fixture.Create<string>();

		yield return (new DataString(null!, ru), item => item.En);
		yield return (new DataString(string.Empty, ru), item => item.En);

		yield return (new DataString(en, null!), item => item.Ru);
		yield return (new DataString(en, string.Empty), item => item.Ru);
	}

	[TestMethod("При недопустимом значении проверка должна завершиться ошибкой")]
	[DynamicData(nameof(Validate2TestData))]
	public void Validate2(DataString value, Expression<Func<DataString, string>> property)
	{
		// Arrange.
		DataStringValidator validator = new();

		// Act.
		TestValidationResult<DataString> result = validator.TestValidate(value);

		// Assert.
		_ = result.ShouldHaveValidationErrorFor(property)
			.WithErrorCode(nameof(NotEmptyValidator<object, object>));
	}
}
