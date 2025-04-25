#pragma warning disable CA1515

using Application.Infrastructure.Validation;

namespace Application.UnitTests.Infrastructure.Validation;

[TestClass]
public class ComplexTypeValidationResultTests
{
	[TestMethod]
	[TestProperty(TestProperties.Constructor, "")]
	public void ThrowsArgumentNullExceptionInConstructorIfValidationResultsIsNull()
	{
		// Arrange.
		const string errorMessage = "Test";
		IEnumerable<ValidationResult>? validationResults = null;

		// Act.
#pragma warning disable CS8604
		Action act = () => _ = new ComplexTypeValidationResult(errorMessage, validationResults);
#pragma warning restore CS8604

		// Assert.
		act.Should().ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(validationResults));
	}

	[TestMethod]
	[TestProperty(TestProperties.PropertyName, nameof(ComplexTypeValidationResult.ValidationResults))]
	public void ValidationResultsMustBeFiledAfterCreation()
	{
		// Arrange.
		const string errorMessage = "Test";
		IEnumerable<ValidationResult> validationResults =
		[
			new("Test 1"),
			new("Test 2")
		];

		// Act.
		ComplexTypeValidationResult validationResult = new(errorMessage, validationResults);

		// Assert.
		validationResult.ValidationResults.Should().BeEquivalentTo(validationResults);
	}
}
