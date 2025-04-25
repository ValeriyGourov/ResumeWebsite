using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestInfrastructure;

namespace Application.UnitTests.Infrastructure.Validation;

[TestClass]
public class ComplexTypeValidationResultTests
{
	[TestMethod]
	[TestProperty(TestProperties.Constructor, null)]
	public void ThrowsArgumentNullExceptionInConstructorIfValidationResultsIsNull()
	{
		// Arrange.
		const string errorMessage = "Test";
		IEnumerable<ValidationResult>? validationResults = null;

		// Act.
		void act()
		{
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
			_ = new ComplexTypeValidationResult(errorMessage, validationResults);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
		}

		// Assert.
		Assert.ThrowsException<ArgumentNullException>(act);
	}

	[TestMethod]
	[TestProperty(TestProperties.PropertyName, nameof(ComplexTypeValidationResult.ValidationResults))]
	public void ValidationResultsMustBeFiledAfterCreation()
	{
		// Arrange.
		const string errorMessage = "Test";
		IEnumerable<ValidationResult> validationResults = new List<ValidationResult>
		{
			new("Test 1"),
			new("Test 2")
		};

		// Act.
		ComplexTypeValidationResult validationResult = new(errorMessage, validationResults);

		// Assert.
		CollectionAssert.AreEquivalent(validationResults.ToArray(), validationResult.ValidationResults.ToArray());
	}
}
