#pragma warning disable CA1515

using Application.Data.Models;
using Application.Infrastructure.Validation;

using AutoFixture;

using FluentValidation;
using FluentValidation.Validators;

using ChildValidatorAdaptorType = FluentValidation.Validators.ChildValidatorAdaptor<
	Application.UnitTests.Infrastructure.Validation.TestData,
	Application.Data.Models.DataString>;

namespace Application.UnitTests.Infrastructure.Validation;

[TestClass]
internal class ValidationToolsTests
{
	[TestMethod("Для свойства должны быть добавлены необходимые модули проверки")]
	public void SetDataStringRule1()
	{
		// Arrange.
		TestDataValidator validator = new();

		// Act.
		validator.SetDataStringRule(item => item.MyProperty1);

		// Assert.
		validator.CheckPropertyValidators();
	}

	[TestMethod("Для свойства должен использоваться предоставленный экземпляр " + nameof(DataStringValidator))]
	public void SetDataStringRule2()
	{
		// Arrange.
		DataStringValidator dataStringValidator = new();
		TestDataValidator validator = new();

		// Act.
		validator.SetDataStringRule(
			dataStringValidator,
			item => item.MyProperty1);

		// Assert.
		validator.CheckProvidedValidator(dataStringValidator);
	}

	[TestMethod("Для свойства коллекции должны быть добавлены необходимые модули проверки")]
	public void SetDataStringRuleForEach1()
	{
		// Arrange.
		TestDataValidator validator = new();

		// Act.
		validator.SetDataStringRuleForEach(item => item.MyCollection1);

		// Assert.
		validator.CheckPropertyValidators();
	}

	[TestMethod("Для свойства коллекции должен использоваться предоставленный экземпляр " + nameof(DataStringValidator))]
	public void SetDataStringRuleForEach2()
	{
		// Arrange.
		DataStringValidator dataStringValidator = new();
		TestDataValidator validator = new();

		// Act.
		validator.SetDataStringRuleForEach(
			dataStringValidator,
			item => item.MyCollection1);

		// Assert.
		validator.CheckProvidedValidator(dataStringValidator);
	}
}

file record TestData(
	DataString MyProperty1,
	DataString MyProperty2,
	IEnumerable<DataString> MyCollection1);

file class TestDataValidator : AbstractValidator<TestData>;

static file class TestDataValidatorExtensions
{
	private static readonly Fixture _fixture = new();

	public static void CheckPropertyValidators(this TestDataValidator validator)
		=> validator.CreateDescriptor().Rules
			.Should()
			.ContainSingle(rules
				=> rules.Components.Any(static component => component.Validator is NotNullValidator<TestData, DataString>)
				&& rules.Components.Any(static component
					=> component.Validator is ChildValidatorAdaptorType
					&& ((ChildValidatorAdaptorType)component.Validator).ValidatorType == typeof(DataStringValidator)));

	public static void CheckProvidedValidator(
		this TestDataValidator validator,
		DataStringValidator dataStringValidator)
	{
		DataString value = _fixture.Create<DataString>();
		IEnumerable<DataString> collection = _fixture.CreateMany<DataString>();
		ValidationContext<TestData> context = new(new TestData(value, value, collection));

		validator.CreateDescriptor().Rules
			.Should()
			.ContainSingle()
			.Which.Components
			.Should()
			.Contain(component
				=> component.Validator is ChildValidatorAdaptorType
				&& ((ChildValidatorAdaptorType)component.Validator).GetValidator(context, value) == dataStringValidator);
	}
}
