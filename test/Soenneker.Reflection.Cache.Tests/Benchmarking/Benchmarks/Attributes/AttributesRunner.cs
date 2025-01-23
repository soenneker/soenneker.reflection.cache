using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Local;
using Soenneker.Tests.Benchmark;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Attributes;

[Collection("Collection")]
public class AttributesRunner : BenchmarkTest
{
    public AttributesRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetAttributes()
    {
        Summary summary = BenchmarkRunner.Run<GetAttributesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [LocalFact]
    public async Task CachedAttributesExtension()
    {
        Summary summary = BenchmarkRunner.Run<CachedAttributesExtensionBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}
