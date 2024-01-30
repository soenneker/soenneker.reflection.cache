﻿using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Facts.Local;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Reflection.Cache.Tests.Benchmarking.Benchmarks.Properties;

[Collection("Collection")]
public class ParametersRunner : BenchmarkTest
{
    public ParametersRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [LocalFact]
    public async Task GetProperty()
    {
        Summary summary = BenchmarkRunner.Run<GetPropertyBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }

    [LocalFact]
    public async Task GetProperties()
    {
        Summary summary = BenchmarkRunner.Run<GetPropertiesBenchmarks>(DefaultConf);

        await OutputSummaryToLog(summary);
    }
}