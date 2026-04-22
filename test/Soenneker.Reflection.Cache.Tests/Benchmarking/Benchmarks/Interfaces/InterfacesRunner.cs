using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Interfaces;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class InterfacesRunner : BenchmarkTest
{
    public InterfacesRunner() : base()
    {
    }
    
    [ManualFact]
    //[LocalOnly]
    public async Task GetInterface()
    {
        Summary summary = BenchmarkRunner.Run<GetInterfaceBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
   // [LocalOnly]
    public async Task GetInterfaces()
    {
        Summary summary = BenchmarkRunner.Run<GetInterfacesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}