using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Methods;

[Collection("Collection")]
public class MethodsRunner : BenchmarkTest
{
    public MethodsRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetMethod()
    {
        Summary summary = BenchmarkRunner.Run<GetMethodBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task GetMethods()
    {
        Summary summary = BenchmarkRunner.Run<GetMethodsBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }
}