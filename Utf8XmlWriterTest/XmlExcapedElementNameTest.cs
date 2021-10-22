using System;
using Xunit;

namespace Tools.Text.Xml.Tests
{
  public class XmlExcapedElementNameTest
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
    public void XmlExcapedElementName_Test(string name)
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
      Assert.Throws<ArgumentException>("value", () => XmlEncodedName.Encode(""));
      Assert.Throws<ArgumentException>("value", () => XmlEncodedName.Encode("".AsSpan()));
    }

    [Fact]
    public void Throws_ArgumentNullException_When_Name_Is_Null()
    {
      Assert.Throws<ArgumentNullException>("value", () => XmlEncodedName.Encode(null!));
    }

    [Fact]
    public void XmlExcapedElementName_Test2()
    {
      foreach (var ch in TestHelper.GenerateTestString())
      {
        XmlExcapedElementName_Test(ch.ToString());
      }
    }

    private static string WriteUsingXmlWriter(string name)
      => TestHelper.WriteUsingXmlWriter((xmlWriter) =>
      {
        xmlWriter.WriteStartElement(name);
        xmlWriter.WriteEndElement();
      });

    private static string WriteUsingLightXmlWriter(string name)
      => TestHelper.WriteUsingLightXmlWriter((xmlWriter) =>
      {
        var elementName = XmlEncodedName.Encode(name);
        xmlWriter.WriteStartElement(elementName);
        xmlWriter.WriteEndElement(elementName);
      });
  }
}
