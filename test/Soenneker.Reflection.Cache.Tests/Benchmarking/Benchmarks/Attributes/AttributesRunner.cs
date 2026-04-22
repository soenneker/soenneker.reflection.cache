using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Attributes;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class AttributesRunner : BenchmarkTest
{
    public AttributesRunner() : base()
    {
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetAttributes()
    {
        Summary summary = BenchmarkRunner.Run<GetAttributesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //[LocalOnly]
    public async Task CachedAttributesExtension()
    {
        Summary summary = BenchmarkRunner.Run<CachedAttributesExtensionBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}