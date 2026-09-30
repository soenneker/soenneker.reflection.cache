using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Fields;

public class FieldsRunner : BenchmarkTest
{
    public FieldsRunner() : base()
    {
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async ValueTask GetField()
    {
        Summary summary = BenchmarkRunner.Run<GetFieldBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async ValueTask GetFields()
    {
        Summary summary = BenchmarkRunner.Run<GetFieldsBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async ValueTask FieldAccessors()
    {
        Summary summary = BenchmarkRunner.Run<FieldAccessorBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }
}



