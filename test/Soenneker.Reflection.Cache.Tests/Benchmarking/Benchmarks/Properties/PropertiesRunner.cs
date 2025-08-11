using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;
using Soenneker.Facts.Local;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Properties;

[Collection("Collection")]
public class PropertiesRunner : BenchmarkTest
{
    public PropertiesRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [ManualFact]
    //[LocalFact]
    public async Task GetProperty()
    {
        Summary summary = BenchmarkRunner.Run<GetPropertyBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //[LocalFact]
    public async Task GetProperties()
    {
        Summary summary = BenchmarkRunner.Run<GetPropertiesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}