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
	private static readonly Fixture _fixture = new();

	[TestMethod("Должны быть добавлены необходимые модули проверки")]
	public void SetDataStringRule1()
	{
		// Arrange.
		TestDataValidator validator = new();

		// Act.
		validator.SetDataStringRule(item => item.MyProperty1);

		// Assert.
		validator.CreateDescriptor().Rules
			.Should()
			.ContainSingle(rules
				=> rules.Components.Any(static component => component.Validator is NotNullValidator<TestData, DataString>)
				&& rules.Components.Any(static component
					=> component.Validator is ChildValidatorAdaptorType
					&& ((ChildValidatorAdaptorType)component.Validator).ValidatorType == typeof(DataStringValidator)));
	}

	[TestMethod("Должен использоваться предоставленный экземпляр " + nameof(DataStringValidator))]
	public void SetDataStringRule2()
	{
		// Arrange.
		DataString value = _fixture.Create<DataString>();
		ValidationContext<TestData> context = new(new TestData(value, value));

		DataStringValidator dataStringValidator = new();
		TestDataValidator validator = new();

		// Act.
		validator.SetDataStringRule(
			dataStringValidator,
			item => item.MyProperty1);

		// Assert.
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

file record TestData(
	DataString MyProperty1,
	DataString MyProperty2);

file class TestDataValidator : AbstractValidator<TestData>;
