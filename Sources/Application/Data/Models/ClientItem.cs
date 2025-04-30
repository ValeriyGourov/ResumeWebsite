#pragma warning disable CA1515

using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

namespace Application.Data.Models;

/// <summary>
/// Модель для представления данных раздела "Клиенты".
/// </summary>
/// <param name="Name">Название клиента.</param>
/// <param name="Uri">Адрес веб-сайта клиента.</param>
/// <param name="Logo">Адрес картинки логотипа клиента.</param>
public sealed record class ClientItem(
	[property: ValidateComplexType] DataString? Name,
	[property: Required] Uri Uri,
	[property: Required] Uri Logo);
