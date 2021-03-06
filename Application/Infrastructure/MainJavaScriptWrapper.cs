﻿using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Application.Infrastructure
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes.
	/// <summary>
	/// Обёртка для вызова функций JavaScript файла main.js.
	/// </summary>
	internal class MainJavaScriptWrapper
	{
		private readonly IJSRuntime _jsRuntime;

		public MainJavaScriptWrapper(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
		}

		/// <summary>
		/// Показывает главный контейнер приложения и скрывает вращатель.
		/// </summary>
		public ValueTask ShowMainContainer() => _jsRuntime.InvokeVoidAsync("showMainContainer");
	}
#pragma warning restore CA1812 // Avoid uninstantiated internal classes.
}
