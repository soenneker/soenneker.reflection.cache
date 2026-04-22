using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Methods;

public class MethodsRunner : BenchmarkTest
{
    public MethodsRunner() : base()
    {
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async Task GetMethod()
    {
        Summary summary = BenchmarkRunner.Run<GetMethodBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
   // [LocalOnly]
    public async Task GetMethods()
    {
        Summary summary = BenchmarkRunner.Run<GetMethodsBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }
}
