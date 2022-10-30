﻿using System.ComponentModel.DataAnnotations;

namespace Application.Data.Models;

/// <summary>
/// Модель для представления данных раздела "Профили".
/// </summary>
/// <param name="Title"><inheritdoc cref="TitleElement" path="/param[@name='Title']"/></param>
/// <param name="Description"><inheritdoc cref="TitleElement" path="/param[@name='Description']"/></param>
/// <param name="Uri">Ссылка на профиль.</param>
/// <param name="FontClass">Используемый для визуализации CSS-класс символа из шрифта "FontAwesome".</param>
public sealed record ProfileItem(
	DataString Title,
	DataString Description,
	[property: Required] Uri Uri,
	[property: Required] string FontClass)
	: TitleElement(Title, Description);