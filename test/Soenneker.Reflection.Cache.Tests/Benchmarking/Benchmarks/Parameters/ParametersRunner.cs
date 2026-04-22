using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Parameters;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class ParametersRunner : BenchmarkTest
{
    public ParametersRunner() : base()
    {
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetParameters()
    {
        Summary summary = BenchmarkRunner.Run<GetParametersBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}