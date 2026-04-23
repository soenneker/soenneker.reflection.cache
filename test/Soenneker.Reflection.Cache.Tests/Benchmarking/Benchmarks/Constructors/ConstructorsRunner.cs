using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Constructors;

public class ConstructorsRunner : BenchmarkTest
{
    public ConstructorsRunner() : base()
    {
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async Task GetConstructor()
    {
        Summary summary = BenchmarkRunner.Run<GetConstructorBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
   // [LocalOnly]
    public async Task CachedConstructors()
    {
        Summary summary = BenchmarkRunner.Run<CachedConstructorBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
   // [LocalOnly]
    public async Task GetConstructors()
    {
        Summary summary = BenchmarkRunner.Run<GetConstructorsBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

     [Skip("Manual")]
    //[LocalOnly]
    public async Task ConstructorInvoke()
    {
        Summary summary = BenchmarkRunner.Run<ConstructorInvokeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
   // [LocalOnly]
    public async Task CreateInstanceParameters()
    {
        Summary summary = BenchmarkRunner.Run<ConstructorInvokeParametersBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }
}



