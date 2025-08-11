using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Parameters;

[Collection("Collection")]
public class ParametersRunner : BenchmarkTest
{
    public ParametersRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [ManualFact]
    //[LocalFact]
    public async Task GetParameters()
    {
        Summary summary = BenchmarkRunner.Run<GetParametersBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}