#pragma warning disable VSTHRD200, CA1515

using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Infrastructure.UnitTests;

[TestClass]
public class JavaScriptModuleBaseTests
{
	private const string _importIdentifier = "import";

	[TestMethod("Вызов метода модуля JavaScript с именем, совпадающим с названием метода класса, должен возвращать ожидаемое значение")]
	[TestProperty(TestProperties.MethodName, "InvokeAsync")]
	public async Task InvokeAsync1()
	{
		// Arrange.
		CancellationToken cancellationToken = default;

		Fixture fixture = new();
		string scriptPath = fixture.Create<string>();
		const string identifier = "returnValueMethod";

		int arg1 = fixture.Create<int>();
		string arg2 = fixture.Create<string>();

		int expected = fixture.Create<int>();

#pragma warning disable CA2007
		await using TestJavaScriptModule module = GetMockedModuleForReturnValueMethod(
			scriptPath,
			identifier,
			arg1,
			arg2,
			expected,
			cancellationToken);
#pragma warning restore CA2007

		// Act.
		int actual = await module
			.ReturnValueMethod(arg1, arg2, cancellationToken)
			.ConfigureAwait(false);

		// Assert.
		_ = actual.Should().Be(expected);
	}

	[TestMethod("Вызов метода модуля JavaScript с явно указанным именем должен возвращать ожидаемое значение")]
	[TestProperty(TestProperties.MethodName, "InvokeAsync")]
	public async Task InvokeAsync2()
	{
		// Arrange.
		CancellationToken cancellationToken = default;

		Fixture fixture = new();
		string scriptPath = fixture.Create<string>();
		string identifier = fixture.Create<string>();

		int arg1 = fixture.Create<int>();
		string arg2 = fixture.Create<string>();

		int expected = fixture.Create<int>();

#pragma warning disable CA2007
		await using TestJavaScriptModule module = GetMockedModuleForReturnValueMethod(
			scriptPath,
			identifier,
			arg1,
			arg2,
			expected,
			cancellationToken);
#pragma warning restore CA2007

		// Act.
		int actual = await module
			.ReturnValueMethodWithExplicitIdentifier(
				identifier,
				arg1,
				arg2,
				cancellationToken)
			.ConfigureAwait(false);

		// Assert.
		_ = actual.Should().Be(expected);
	}

	[TestMethod("Вызов метода модуля JavaScript с именем, совпадающим с названием метода класса, должен вызывать метод без возврата значения")]
	[TestProperty(TestProperties.MethodName, "InvokeVoidAsync")]
	public async Task InvokeVoidAsync1()
	{
		// Arrange.
		CancellationToken cancellationToken = default;

		Fixture fixture = new();
		string scriptPath = fixture.Create<string>();
		const string identifier = "voidMethod";

		int arg1 = fixture.Create<int>();
		string arg2 = fixture.Create<string>();

		Mock<IJSObjectReference> jsObjectReferenceMock = GetJSObjectReferenceMockForVoidMethod(
			identifier,
			arg1,
			arg2,
			cancellationToken);

#pragma warning disable CA2007
		await using TestJavaScriptModule module = GetMockedModule(
			jsObjectReferenceMock.Object,
			scriptPath,
			cancellationToken);
#pragma warning restore CA2007

		// Act.
		await module
			.VoidMethod(arg1, arg2, cancellationToken)
			.ConfigureAwait(false);

		// Assert.
		jsObjectReferenceMock.Verify();
	}

	[TestMethod("Вызов метода модуля JavaScript с явно указанным именем должен вызывать метод без возврата значения")]
	[TestProperty(TestProperties.MethodName, "InvokeVoidAsync")]
	public async Task InvokeVoidAsync2()
	{
		// Arrange.
		CancellationToken cancellationToken = default;

		Fixture fixture = new();
		string scriptPath = fixture.Create<string>();
		string identifier = fixture.Create<string>();

		int arg1 = fixture.Create<int>();
		string arg2 = fixture.Create<string>();

		Mock<IJSObjectReference> jsObjectReferenceMock = GetJSObjectReferenceMockForVoidMethod(
			identifier,
			arg1,
			arg2,
			cancellationToken);

#pragma warning disable CA2007
		await using TestJavaScriptModule module = GetMockedModule(
			jsObjectReferenceMock.Object,
			scriptPath,
			cancellationToken);
#pragma warning restore CA2007

		// Act.
		await module
			.VoidMethodWithExplicitIdentifier(identifier, arg1, arg2, cancellationToken)
			.ConfigureAwait(false);

		// Assert.
		jsObjectReferenceMock.Verify();
	}

