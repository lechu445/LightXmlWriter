using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using BenchmarkDotNet.Attributes;
using XmlTools.Test.Examples;

namespace XmlTools.Tests.Benchmarks
{
  [MemoryDiagnoser]
  public class OTA_Standard_XML
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

    [IterationSetup(Target = nameof(XmlWriter_From_Stream_Write_Xml))]
    public void Before_Each_Test2()
    {
      this.xmlWriter = XmlWriter.Create(new MemoryStream(9000));
    }

    [Benchmark]
    public void LightXmlWriter_Write_Xml()
    {
      OTA_Standard_XML_Writer_LightXmlWriter.Write(this.writer);
    }

    [Benchmark]
    public void XmlWriter_Write_Xml()
    {
      OTA_Standard_XML_Writer_XmlWriter.Write(this.xmlWriter);
    }

    [Benchmark]
    public void XmlWriter_From_Stream_Write_Xml()
    {
      OTA_Standard_XML_Writer_XmlWriter.Write(this.xmlWriter);
    }
  }
}
