using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Properties;

public class PropertiesRunner : BenchmarkTest
{
    public PropertiesRunner() : base()
    {
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async Task GetProperty()
    {
        Summary summary = BenchmarkRunner.Run<GetPropertyBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async Task GetProperties()
    {
        Summary summary = BenchmarkRunner.Run<GetPropertiesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }
}