	[TestMethod("При освобождении ресурсов экземпляра модуля должны освобождаться ресурсы внутренних объектов")]
	[TestProperty(TestProperties.MethodName, nameof(TestJavaScriptModule.DisposeAsync))]
	public async Task DisposeAsync1()
	{
		// Arrange.
		CancellationToken cancellationToken = default;

		Fixture fixture = new();
		string scriptPath = fixture.Create<string>();

		int arg1 = fixture.Create<int>();
		string arg2 = fixture.Create<string>();

		Mock<IJSObjectReference> jsObjectReferenceMock = new();
		jsObjectReferenceMock
			.Setup(jsObjectReference => jsObjectReference.DisposeAsync())
			.Verifiable();

		TestJavaScriptModule module = GetMockedModule(
			jsObjectReferenceMock.Object,
			scriptPath,
			cancellationToken);

		// Act.
		_ = await module
			.ReturnValueMethod(arg1, arg2, cancellationToken)
			.ConfigureAwait(false);
		await module
			.DisposeAsync()
			.ConfigureAwait(false);

		// Assert.
		jsObjectReferenceMock.Verify();
	}

	[TestMethod("При освобождении ресурсов внутренних объектов исключения должны игнорироваться")]
	[TestProperty(TestProperties.MethodName, nameof(TestJavaScriptModule.DisposeAsync))]
	public async Task DisposeAsync2()
	{
		// Arrange.
		CancellationToken cancellationToken = default;

		Fixture fixture = new();
		string scriptPath = fixture.Create<string>();

		int arg1 = fixture.Create<int>();
		string arg2 = fixture.Create<string>();

		Mock<IJSObjectReference> jsObjectReferenceMock = new();
		jsObjectReferenceMock
			.Setup(jsObjectReference => jsObjectReference.DisposeAsync())
			.Throws(() => new JSDisconnectedException("Тестовое сообщение"))
			.Verifiable();

		TestJavaScriptModule module = GetMockedModule(
			jsObjectReferenceMock.Object,
			scriptPath,
			cancellationToken);

		// Act.
		_ = await module
			.ReturnValueMethod(arg1, arg2, cancellationToken)
			.ConfigureAwait(false);
		await module
			.DisposeAsync()
			.ConfigureAwait(false);

		// Assert.
		jsObjectReferenceMock.Verify();
	}

	private static TestJavaScriptModule GetMockedModuleForReturnValueMethod<T>(
		string scriptPath,
		string identifier,
		int arg1,
		string arg2,
		T returnValue,
		CancellationToken cancellationToken)
	{
		Mock<IJSObjectReference> jsObjectReferenceMock = new();
		_ = jsObjectReferenceMock
			.Setup(jsObjectReference => jsObjectReference.InvokeAsync<T>(
				identifier,
				cancellationToken,
				new object[] { arg1, arg2 }))
			.ReturnsAsync(returnValue);

		return GetMockedModule(
			jsObjectReferenceMock.Object,
			scriptPath,
			cancellationToken);
	}

	private static Mock<IJSObjectReference> GetJSObjectReferenceMockForVoidMethod(
		string identifier,
		int arg1,
		string arg2,
		CancellationToken cancellationToken)
	{
		Mock<IJSObjectReference> jsObjectReferenceMock = new();
		jsObjectReferenceMock
			.Setup(jsObjectReference => jsObjectReference.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
				identifier,
				cancellationToken,
				new object[] { arg1, arg2 }))
			.Verifiable();

