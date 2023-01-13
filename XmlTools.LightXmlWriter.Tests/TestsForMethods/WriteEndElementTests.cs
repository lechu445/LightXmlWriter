using System.IO;
using System.Text;
using Xunit;

namespace XmlTools.Tests.TestsForMethods
{
  public class WriteEndElementTests
  {
    [Fact]
    public void WriteEndElement()
    {
      var sb = new StringBuilder();
      using (var writer = new LightXmlWriter(new StringWriter(sb)))
      {
        writer.WriteEndElement(null);
        Assert.Equal(">", sb.ToString());
        sb.Clear();

        writer.WriteEndElement("");
        Assert.Equal("</>", sb.ToString());
        sb.Clear();

        writer.WriteEndElement("x");
        Assert.Equal("</x>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteEndElement("root");
        Assert.Equal("<root/>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteValue("value");
        writer.WriteEndElement("root");
        Assert.Equal("<root>value</root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteValue("value");
        writer.WriteEndElement("");
        Assert.Equal("<root>value</>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("");
        writer.WriteValue("value");
        writer.WriteEndElement("root");
        Assert.Equal("<>value</root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteValue("");
        writer.WriteEndElement("root");
        Assert.Equal("<root></root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteValue((string)null);
        writer.WriteEndElement("root");
        Assert.Equal("<root></root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteStartAttribute("attr");
        writer.WriteEndElement("root");
        Assert.Equal("<root attr=\"/>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteStartAttribute("attr");
        writer.WriteValue("value");
        writer.WriteEndElement("root");
        Assert.Equal("<root attr=\"value/>", sb.ToString());
        sb.Clear();
      }
    }

    [Fact]
    public void WriteEndElement_With_Prefix()
    {
      var sb = new StringBuilder();
      using (var writer = new LightXmlWriter(new StringWriter(sb)))
      {
        writer.WriteEndElement(null, null);
        Assert.Equal(">", sb.ToString());
        sb.Clear();

        writer.WriteEndElement("", null);
        Assert.Equal("</:>", sb.ToString());
        sb.Clear();

        writer.WriteEndElement(null, "");
        Assert.Equal("</>", sb.ToString());
        sb.Clear();

        writer.WriteEndElement("", "");
        Assert.Equal("</:>", sb.ToString());
        sb.Clear();

        writer.WriteEndElement(null, "root");
        Assert.Equal("</root>", sb.ToString());
        sb.Clear();

        writer.WriteEndElement("", "root");
        Assert.Equal("</:root>", sb.ToString());
        sb.Clear();

        writer.WriteEndElement("prefix", "");
        Assert.Equal("</prefix:>", sb.ToString());
        sb.Clear();

        writer.WriteEndElement("prefix", null);
        Assert.Equal("</prefix:>", sb.ToString());
        sb.Clear();

        writer.WriteEndElement("prefix", "root");
        Assert.Equal("</prefix:root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteEndElement("prefix", "root");
        Assert.Equal("<root/>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteValue("value");
        writer.WriteEndElement("prefix", "root");
        Assert.Equal("<root>value</prefix:root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteValue("value");
        writer.WriteEndElement("prefix", "");
        Assert.Equal("<root>value</prefix:>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("");
        writer.WriteValue("value");
        writer.WriteEndElement("prefix", "root");
        Assert.Equal("<>value</prefix:root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteValue("");
        writer.WriteEndElement("prefix", "root");
        Assert.Equal("<root></prefix:root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteValue((string)null);
        writer.WriteEndElement("prefix", "root");
        Assert.Equal("<root></prefix:root>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteStartAttribute("attr");
        writer.WriteEndElement("prefix", "root");
        Assert.Equal("<root attr=\"/>", sb.ToString());
        sb.Clear();

        writer.WriteStartElement("root");
        writer.WriteStartAttribute("attr");
        writer.WriteValue("value");
        writer.WriteEndElement("prefix", "root");
        Assert.Equal("<root attr=\"value/>", sb.ToString());
        sb.Clear();
      }
    }
  }
}
