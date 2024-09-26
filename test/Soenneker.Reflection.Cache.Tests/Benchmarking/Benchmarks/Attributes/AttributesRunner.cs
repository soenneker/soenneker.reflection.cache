using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;
using Xunit.Abstractions;

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

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task CachedAttributesExtension()
    {
        Summary summary = BenchmarkRunner.Run<CachedAttributesExtensionBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }
}
