using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Fields;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class FieldsRunner : BenchmarkTest
{
    public FieldsRunner() : base()
    {
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetField()
    {
        Summary summary = BenchmarkRunner.Run<GetFieldBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetFields()
    {
        Summary summary = BenchmarkRunner.Run<GetFieldsBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}
