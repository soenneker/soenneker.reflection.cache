using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;
using Xunit.Abstractions;

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

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task GetMembers()
    {
        Summary summary = BenchmarkRunner.Run<GetMembersBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }
}
