using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Facts.Manual;
using Soenneker.Tests.Benchmark;
using System.Threading.Tasks;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Members;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class MembersRunner : BenchmarkTest
{
    public MembersRunner() : base()
    {
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetMember()
    {
        Summary summary = BenchmarkRunner.Run<GetMemberBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }

    [ManualFact]
    //[LocalOnly]
    public async Task GetMembers()
    {
        Summary summary = BenchmarkRunner.Run<GetMembersBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}
