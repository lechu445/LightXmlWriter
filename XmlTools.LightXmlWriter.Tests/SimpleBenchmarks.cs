using BenchmarkDotNet.Attributes;
using System;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;

namespace XmlTools.Benchmarks
{
  public class SimpleBenchmarks
  {
    private LightXmlWriter writer;
    private XmlWriter xmlWriter;

    [Params(0, 1, 2, 3, 4, 5)]
    public int MethodIndex { get; set; }

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
      methods1[MethodIndex].Invoke(this.writer);
    }

    [Benchmark(Baseline = true)]
    public void XmlWriter_Write_Xml()
    {
      methods2[MethodIndex].Invoke(this.xmlWriter);
    }

    private static readonly Action<LightXmlWriter>[] methods1 = new[]
    {
      new Action<LightXmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteEndElement("Person"); }),
      new Action<LightXmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteStartElement("Name"); w.WriteEndElement("Name"); w.WriteEndElement("Person"); }),
      new Action<LightXmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteAttributeString("Age", "22"); w.WriteEndElement("Person"); }),
      new Action<LightXmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteElementString("Age", "22"); w.WriteEndElement("Person"); }),
      new Action<LightXmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteValue("this is some text <that> should be escaped"); w.WriteEndElement("Person"); }),
      new Action<LightXmlWriter>((w) => { w.WriteStartElement("soapenv", "Person", "http://ns.com"); w.WriteEndElement("soapenv:Person"); }),
    };

    private static readonly Action<XmlWriter>[] methods2 = new[]
    {
      new Action<XmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteEndElement(); }),
      new Action<XmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteStartElement("Name"); w.WriteEndElement(); w.WriteEndElement(); }),
      new Action<XmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteAttributeString("Age", "22"); w.WriteEndElement(); }),
      new Action<XmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteElementString("Age", "22"); w.WriteEndElement(); }),
      new Action<XmlWriter>((w) => { w.WriteStartElement("Person"); w.WriteValue("this is some text <that> should be escaped"); w.WriteEndElement(); }),
      new Action<XmlWriter>((w) => { w.WriteStartElement("soapenv", "Person", "http://ns.com"); w.WriteEndElement(); }),
    };

    [Fact]
    public void Should_Be_The_Same_Output()
    {
      for (int i = 0; i < methods1.Length; i++)
      {
        var sb1 = new StringBuilder();
        var sb2 = new StringBuilder();
        var tw1 = new StringWriter(sb1);
        var tw2 = new StringWriter(sb2);
        var lighWriter = new LightXmlWriter(tw1);
        var xmlWriter = XmlWriter.Create(tw2, new XmlWriterSettings { OmitXmlDeclaration = true });
        methods1[i].Invoke(lighWriter);
        methods2[i].Invoke(xmlWriter);
        lighWriter.Dispose();
        xmlWriter.Dispose();
        var expected = new XmlDocument();
        var actual = new XmlDocument();
        expected.LoadXml(sb2.ToString());
        actual.LoadXml(sb1.ToString());

        Assert.Equal(expected.OuterXml, actual.OuterXml);
      }
    }
  }
}
