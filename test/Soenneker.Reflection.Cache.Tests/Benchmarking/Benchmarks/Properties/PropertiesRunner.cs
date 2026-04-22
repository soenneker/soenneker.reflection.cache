using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Properties;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class PropertiesRunner : BenchmarkTest
{
    public PropertiesRunner() : base()
    {
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetProperty()
    {
        Summary summary = BenchmarkRunner.Run<GetPropertyBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetProperties()
    {
        Summary summary = BenchmarkRunner.Run<GetPropertiesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}