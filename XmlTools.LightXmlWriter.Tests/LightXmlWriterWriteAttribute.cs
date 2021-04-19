using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;

namespace XmlTools.Tests
{
  public class LightXmlWriterWriteAttribute
  {
    [Fact]
    public void WriteAttribute_In_Element()
    {
      string WriteValue(Action<LightXmlWriter> writeAction)
        => WriteAttributeUsingLightXmlWriter(writeAction);

      Assert.Equal("<Person attr=\"test\"/>", WriteValue(w => w.WriteValue("test")));
      Assert.Equal("<Person attr=\"x\"/>", WriteValue(w => w.WriteValue('x', escape: false)));
      Assert.Equal("<Person attr=\"&quot;\"/>", WriteValue(w => w.WriteValue('"')));
      Assert.Equal("<Person attr=\"te&amp;st\"/>", WriteValue(w => w.WriteValue("te&st", escape: true)));
      Assert.Equal("<Person attr=\"te&st\"/>", WriteValue(w => w.WriteValue("te&st", escape: false)));
      Assert.Equal("<Person attr=\"True\"/>", WriteValue(w => w.WriteValue(true)));
      Assert.Equal("<Person attr=\"False\"/>", WriteValue(w => w.WriteValue(false)));
      Assert.Equal("<Person attr=\"-2147483648\"/>", WriteValue(w => w.WriteValue(int.MinValue)));
      Assert.Equal("<Person attr=\"-79228162514264337593543950335\"/>", WriteValue(w => w.WriteValue(decimal.MinValue)));
      Assert.Equal("<Person attr=\"-9223372036854775808\"/>", WriteValue(w => w.WriteValue(long.MinValue)));
      Assert.Equal("<Person attr=\"9999-12-31\"/>", WriteValue(w => w.WriteValue(DateTime.MaxValue, "yyyy-MM-dd")));
      Assert.Equal("<Person attr=\"test\"/>", WriteValue(w => w.WriteValue("test", (wr, state) => wr.Write(state))));
      Assert.Equal("<Person attr=\"test\"/>", WriteValue(w => w.WriteValue("test".ToCharArray(), 0, "test".Length)));

#if NETCOREAPP3_1
            Assert.Equal("<Person attr=\"-1.7976931348623157E+308\"/>", WriteValue(w => w.WriteValue(double.MinValue)));
#else
      Assert.Equal("<Person attr=\"-1.79769313486232E+308\"/>", WriteValue(w => w.WriteValue(double.MinValue)));
#endif

#if !NET462
      Assert.Equal("<Person attr=\"test\"/>", WriteValue(w => w.WriteValue("test".AsSpan())));
      Assert.Equal("<Person attr=\"te&amp;st\"/>", WriteValue(w => w.WriteValue("te&st".AsSpan(), escape: true)));
      Assert.Equal("<Person attr=\"te&st\"/>", WriteValue(w => w.WriteValue("te&st".AsSpan(), escape: false)));
      Assert.Equal("<Person attr=\"9999-12-31\"/>", WriteValue(w => w.WriteValue(DateTime.MaxValue, "yyyy-MM-dd".AsSpan())));
#endif
    }

    [Fact]
    public void WriteValue_Different_Characters()
    {
      var s = TestString;
      Assert.Equal(WriteAttributeUsingXmlWriter(w => w.WriteValue(s)).Replace(" />", "/>"), WriteAttributeUsingLightXmlWriter(w => w.WriteValue(s)));
      Assert.Equal(WriteAttributeUsingXmlWriter(w => w.WriteChars(s.ToCharArray(), 0, s.Length)).Replace(" />", "/>"), WriteAttributeUsingLightXmlWriter(w => w.WriteChars(s.ToCharArray(), 0, s.Length)));
#if !NET462
      Assert.Equal(WriteAttributeUsingXmlWriter(w => w.WriteValue(s)).Replace(" />", "/>"), WriteAttributeUsingLightXmlWriter(w => w.WriteValue(s.AsSpan())));
#endif
    }

    private static string WriteAttributeUsingLightXmlWriter(Action<LightXmlWriter> writeAction)
    {
      var sb = new StringBuilder();
      using (var writer = new LightXmlWriter(new StringWriter(sb, CultureInfo.InvariantCulture)))
      {
        writer.WriteStartElement("Person");
        writer.WriteStartAttribute("attr");
        writeAction(writer);
        writer.WriteEndAttribute("attr");
        writer.WriteEndElement("Person");
      }
      return sb.ToString();
    }

    private static string WriteAttributeUsingXmlWriter(Action<XmlWriter> writeAction)
    {
      var sb = new StringBuilder();
      using (var writer = XmlWriter.Create(new StringWriter(sb, CultureInfo.InvariantCulture), new XmlWriterSettings { OmitXmlDeclaration = true }))
      {
        writer.WriteStartElement("Person");
        writer.WriteStartAttribute("attr");
        writeAction(writer);
        writer.WriteEndAttribute();
        writer.WriteEndElement();
      }
      return sb.ToString();
    }

    private static readonly string TestString = GenerateTestString();

    private static string GenerateTestString()
    {
      var sb = new StringBuilder(126);

      char letter;

      for (int i = 32; i < 126; i++)
      {
        letter = Convert.ToChar(i);
        sb.Append(letter);
      }

      sb.Append('ą');
      sb.Append('ó');
      sb.Append('ł');

      return sb.ToString();
    }
  }
}
