using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Parameters;

public class ParametersRunner : BenchmarkTest
{
    public ParametersRunner() : base()
    {
    }

    [Skip("Manual")]
    //[LocalOnly]
    public async Task GetParameters()
    {
        Summary summary = BenchmarkRunner.Run<GetParametersBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }
}



