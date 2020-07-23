using System;

using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Localization.Infrastructure
{
	/// <summary>
	/// Обёртка для вызова функций JavaScript.
	/// </summary>
	internal class JavaScriptWrapper
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly IStringLocalizer<JavaScriptWrapper> _localizer;

		public JavaScriptWrapper(
			IJSRuntime jsRuntime,
			IStringLocalizer<JavaScriptWrapper> localizer)
		{
			_jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
			_localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
		}

		/// <summary>
		/// Создаёт куки-файл в системе пользователя.
		/// </summary>
		/// <param name="name">Имя куки-файла.</param>
		/// <param name="value">Значение куки-файла.</param>
		/// <param name="expirationDays">Срок жизни куки-файла, исчисляемый в днях.</param>
		public void CreateCookie(string name, string value, int expirationDays)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException(_localizer["Имя строки данных не может быть пустым."], nameof(name));
			}
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException(_localizer["Значение строки данных не может быть пустым."], nameof(value));
			}

			_jsRuntime.InvokeVoidAsync("localization.CreateCookie", name, value, expirationDays);
		}
	}
}
