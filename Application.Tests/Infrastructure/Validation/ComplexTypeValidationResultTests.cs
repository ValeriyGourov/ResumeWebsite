using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestInfrastructure;

namespace Application.Infrastructure.Validation.Tests
{
	[TestClass]
	public class ComplexTypeValidationResultTests
	{
		[TestMethod]
		[TestProperty(TestProperties.Constructor, null)]
		public void ThrowsArgumentNullExceptionInConstructorIfValidationResultsIsNull()
		{
			// Arrange.
			string errorMessage = "Test";
			IEnumerable<ValidationResult>? validationResults = null;

			// Act.
			void act()
			{
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
				ComplexTypeValidationResult validationResult = new ComplexTypeValidationResult(errorMessage, validationResults);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
			};

			// Assert.
			Assert.ThrowsException<ArgumentNullException>(act);
		}

		[TestMethod]
		[TestProperty(TestProperties.PropertyName, nameof(ComplexTypeValidationResult.ValidationResults))]
		public void ValidationResultsMustBeFiledAfterCreation()
		{
			// Arrange.
			string errorMessage = "Test";
			IEnumerable<ValidationResult> validationResults = new List<ValidationResult>
			{
				new ValidationResult("Test 1"),
				new ValidationResult("Test 2")
			};

			// Act.
			ComplexTypeValidationResult validationResult = new ComplexTypeValidationResult(errorMessage, validationResults);


			// Assert.
			CollectionAssert.AreEquivalent(validationResults.ToArray(), validationResult.ValidationResults.ToArray());
		}
	}
}