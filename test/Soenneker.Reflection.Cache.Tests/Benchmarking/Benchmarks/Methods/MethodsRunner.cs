using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Local;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;
using Soenneker.Facts.Manual;
using Xunit;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Methods;

[Collection("Collection")]
public class MethodsRunner : BenchmarkTest
{
    public MethodsRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [ManualFact]
    //[LocalFact]
    public async Task GetMethod()
    {
        Summary summary = BenchmarkRunner.Run<GetMethodBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
   // [LocalFact]
    public async Task GetMethods()
    {
        Summary summary = BenchmarkRunner.Run<GetMethodsBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}