using System.IO;
using System.Threading;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using Perfolizer.Mathematics.OutlierDetection;
using Xunit;


namespace Soenneker.Reflection.Cache.Tests.Benchmarking;

[Collection("Collection")]
public abstract class BenchmarkTest 
{
    protected ManualConfig DefaultConf { get; }

    private readonly ITestOutputHelper _outputHelper;

    protected static CancellationToken CancellationToken => TestContext.Current.CancellationToken;

    protected BenchmarkTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;

        DefaultConf = ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator);
        DefaultConf.SummaryStyle = SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend);
    }

    protected async System.Threading.Tasks.ValueTask OutputSummaryToLog(Summary summary, CancellationToken cancellationToken = default)
    {
        string[] logs = await File.ReadAllLinesAsync(summary.LogFilePath, cancellationToken);

        foreach (string? log in logs)
        {
            _outputHelper.WriteLine(log);
        }
    }
}