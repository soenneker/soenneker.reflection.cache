using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Attributes.Local;
using Soenneker.Tests.Benchmark;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

public class TypesRunner : BenchmarkTest
{
    public TypesRunner() : base()
    {
    }

    [Skip("Manual")]
    //   [LocalOnly]
    public async Task GetTypeBenchmarks()
    {
        Summary summary = BenchmarkRunner.Run<GetTypeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
    //   [LocalOnly]
    public async Task GetCachedTypeBenchmarks()
    {
        Summary summary = BenchmarkRunner.Run<GetCachedTypeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
    //  [LocalOnly]
    public async Task GetGenericTypeDefinition()
    {
        Summary summary = BenchmarkRunner.Run<GetGenericTypeDefinitionBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
    //  [LocalOnly]
    public async Task GetElementType()
    {
        Summary summary = BenchmarkRunner.Run<GetElementTypeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    [Skip("Manual")]
    //  [LocalOnly]
    public async Task IsAssignableFrom()
    {
        Summary summary = BenchmarkRunner.Run<IsAssignableFromBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }

    // [Skip("Manual")]
    [LocalOnly]
    public async Task CachedType()
    {
        Summary summary = BenchmarkRunner.Run<CachedTypeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog();
    }
}
