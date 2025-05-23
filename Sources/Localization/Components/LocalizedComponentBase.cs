﻿using Localization.Infrastructure;

using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.Threading;

namespace Localization.Components;

/// <summary>
/// Базовый компонент с поддержкой переключения культуры приложения на лету. Все компоненты,
/// требующие такой локализации, должны наследоваться от данного класса.
/// </summary>
public abstract class LocalizedComponentBase : ComponentBase, IDisposable
{
	private bool _disposedValue;

	/// <summary>
	/// Механизм для асинхронного выполнения обработчиков событий.
	/// </summary>
	private readonly JoinableTaskFactory _joinableTaskFactory = new(new JoinableTaskContext());

	/// <summary>
	/// Преобразователь культуры, предоставляющий сведения о используемой культуре приложения.
	/// </summary>
	[Inject] protected CultureChanger CultureChanger { get; set; } = null!;

	/// <inheritdoc/>
	protected override void OnInitialized()
	{
		base.OnInitialized();

		CultureChanger.CultureChanged += CultureChanged;
	}

	/// <summary>
	/// Обработчик события изменения выбранной культуры приложения. Уведомляет компонент о том,
	/// что его состояние изменилось с цель использовать ресурсы локализации для выбранной
	/// культуры.
	/// </summary>
	private void CultureChanged(object? sender, EventArgs e)
	{
		_ = _joinableTaskFactory.RunAsync(() => InvokeAsync(() => StateHasChanged()));
	}

	/// <inheritdoc cref="Dispose()"/>
	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				// Здесь необходимо освободить управляемое состояние (управляемые объекты).

				CultureChanger.CultureChanged -= CultureChanged;
			}

			// Здесь необходимо освободить неуправляемые ресурсы (неуправляемые объекты) и
			// переопределить метод завершения.
			// Здесь необходимо установить значение NULL для больших полей.
			_disposedValue = true;
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		// Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