		return jsObjectReferenceMock;
	}

	private static TestJavaScriptModule GetMockedModule(
		IJSObjectReference jsObjectReference,
		string scriptPath,
		CancellationToken cancellationToken)
	{
		Mock<IJSRuntime> jsRuntimeMock = new();
		_ = jsRuntimeMock
			.Setup(jsRuntime => jsRuntime.InvokeAsync<IJSObjectReference>(
				_importIdentifier,
				cancellationToken,
				new object[] { scriptPath }))
			.ReturnsAsync(jsObjectReference);

		return new(
			Mock.Of<ILogger<TestJavaScriptModule>>(),
			jsRuntimeMock.Object,
			scriptPath);
	}

	/// <summary>
	/// Тестовый класс, унаследованный от <see cref="JavaScriptModuleBase"/>.
	/// </summary>
	/// <inheritdoc/>
	internal sealed class TestJavaScriptModule(
		ILogger<TestJavaScriptModule> logger,
		IJSRuntime jsRuntime,
		string scriptPath)
		: JavaScriptModuleBase(logger, jsRuntime, scriptPath)
	{
		/// <summary>
		/// Метод-обёртка для тестирования <see cref="JavaScriptModuleBase.InvokeVoidAsync(object?[]?, CancellationToken, string)"/>.
		/// </summary>
		/// <param name="arg1">Первый аргумент метода.</param>
		/// <param name="arg2">Второй аргумент метода.</param>
		/// <param name="cancellationToken"><inheritdoc cref="JavaScriptModuleBase.InvokeVoidAsync" path="/param[@name='cancellationToken']"/></param>
		public ValueTask VoidMethod(
			int arg1,
			string arg2,
			CancellationToken cancellationToken = default)
			=> InvokeVoidAsync(
				[arg1, arg2],
				cancellationToken);

		/// <summary>
		/// Метод-обёртка для тестирования <see cref="JavaScriptModuleBase.InvokeVoidAsync(string, object?[]?, CancellationToken)"/>.
		/// </summary>
		/// <param name="identifier"><inheritdoc cref="JavaScriptModuleBase.InvokeVoidAsync" path="/param[@name='identifier']"/></param>
		/// <param name="arg1">Первый аргумент метода.</param>
		/// <param name="arg2">Второй аргумент метода.</param>
		/// <param name="cancellationToken"><inheritdoc cref="JavaScriptModuleBase.InvokeVoidAsync" path="/param[@name='cancellationToken']"/></param>
		public ValueTask VoidMethodWithExplicitIdentifier(
			string identifier,
			int arg1,
			string arg2,
			CancellationToken cancellationToken = default)
			=> InvokeVoidAsync(
				identifier,
				[arg1, arg2],
				cancellationToken);

		/// <summary>
		/// Метод-обёртка для тестирования <see cref="JavaScriptModuleBase.InvokeAsync{TValue}(object?[]?, CancellationToken, string)"/>.
		/// </summary>
		/// <param name="arg1">Первый аргумент метода.</param>
		/// <param name="arg2">Второй аргумент метода.</param>
		/// <param name="cancellationToken"><inheritdoc cref="JavaScriptModuleBase.InvokeVoidAsync" path="/param[@name='cancellationToken']"/></param>
		/// <returns><inheritdoc cref="JavaScriptModuleBase.InvokeAsync" path="/returns"/></returns>
		public ValueTask<int> ReturnValueMethod(
			int arg1,
			string arg2,
			CancellationToken cancellationToken)
			=> InvokeAsync<int>(
				[arg1, arg2],
				cancellationToken);

		/// <summary>
		/// Метод-обёртка для тестирования <see cref="JavaScriptModuleBase.InvokeAsync{TValue}(string, object?[]?, CancellationToken)"/>.
		/// </summary>
		/// <param name="identifier"><inheritdoc cref="JavaScriptModuleBase.InvokeAsync" path="/param[@name='identifier']"/></param>
		/// <param name="arg1">Первый аргумент метода.</param>
		/// <param name="arg2">Второй аргумент метода.</param>
		/// <param name="cancellationToken"><inheritdoc cref="JavaScriptModuleBase.InvokeVoidAsync" path="/param[@name='cancellationToken']"/></param>
		/// <returns><inheritdoc cref="JavaScriptModuleBase.InvokeAsync" path="/returns"/></returns>
		public ValueTask<int> ReturnValueMethodWithExplicitIdentifier(
			string identifier,
			int arg1,
			string arg2,
			CancellationToken cancellationToken)
			=> InvokeAsync<int>(
				identifier,
				[arg1, arg2],
				cancellationToken);
	}
}
