using System.IO;
using System.Xml;
using BenchmarkDotNet.Attributes;

namespace XmlTools.Benchmarks
{
  [MemoryDiagnoser]
  public class Creating_Writer
  {
    [Benchmark]
    public LightXmlWriter LightXmlWriter_StreamWriter()
    {
      return new LightXmlWriter(StreamWriter.Null);
    }

    [Benchmark(Baseline = true)]
    public XmlWriter XmlWriter_StreamWriter()
    {
      return XmlWriter.Create(StreamWriter.Null);
    }

    [Benchmark]
    public XmlWriter XmlWriter_Stream()
    {
      return XmlWriter.Create(Stream.Null);
    }
  }
}
