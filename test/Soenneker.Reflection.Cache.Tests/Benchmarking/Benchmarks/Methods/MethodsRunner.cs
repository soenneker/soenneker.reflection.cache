using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;
using Soenneker.Facts.Manual;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Methods;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class MethodsRunner : BenchmarkTest
{
    public MethodsRunner() : base()
    {
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetMethod()
    {
        Summary summary = BenchmarkRunner.Run<GetMethodBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
   // [LocalOnly]
    public async Task GetMethods()
    {
        Summary summary = BenchmarkRunner.Run<GetMethodsBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}