using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Local;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

[Collection("Collection")]
public class TypesRunner : BenchmarkTest
{
    public TypesRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [ManualFact]
    //   [LocalFact]
    public async Task GetTypeBenchmarks()
    {
        Summary summary = BenchmarkRunner.Run<GetTypeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //   [LocalFact]
    public async Task GetCachedTypeBenchmarks()
    {
        Summary summary = BenchmarkRunner.Run<GetCachedTypeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //  [LocalFact]
    public async Task GetGenericTypeDefinition()
    {
        Summary summary = BenchmarkRunner.Run<GetGenericTypeDefinitionBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //  [LocalFact]
    public async Task GetElementType()
    {
        Summary summary = BenchmarkRunner.Run<GetElementTypeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //  [LocalFact]
    public async Task IsAssignableFrom()
    {
        Summary summary = BenchmarkRunner.Run<IsAssignableFromBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    // [ManualFact]
    [LocalFact]
    public async Task CachedType()
    {
        Summary summary = BenchmarkRunner.Run<CachedTypeBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}