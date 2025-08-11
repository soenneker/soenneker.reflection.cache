using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;
using Soenneker.Facts.Local;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Interfaces;

[Collection("Collection")]
public class InterfacesRunner : BenchmarkTest
{
    public InterfacesRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

  //  [ManualFact]
    [LocalFact]
    public async Task GetInterface()
    {
        Summary summary = BenchmarkRunner.Run<GetInterfaceBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

   // [ManualFact]
    [LocalFact]
    public async Task GetInterfaces()
    {
        Summary summary = BenchmarkRunner.Run<GetInterfacesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}