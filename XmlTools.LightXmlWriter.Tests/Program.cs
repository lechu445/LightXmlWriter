using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using XmlTools.Benchmarks;

namespace XmlTools.Test
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      var config = ManualConfig
        .Create(DefaultConfig.Instance)
        .With(MemoryDiagnoser.Default);

      var summary = BenchmarkRunner.Run<SimpleBenchmarks>(config);
    }
  }
}