using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Types;

[Collection("Collection")]
public class TypesRunner : BenchmarkTest
{
    public TypesRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetTypeBenchmarks()
    {
        Summary summary = BenchmarkRunner.Run<GetTypeBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary, CancellationToken);
    }

    [LocalFact]
    public async Task GetCachedTypeBenchmarks()
    {
        Summary summary = BenchmarkRunner.Run<GetCachedTypeBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary, CancellationToken);
    }

    [LocalFact]
    public async Task GetGenericTypeDefinition()
    {
        Summary summary = BenchmarkRunner.Run<GetGenericTypeDefinitionBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary, CancellationToken);
    }

    [LocalFact]
    public async Task GetElementType()
    {
        Summary summary = BenchmarkRunner.Run<GetElementTypeBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary, CancellationToken);
    }

    [LocalFact]
    public async Task IsAssignableFrom()
    {
        Summary summary = BenchmarkRunner.Run<IsAssignableFromBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary, CancellationToken);
    }

    [LocalFact]
    public async Task CachedType()
    {
        Summary summary = BenchmarkRunner.Run<CachedTypeBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary, CancellationToken);
    }
}
