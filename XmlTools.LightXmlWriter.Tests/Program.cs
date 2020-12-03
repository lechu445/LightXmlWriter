using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

namespace XmlTools.Test
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      var config = ManualConfig
        .Create(DefaultConfig.Instance)
        .AddDiagnoser(MemoryDiagnoser.Default);

      BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
    }
  }
}