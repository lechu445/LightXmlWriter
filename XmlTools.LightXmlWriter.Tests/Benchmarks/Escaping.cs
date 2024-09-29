using System.IO;
using BenchmarkDotNet.Attributes;

namespace XmlTools.Benchmarks
{
  [MemoryDiagnoser]
  public class Escaping
  {
    private LightXmlWriter writer;

    [IterationSetup(Target = nameof(LightXmlWriter_Write_Xml))]
    public void LightXmlWriter_Before_Each_Test()
    {
      this.writer = new LightXmlWriter(new StreamWriter(Stream.Null));
    }

    [Benchmark]
    public void LightXmlWriter_Write_Xml()
    {
      for (int i = 0; i < 10_000_000; i++)
      {

        this.writer.WriteValue("<something&amp;/>", escape: true);
      }
    }
  }
}
