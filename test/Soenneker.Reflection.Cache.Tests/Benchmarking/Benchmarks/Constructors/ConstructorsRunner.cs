using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Local;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;
using Soenneker.Facts.Manual;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

[Collection("Collection")]
public class ConstructorsRunner : BenchmarkTest
{
    public ConstructorsRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [ManualFact]
    //[LocalFact]
    public async Task GetConstructor()
    {
        Summary summary = BenchmarkRunner.Run<GetConstructorBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
   // [LocalFact]
    public async Task CachedConstructors()
    {
        Summary summary = BenchmarkRunner.Run<CachedConstructorBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
   // [LocalFact]
    public async Task GetConstructors()
    {
        Summary summary = BenchmarkRunner.Run<GetConstructorsBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

     [ManualFact]
    //[LocalFact]
    public async Task ConstructorInvoke()
    {
        Summary summary = BenchmarkRunner.Run<ConstructorInvokeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
   // [LocalFact]
    public async Task CreateInstanceParameters()
    {
        Summary summary = BenchmarkRunner.Run<ConstructorInvokeParametersBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}