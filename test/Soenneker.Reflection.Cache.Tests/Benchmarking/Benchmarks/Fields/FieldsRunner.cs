using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Fields;

[Collection("Collection")]
public class FieldsRunner : BenchmarkTest
{
    public FieldsRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetField()
    {
        Summary summary = BenchmarkRunner.Run<GetFieldBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task GetFields()
    {
        Summary summary = BenchmarkRunner.Run<GetFieldsBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }
}
