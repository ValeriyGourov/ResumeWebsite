using System.Linq.Expressions;

using Application.Data.Models;

using FluentValidation;

namespace Application.Infrastructure.Validation;

/// <summary>
/// Предоставляет методы расширения для настройки правил проверки свойств
/// <see cref="DataString"/> с помощью FluentValidation.
/// </summary>
/// <remarks>
/// Этот класс содержит вспомогательные методы, упрощающие процесс установки правил
/// валидации для свойств типа <see cref="DataString"/> в FluentValidation
/// <see cref="AbstractValidator{T}"/>.
/// </remarks>
internal static class ValidationTools
{
	/// <summary>
	/// Настраивает правило валидации для свойства <see cref="DataString"/> на
	/// указанном валидаторе.
	/// </summary>
	/// <remarks>
	/// Этот метод проверяет, что указанное свойство <see cref="DataString"/> не является
	/// <see langword="null"/>, и применяет предоставленный или используемый по умолчанию
	/// <see cref="DataStringValidator"/> для дополнительной проверки.
	/// </remarks>
	/// <typeparam name="T">Тип проверяемого объекта.</typeparam>
	/// <param name="validator">
	/// Валидатор, в который будет добавлено правило. Не может быть <see langword="null"/>.
	/// </param>
	/// <param name="expression">
	/// Выражение, определяющее свойство <see cref="DataString"/> для проверки.
	/// Свойство не должно быть <see langword="null"/>.
	/// </param>
	/// <param name="dataStringValidator">
	/// Необязательный пользовательский <see cref="DataStringValidator"/> для использования
	/// при проверке.  Если он не указан, будет использоваться
	/// <see cref="DataStringValidator"/> по умолчанию.
	/// </param>
	public static void SetDataStringRule<T>(
		this AbstractValidator<T> validator,
		Expression<Func<T, DataString>> expression,
		DataStringValidator? dataStringValidator = null)
	{
		_ = validator.RuleFor(expression)
			.NotNull()
			.SetValidator(dataStringValidator ?? new());
	}

	/// <summary>
	/// Настраивает правила проверки для одного или нескольких свойств
	/// <see cref="DataString"/> указанного типа.
	/// </summary>
	/// <remarks>
	/// Этот метод позволяет определить правила проверки для нескольких свойств
	/// <see cref="DataString"/> за один вызов. Правила применяются к каждому свойству,
	/// указанному в <paramref name="expressions"/>.
	/// </remarks>
	/// <typeparam name="T">
	/// <inheritdoc
	///		cref="SetDataStringRule{T}(AbstractValidator{T}, Expression{Func{T, DataString}}, DataStringValidator?)"
	///		path="/typeparam[@name='T']"/>
	/// </typeparam>
	/// <param name="validator">
	/// <inheritdoc
	///		cref="SetDataStringRule{T}(AbstractValidator{T}, Expression{Func{T, DataString}}, DataStringValidator?)"
	///		path="/param[@name='validator']"/>
	/// </param>
	/// <param name="dataStringValidator">
	/// <inheritdoc
	///		cref="SetDataStringRule{T}(AbstractValidator{T}, Expression{Func{T, DataString}}, DataStringValidator?)"
	///		path="/param[@name='dataStringValidator']"/>
	/// </param>
	/// <param name="expressions">
	/// Одно или несколько выражений, определяющих свойства <see cref="DataString"/> из
	/// <typeparamref name="T"/>, к которым будут применены правила проверки. Каждое
	/// выражение должно указывать на свойство типа <see cref="DataString"/>.
	/// </param>
	public static void SetDataStringRule<T>(
		this AbstractValidator<T> validator,
		DataStringValidator? dataStringValidator = null,
		params ReadOnlySpan<Expression<Func<T, DataString>>> expressions)
	{
		foreach (Expression<Func<T, DataString>> expression in expressions)
		{
			SetDataStringRule(validator, expression, dataStringValidator);
		}
	}
}
