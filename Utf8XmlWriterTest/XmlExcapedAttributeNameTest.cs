using System;
using Xunit;

namespace Tools.Text.Xml.Tests
{
  public class XmlExcapedAttributeNameTest
  {
    [Theory]
    [InlineData("x")]
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
    public void XmlExcapedName_Attribute_Name_Test(string name)
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
    public void Throws_ArgumentException_When_Name_Is_Empty()
    {
      Assert.Throws<ArgumentException>("value", () => XmlExcapedName.Encode(""));
      Assert.Throws<ArgumentException>("value", () => XmlExcapedName.Encode("".AsSpan()));
    }

    [Fact]
    public void Throws_ArgumentNullException_When_Name_Is_Null()
    {
      Assert.Throws<ArgumentNullException>("value", () => XmlExcapedName.Encode(null!));
    }

    [Fact]
    public void XmlExcapedName_Test2()
    {
      foreach (var ch in TestHelper.GenerateTestString())
      {
        XmlExcapedName_Attribute_Name_Test(ch.ToString());
      }
    }

    private static string WriteUsingXmlWriter(string name)
      => TestHelper.WriteUsingXmlWriter((xmlWriter) =>
      {
        xmlWriter.WriteStartElement("x");
        xmlWriter.WriteAttributeString(name, true.ToString());
        xmlWriter.WriteEndElement();
      });

    private static string WriteUsingLightXmlWriter(string name)
      => TestHelper.WriteUsingLightXmlWriter((xmlWriter) =>
      {
        var attributeName = XmlExcapedName.Encode(name);
        xmlWriter.WriteStartElement("x");
        xmlWriter.WriteAttributeString(attributeName, true);
        xmlWriter.WriteEndElement("x");
      });
  }
}
