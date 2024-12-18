using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Parameters;

[Collection("Collection")]
public class ParametersRunner : BenchmarkTest
{
    public ParametersRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetParameters()
    {
        Summary summary = BenchmarkRunner.Run<GetParametersBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }
}