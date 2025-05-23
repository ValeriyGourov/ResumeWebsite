#pragma warning disable CA1515

using Application.Infrastructure.Validation;

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Модель для представления данных раздела "Клиенты".
/// </summary>
/// <param name="Name">Название клиента.</param>
/// <param name="Uri">Адрес веб-сайта клиента.</param>
/// <param name="Logo">Адрес картинки логотипа клиента.</param>
public sealed record class ClientItem(DataString Name, Uri Uri, Uri Logo);

internal sealed class ClientItemValidator : AbstractValidator<ClientItem>
{
	public ClientItemValidator()
	{
		this.SetDataStringRule(item => item.Name);

		RuleFor(item => item.Uri)
			.NotNull();

		RuleFor(item => item.Logo)
			.NotNull();
	}
}
