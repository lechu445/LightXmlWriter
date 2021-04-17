using System.IO;
using System.Xml;
using BenchmarkDotNet.Attributes;
using XmlTools.Test.Examples;

namespace XmlTools.Benchmarks
{
  [MemoryDiagnoser]
  public class LightXmlWriterBenchmarks
  {
    private LightXmlWriter writer;
    private XmlWriter xmlWriter;

    [IterationSetup(Target = nameof(LightXmlWriter_Write_Xml))]
    public void LightXmlWriter_Before_Each_Test()
    {
      this.writer = new LightXmlWriter(new StreamWriter(new MemoryStream()));
    }

    [IterationSetup(Target = nameof(XmlWriter_Write_Xml))]
    public void Before_Each_Test()
    {
      this.xmlWriter = XmlWriter.Create(new StreamWriter(new MemoryStream()));
    }

    [Benchmark]
    public void LightXmlWriter_Write_Xml()
    {
      BuchbinderBookWriter.Write(this.writer);
    }

    [Benchmark]
    public void XmlWriter_Write_Xml()
    {
      BuchbinderBookWriter.Write(this.xmlWriter);
    }
  }

  [MemoryDiagnoser]
  public class EnterpriseLightXmlWriterBenchmarks
  {
    private LightXmlWriter writer;
    private XmlWriter xmlWriter;

    [IterationSetup(Target = nameof(LightXmlWriter_Write_Xml))]
    public void LightXmlWriter_Before_Each_Test()
    {
      this.writer = new LightXmlWriter(new StreamWriter(new MemoryStream(9000)));
    }

    [IterationSetup(Target = nameof(XmlWriter_Write_Xml))]
    public void Before_Each_Test()
    {
      this.xmlWriter = XmlWriter.Create(new StreamWriter(new MemoryStream(9000)));
    }

    [Benchmark]
    public void LightXmlWriter_Write_Xml()
    {
      EnterpriseBookWriter.Write(this.writer);
    }

    [Benchmark]
    public void XmlWriter_Write_Xml()
    {
      EnterpriseBookXmlWriter.Write(this.xmlWriter);
    }
  }
}
