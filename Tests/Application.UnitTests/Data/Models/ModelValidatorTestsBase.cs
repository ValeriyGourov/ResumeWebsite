using System.Linq.Expressions;
using System.Text;

using AutoFixture;

using FluentValidation;
using FluentValidation.TestHelper;
using FluentValidation.Validators;

namespace Application.UnitTests.Data.Models;

internal abstract class ModelValidatorTestsBase
{
	public const string ValidateWithIncorrectDataDisplayName = "При недопустимом значении проверка должна завершиться ошибкой";
	public const string ValidateBaseClassDisplayName = "Должны вызываться проверки базового класса";

	public const string NotNullValidatorName = nameof(NotNullValidator<object, object>);
	public const string NotEmptyValidatorName = nameof(NotEmptyValidator<object, object>);
	public const string PredicateValidatorName = nameof(PredicateValidator<object, object>);
	public const string GreaterThanValidatorName = nameof(GreaterThanValidator<object, int>);
	public const string GreaterThanOrEqualValidatorName = nameof(GreaterThanOrEqualValidator<object, int>);
	public const string InclusiveBetweenValidatorName = nameof(InclusiveBetweenValidator<object, int>);

	protected static Fixture Fixture { get; } = new();

	static ModelValidatorTestsBase()
	{
		// TODO: После обновления AutoFixture до версии 5.0.0 это будет ненужно так как там будет встроенная поддержка типа DateOnly.
		Fixture.Customize<DateOnly>(transformation => transformation.FromFactory((DateTime value) => DateOnly.FromDateTime(value)));
	}
}

internal abstract class ModelValidatorTestsBase<TModel, TValidator>
	: ModelValidatorTestsBase
	where TValidator : AbstractValidator<TModel>, new()
{
	[TestMethod("При корректном значении ошибок проверки быть не должно")]
	public void ValidateWithValidValue()
	{
		// Arrange.
		TModel value = CreateValidModel();
		TValidator validator = new();

		// Act.
		TestValidationResult<TModel> result = validator.TestValidate(value);

		// Assert.
		result.ShouldNotHaveAnyValidationErrors();
	}

	public virtual void ValidateWithInvalidValue(
		TModel value,
		Expression<Func<TModel, object>> property,
		string expectedErrorCode)
		=> ValidateWithInvalidValue(
			value,
			(property, expectedErrorCode));

	public virtual void ValidateWithInvalidValue(
		TModel value,
		string propertyName,
		string expectedErrorCode)
		=> ValidateWithInvalidValue(value, (propertyName, expectedErrorCode));

	protected static void ValidateWithInvalidValue(
		TModel value,
		params ReadOnlySpan<(string propertyName, string expectedErrorCode)> properties)
	{
		// Arrange.
		TValidator validator = new();

		// Act.
		TestValidationResult<TModel> result = validator.TestValidate(value);

		// Assert.
		foreach ((string propertyName, string expectedErrorCode) in properties)
		{
			_ = result.ShouldHaveValidationErrorFor(propertyName)
				.WithErrorCode(expectedErrorCode);
		}
	}

	protected static void ValidateWithInvalidValue(
		TModel value,
		params ReadOnlySpan<(Expression<Func<TModel, object>> property, string expectedErrorCode)> properties)
	{
		foreach ((Expression<Func<TModel, object>> property, string expectedErrorCode) in properties)
		{
			ValidateWithInvalidValue(
				value,
				(GetPropertyName<TModel, object>(property), expectedErrorCode));
		}
	}

	protected virtual TModel CreateValidModel()
	{
		return Fixture.Create<TModel>();
	}

	protected static string GetPropertyName<TProperty>(Expression<Func<TModel, TProperty>> property)
		=> GetPropertyName<TModel, TProperty>(property);

	protected static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> property)
	{
		return property.Body.NodeType switch
		{
			ExpressionType.MemberAccess => GetPropertyName(
				property.Body,
				new()),
			ExpressionType.Convert => GetPropertyName(
				((UnaryExpression)property.Body).Operand,
				new()),
			_ => throw new ArgumentException(
				"Неподдерживаемый тип выражения.",
				nameof(property))
		};
	}

	private static string GetPropertyName(
		Expression propertyExpression,
		StringBuilder stringBuilder)
	{
		if (propertyExpression is not MemberExpression memberExpression)
		{
			return string.Empty;
		}

		if (stringBuilder.Length > 0)
		{
			stringBuilder.Insert(0, '.');
		}

		stringBuilder.Insert(0, memberExpression.Member.Name);

		if (memberExpression.Expression?.NodeType == ExpressionType.MemberAccess)
		{
			GetPropertyName(memberExpression.Expression, stringBuilder);
		}

		return stringBuilder.ToString();
	}

	protected static string GetCollectionItemPropertyName<TCollection, TProperty>(
		Expression<Func<TModel, IEnumerable<TCollection>>> collection,
		Expression<Func<TCollection, TProperty>> property,
		int index = 0)
	{
		string
			collectionName = GetPropertyName<TModel, IEnumerable<TCollection>>(collection),
			propertyName = GetPropertyName(property);

		return $"{collectionName}[{index}].{propertyName}";
	}
}
