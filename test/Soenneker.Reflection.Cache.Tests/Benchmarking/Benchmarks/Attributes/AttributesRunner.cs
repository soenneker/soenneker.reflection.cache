using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Attributes;

public class AttributesRunner : BenchmarkTest
{
    [Skip("Manual")]
    //[LocalOnly]
    public async Task GetAttributes()
    {
        Summary summary = BenchmarkRunner.Run<GetAttributesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async Task CachedAttributesExtension()
    {
        Summary summary = BenchmarkRunner.Run<CachedAttributesExtensionBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }
}
