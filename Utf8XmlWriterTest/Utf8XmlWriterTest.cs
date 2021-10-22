using System.IO;
using System.Xml.Linq;
using Tools.Text.Xml.Tests.Examples;
using Xunit;

namespace Tools.Text.Xml.Tests
{
  public class Utf8XmlWriterTest
  {
    private static class XmlNames
    {
      public static readonly XmlEncodedName Person = XmlEncodedName.Encode("Person");
    }

    [Fact]
    public void Single_Element()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person/>", ms.ToUtf8String());
    }

    [Fact]
    public void WriteValue_String_Escape()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteValue("This is &some <person");
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person>This is &amp;some &lt;person</Person>", ms.ToUtf8String());
    }

    [Fact]
    public void WriteValue_Int()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteValue(12);
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person>12</Person>", ms.ToUtf8String());
    }

    [Fact]
    public void WriteValue_Int2()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement("Person");
        subject.WriteValue(12);
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person>12</Person>", ms.ToUtf8String());
    }

    [Fact]
    public void Element_With_Arguments()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteAttributeString("name", "John");
        subject.WriteAttributeString("age", -132);
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person name=\"John\" age=\"-132\"/>", ms.ToUtf8String());
    }

    [Fact]
    public void Element_With_Null_Argument()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteAttributeString("name", null);
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person name=\"\"/>", ms.ToUtf8String());
    }

    [Fact]
    public void Inner_Element_Without_Value()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteStartElement("Details");
        subject.WriteEndElement("Details");
        subject.WriteEndElement(XmlNames.Person);
        Assert.Equal("<Person><Details/></Person>", ms.ToUtf8String());
      }
    }

    [Fact]
    public void Multiple_InnerElements()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteStartElement("Details");
        subject.WriteEndElement("Details");
        subject.WriteStartElement("Details");
        subject.WriteEndElement("Details");
        subject.WriteEndElement(XmlNames.Person);
        Assert.Equal("<Person><Details/><Details/></Person>", ms.ToUtf8String());
      }
    }

    [Fact]
    public void Creates_InnerElement_Attributes()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteStartElement("Details");
        subject.WriteStartAttribute("name");
        subject.WriteValue("Jon");
        subject.WriteEndAttribute();
        subject.WriteEndElement("Details");
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person><Details name=\"Jon\"/></Person>", ms.ToUtf8String());
    }

    [Fact]
    public void WriteAttributeString_Prefix_Namespace()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteAttributeString("pref", "name", "namespace", "Jon");
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person pref:name=\"Jon\" xmlns:pref=\"namespace\"/>", ms.ToUtf8String());
    }

    [Fact]
    public void WriteAttributeString_Prefix_Empty_Namespace()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteAttributeString("pref", "name", "", "Jon");
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person pref:name=\"Jon\"/>", ms.ToUtf8String());
    }

    [Fact]
    public void Inner_InnerElement()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteStartElement("Details");
        subject.WriteAttributeString("weight", "76");
        subject.WriteStartElement("Extra");
        subject.WriteEndElement("Extra");
        subject.WriteEndElement("Details");
        subject.WriteEndElement(XmlNames.Person);
        Assert.Equal("<Person><Details weight=\"76\"><Extra/></Details></Person>", ms.ToUtf8String());
      }
    }

    [Fact]
    public void WriteElementString()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteElementString(XmlNames.Person, "Somebody");
        Assert.Equal("<Person>Somebody</Person>", ms.ToUtf8String());
      }
    }

    [Fact]
    public void WriteElement_Int()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteElementString(XmlNames.Person, 123);
      }
      Assert.Equal("<Person>123</Person>", ms.ToUtf8String());
    }

    [Fact]
    public void WriteElement_Double()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteElementString(XmlNames.Person, 123.45);
      }
      Assert.Equal("<Person>123.45</Person>", ms.ToUtf8String());
    }

    [Fact]
    public void WriteElementString_Null_Value()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteElementString(XmlNames.Person, null);
      }
      Assert.Equal("<Person/>", ms.ToUtf8String());
    }

    [Fact]
    public void Inner_WriteElementString()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement("Persons");
        subject.WriteElementString(XmlNames.Person, "Somebody");
        subject.WriteEndElement("Persons");
      }
      Assert.Equal("<Persons><Person>Somebody</Person></Persons>", ms.ToUtf8String());
    }

    //[Fact]
    //public void WriteElementString_Prefix()
    //{
    //  var ms = new MemoryStream();
    //  using (var subject = new Utf8XmlWriter(ms))
    //  {
    //    subject.WriteElementString("soap", XmlNames.Person, null, "Somebody");
    //  }
    //  Assert.Equal("<soap:Person>Somebody</soap:Person>", ms.ToUtf8String());
    //}

    //[Fact]
    //public void WriteElementString_Prefix_Namespace()
    //{
    //  var ms = new MemoryStream();
    //  using (var subject = new Utf8XmlWriter(ms))
    //  {
    //    subject.WriteElementString("soap", XmlNames.Person, "some_namespace", "Somebody");
    //  }
    //  Assert.Equal("<soap:Person xmlns:soap=\"some_namespace\">Somebody</soap:Person>", ms.ToUtf8String());
    //}

    [Fact]
    public void Write_Raw_Value()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteRaw("34");
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person>34</Person>", ms.ToUtf8String());
    }

    [Fact]
    public void Write_Value_Then_Write_Raw()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        subject.WriteStartElement(XmlNames.Person);
        subject.WriteValue("&first");
        subject.WriteRaw("&second");
        subject.WriteEndElement(XmlNames.Person);
      }
      Assert.Equal("<Person>&amp;first&second</Person>", ms.ToUtf8String());
    }

    [Fact]
    public void Writes_Supplier_Request()
    {
      var ms = new MemoryStream();
      using (var subject = new Utf8XmlWriter(ms))
      {
        Simple_XML_Writer.Write(subject);
      }
      var actual = ms.ToUtf8String();
      var expected = File.ReadAllText(Path.Combine("..", "..", "..", "Examples", "Simple_XML_Writer.xml"));

      Assert.Equal(XDocument.Parse(expected).ToString(), XDocument.Parse(actual).ToString());
    }

    //[Fact]
    //public void Writes_Supplier_OTA_Standard_Request()
    //{
    //  var ms = new MemoryStream();
    //  using (var subject = new Utf8XmlWriter(ms))
    //  {
    //    OTA_Standard_XML_Writer_Utf8XmlWriter.Write(subject);
    //  }
    //  var actual = ms.ToUtf8String();
    //  var expected = File.ReadAllText(Path.Combine("..", "..", "..", "Examples", "OTA_Standard_XML_Writer.xml"));

    //  Assert.Equal(XDocument.Parse(expected).ToString(), XDocument.Parse(actual).ToString());
    //}

    //[Fact]
    //public void XmlWriter_Writes_Supplier_OTA_Standard_Request()
    //{
    //  var ms = new MemoryStream();
    //  using (var subject = System.Xml.XmlWriter.Create(ms))
    //  {
    //    OTA_Standard_XML_Writer_XmlWriter.Write(subject);
    //  }
    //  var actual = ms.ToUtf8String();
    //  var expected = File.ReadAllText(Path.Combine("..", "..", "..", "Examples", "OTA_Standard_XML_Writer.xml"));

    //  Assert.Equal(XDocument.Parse(expected).ToString(), XDocument.Parse(actual).ToString());
    //}
  }
}
