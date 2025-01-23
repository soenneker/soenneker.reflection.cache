using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Local;
using Soenneker.Tests.Benchmark;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Members;

[Collection("Collection")]
public class MembersRunner : BenchmarkTest
{
    public MembersRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetMember()
    {
        Summary summary = BenchmarkRunner.Run<GetMemberBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [LocalFact]
    public async Task GetMembers()
    {
        Summary summary = BenchmarkRunner.Run<GetMembersBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}
