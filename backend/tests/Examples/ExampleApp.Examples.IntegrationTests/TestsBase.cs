using Xunit;

namespace ExampleApp.Examples.IntegrationTests;

public abstract class TestsBase<TApp> : IAsyncLifetime, IDisposable
    where TApp : ExampleAppTestApp, new()
{
    protected TApp App { get; private set; }

    public TestsBase()
    {
        App = new TApp();
    }

    ValueTask IAsyncLifetime.InitializeAsync() => App.InitializeAsync();

    ValueTask IAsyncDisposable.DisposeAsync() => App.DisposeAsync();

    void IDisposable.Dispose() => App.Dispose();
}
