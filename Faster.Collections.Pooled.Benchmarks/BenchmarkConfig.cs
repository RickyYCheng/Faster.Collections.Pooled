// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;

internal class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(
            Job.Default
            // .WithRuntime(NativeAotRuntime.Net10_0)
        );
        AddDiagnoser(MemoryDiagnoser.Default);
        AddExporter(HtmlExporter.Default);
        AddExporter(MarkdownExporter.GitHub);
        AddLogger(ConsoleLogger.Default);

        Options |= ConfigOptions.DisableLogFile;
    }
}
