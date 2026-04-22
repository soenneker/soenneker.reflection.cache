using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Interfaces;

public class InterfacesRunner : BenchmarkTest
{
    public InterfacesRunner() : base()
    {
    }
    
    [Skip("Manual")]
    //[LocalOnly]
    public async Task GetInterface()
    {
        Summary summary = BenchmarkRunner.Run<GetInterfaceBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
   // [LocalOnly]
    public async Task GetInterfaces()
    {
        Summary summary = BenchmarkRunner.Run<GetInterfacesBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }
}
