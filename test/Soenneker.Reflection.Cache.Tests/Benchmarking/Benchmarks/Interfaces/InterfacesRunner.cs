using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Interfaces;

[Collection("Collection")]
public class InterfacesRunner : BenchmarkTest
{
    public InterfacesRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetInterface()
    {
        Summary summary = BenchmarkRunner.Run<GetInterfaceBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary, CancellationToken);
    }

    [LocalFact]
    public async Task GetInterfaces()
    {
        Summary summary = BenchmarkRunner.Run<GetInterfacesBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary, CancellationToken);
    }
}