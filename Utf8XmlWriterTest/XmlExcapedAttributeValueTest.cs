using System;
using Xunit;

namespace Tools.Text.Xml.Tests
{
  public class XmlExcapedAttributeValueTest
  {
    [Theory]
    [InlineData("x")]
    [InlineData("")]
    [InlineData("_x")]
    [InlineData("123")]
    [InlineData("xą")]
    [InlineData("ąx")]
    [InlineData("\n")]
    [InlineData("ab cd")]
    [InlineData("<")]
    [InlineData("\"")]
    [InlineData("'")]
    [InlineData("@")]
    public void XmlExcapedElementValue_Test(string name)
    {
      Exception? expectedException = null;
      Exception? actualException = null;
      string? expected = null;
      try
      {
        expected = WriteUsingXmlWriter(name);
      }
      catch (Exception ex)
      {
        expectedException = ex;
      }

      string? actual = null;
      try
      {
        actual = WriteUsingLightXmlWriter(name);
      }
      catch (Exception ex)
      {
        actualException = ex;
      }

      if (expected != null || actual != null)
      {
        Assert.Equal(expected?.Replace(" />", "/>"), actual);
      }

      if (expectedException != null || actualException != null)
      {
        Assert.Equal(expectedException?.GetType(), actualException?.GetType());
        Assert.Equal(expectedException?.Message, actualException?.Message);
      }
    }

    [Fact]
    public void XmlExcapedElementValue_Test2()
    {
      foreach (var ch in TestHelper.GenerateTestString())
      {
        XmlExcapedElementValue_Test(ch.ToString());
      }
    }

    private static string WriteUsingXmlWriter(string name)
      => TestHelper.WriteUsingXmlWriter((xmlWriter) =>
      {
        xmlWriter.WriteStartElement("x");
        xmlWriter.WriteAttributeString("y", name);
        xmlWriter.WriteEndElement();
      });

    private static string WriteUsingLightXmlWriter(string name)
      => TestHelper.WriteUsingLightXmlWriter((xmlWriter) =>
      {
        var encodedValue = XmlExcapedAttributeValue.Encode(name);
        xmlWriter.WriteStartElement("x");
        xmlWriter.WriteAttributeString("y", encodedValue);
        xmlWriter.WriteEndElement("x");
      });
  }
}
