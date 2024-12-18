using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

[Collection("Collection")]
public class ConstructorsRunner : BenchmarkTest
{
    public ConstructorsRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetConstructor()
    {
        Summary summary = BenchmarkRunner.Run<GetConstructorBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task CachedConstructors()
    {
        Summary summary = BenchmarkRunner.Run<CachedConstructorBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task GetConstructors()
    {
        Summary summary = BenchmarkRunner.Run<GetConstructorsBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task ConstructorInvoke()
    {
        Summary summary = BenchmarkRunner.Run<ConstructorInvokeBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task CreateInstanceParameters()
    {
        Summary summary = BenchmarkRunner.Run<ConstructorInvokeParametersBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }
}