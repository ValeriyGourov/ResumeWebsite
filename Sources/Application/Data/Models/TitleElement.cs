#pragma warning disable CA1515
#pragma warning disable CS8620 // TODO: Удалить после исправления ошибки анализатора: https://github.com/dotnet/roslyn/issues/80024

using Application.Infrastructure.Validation;

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Элемент данных, содержащий название и описание.
/// </summary>
/// <param name="Title">Название.</param>
/// <param name="Description">Детальное описание.</param>
public record TitleElement(DataString Title, DataString Description);

internal sealed class TitleElementValidator : AbstractValidator<TitleElement>
{
	public TitleElementValidator()
	{
		DataStringValidator dataStringValidator = new();

		this.SetDataStringRule(
			dataStringValidator,
			item => item.Title,
			item => item.Description);
	}
}
