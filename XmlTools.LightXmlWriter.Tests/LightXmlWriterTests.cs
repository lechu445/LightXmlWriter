using System.IO;
using System.Text;
using System.Xml.Linq;
using XmlTools.Test.Examples;
using Xunit;

namespace XmlTools.Tests
{
  public class LightXmlWriterTest
  {
    [Fact]
    public void Single_Element()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person/>", sb.ToString());
    }

    [Fact]
    public void WriteValue_String_Escape()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteValue("This is &some <person", escape: true);
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person>This is &amp;some &lt;person</Person>", sb.ToString());
    }

    [Fact]
    public void WriteValue_Int()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteValue(12);
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person>12</Person>", sb.ToString());
    }

    [Fact]
    public void Unescaped_Value()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteValue("This is &some <person", escape: false);
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person>This is &some <person</Person>", sb.ToString());
    }

    [Fact]
    public void Element_With_Arguments()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteAttributeString("name", "John");
        subject.WriteAttributeString("age", -132);
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person name=\"John\" age=\"-132\"/>", sb.ToString());
    }

    [Fact]
    public void Element_With_Null_Argument()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteAttributeString("name", null);
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person name=\"\"/>", sb.ToString());
    }

    [Fact]
    public void Inner_Element_Without_Value()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteStartElement("Details");
        subject.WriteEndElement("Details");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person><Details/></Person>", sb.ToString());
    }

    [Fact]
    public void Multiple_InnerElements()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteStartElement("Details");
        subject.WriteEndElement("Details");
        subject.WriteStartElement("Details");
        subject.WriteEndElement("Details");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person><Details/><Details/></Person>", sb.ToString());
    }

    [Fact]
    public void Creates_InnerElement_Attributes()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteStartElement("Details");
        subject.WriteStartAttribute("name");
        subject.WriteValue("Jon");
        subject.WriteEndAttribute();
        subject.WriteEndElement("Details");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person><Details name=\"Jon\"/></Person>", sb.ToString());
    }

    [Fact]
    public void WriteAttributeString_Prefix_Namespace()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteAttributeString("pref", "name", "namespace", "Jon");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person pref:name=\"Jon\" xmlns:pref=\"namespace\"/>", sb.ToString());
    }

    [Fact]
    public void WriteAttributeString_Prefix_Empty_Namespace()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteAttributeString("pref", "name", "", "Jon");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person pref:name=\"Jon\"/>", sb.ToString());
    }

    [Fact]
    public void Inner_InnerElement()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteStartElement("Details");
        subject.WriteAttributeString("weight", "76");
        subject.WriteStartElement("Extra");
        subject.WriteEndElement("Extra");
        subject.WriteEndElement("Details");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person><Details weight=\"76\"><Extra/></Details></Person>", sb.ToString());
    }

    [Fact]
    public void WriteElementString()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteElementString("Person", "Somebody");
      }
      Assert.Equal("<Person>Somebody</Person>", sb.ToString());
    }

    [Fact]
    public void WriteElement_Int()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteElementString("Person", 123);
      }
      Assert.Equal("<Person>123</Person>", sb.ToString());
    }

    [Fact]
    public void WriteElement_Double()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteElementString("Person", 123.45);
      }
      Assert.Equal($"<Person>{123.45.ToString()}</Person>", sb.ToString());
    }

    [Fact]
    public void WriteElementString_Null_Value()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteElementString("Person", null);
      }
      Assert.Equal("<Person/>", sb.ToString());
    }

    [Fact]
    public void Inner_WriteElementString()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Persons");
        subject.WriteElementString("Person", "Somebody");
        subject.WriteEndElement("Persons");
      }
      Assert.Equal("<Persons><Person>Somebody</Person></Persons>", sb.ToString());
    }

    [Fact]
    public void WriteElementString_Prefix()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteElementString("soap", "Person", null, "Somebody");
      }
      Assert.Equal("<soap:Person>Somebody</soap:Person>", sb.ToString());
    }

    [Fact]
    public void WriteElementString_Prefix_Namespace()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteElementString("soap", "Person", "some_namespace", "Somebody");
      }
      Assert.Equal("<soap:Person xmlns:soap=\"some_namespace\">Somebody</soap:Person>", sb.ToString());
    }

    [Fact]
    public void Write_Raw_Value()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteRaw("34");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person>34</Person>", sb.ToString());
    }

    [Fact]
    public void Write_Value_Then_Write_Raw()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        subject.WriteStartElement("Person");
        subject.WriteValue("&first");
        subject.WriteRaw("&second");
        subject.WriteEndElement("Person");
      }
      Assert.Equal("<Person>&amp;first&second</Person>", sb.ToString());
    }

    [Fact]
    public void Writes_Supplier_Request()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        BuchbinderBookWriter.Write(subject);
      }
      var actual = sb.ToString();
      var expected = File.ReadAllText(Path.Combine("..", "..", "..", "Examples", "BuchbinderBookWriter.xml"));

      Assert.Equal(XDocument.Parse(expected).ToString(), XDocument.Parse(actual).ToString());
    }

    [Fact]
    public void Writes_Supplier_Enterprise_Request()
    {
      var sb = new StringBuilder();
      using (var subject = new LightXmlWriter(new StringWriter(sb)))
      {
        EnterpriseBookWriter.Write(subject);
      }
      var actual = sb.ToString();
      var expected = File.ReadAllText(Path.Combine("..", "..", "..", "Examples", "EnterpriseBookWriter.xml"));

      Assert.Equal(XDocument.Parse(expected).ToString(), XDocument.Parse(actual).ToString());
    }

    [Fact]
    public void XmlWriter_Writes_Supplier_Enterprise_Request()
    {
      var sb = new StringBuilder();
      using (var subject = System.Xml.XmlWriter.Create(new StringWriter(sb)))
      {
        EnterpriseBookXmlWriter.Write(subject);
      }
      var actual = sb.ToString();
      var expected = File.ReadAllText(Path.Combine("..", "..", "..", "Examples", "EnterpriseBookWriter.xml"));

      Assert.Equal(XDocument.Parse(expected).ToString(), XDocument.Parse(actual).ToString());
    }
  }
}
