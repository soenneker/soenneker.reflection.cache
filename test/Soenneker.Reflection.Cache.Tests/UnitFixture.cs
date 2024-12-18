using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.XUnit.Injectable;
using Serilog.Sinks.XUnit.Injectable.Abstract;
using Serilog.Sinks.XUnit.Injectable.Extensions;
using Xunit;

namespace Soenneker.Reflection.Cache.Tests;

/// <summary>
/// A base xUnit fixture providing injectable log output and DI mechanisms like IServiceCollection and ServiceProvider
/// </summary>
public abstract class UnitFixture : IAsyncLifetime
{
    public ServiceProvider? ServiceProvider { get; set; }

    protected IServiceCollection Services { get; set; }

    public UnitFixture()
    {
        // this needs to remain in constructor because of derivations
        Services = new ServiceCollection();

        var injectableTestOutputSink = new InjectableTestOutputSink();

        Services.AddSingleton<IInjectableTestOutputSink>(injectableTestOutputSink);

        ILogger serilogLogger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.InjectableTestOutput(injectableTestOutputSink)
            .Enrich.FromLogContext()
            .CreateLogger();

        Log.Logger = serilogLogger;
    }

    public virtual ValueTask InitializeAsync()
    {
        ServiceProvider = Services.BuildServiceProvider();

        return ValueTask.CompletedTask;
    }
    
    public virtual async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (ServiceProvider != null)
            await ServiceProvider.DisposeAsync().ConfigureAwait(false);
    }
}