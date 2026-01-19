using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
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
