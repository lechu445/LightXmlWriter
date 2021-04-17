using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;

namespace XmlTools.Tests
{
  public class LightXmlWriterWriteValue
  {
    [Fact]
    public void WriteValue_In_Element()
    {
      string WriteValue(Action<LightXmlWriter> writeAction)
        => WriteValueUsingLightXmlWriter(writeAction);

      Assert.Equal("<Person>test</Person>", WriteValue(w => w.WriteValue("test")));
      Assert.Equal("<Person>x</Person>", WriteValue(w => w.WriteValue('x', escape: false)));
      Assert.Equal("<Person>&quot;</Person>", WriteValue(w => w.WriteValue('"')));
      Assert.Equal("<Person>te&amp;st</Person>", WriteValue(w => w.WriteValue("te&st", escape: true)));
      Assert.Equal("<Person>te&st</Person>", WriteValue(w => w.WriteValue("te&st", escape: false)));
      Assert.Equal("<Person>True</Person>", WriteValue(w => w.WriteValue(true)));
      Assert.Equal("<Person>False</Person>", WriteValue(w => w.WriteValue(false)));
      Assert.Equal("<Person>-2147483648</Person>", WriteValue(w => w.WriteValue(int.MinValue)));
      Assert.Equal("<Person>-79228162514264337593543950335</Person>", WriteValue(w => w.WriteValue(decimal.MinValue)));
      Assert.Equal("<Person>-9223372036854775808</Person>", WriteValue(w => w.WriteValue(long.MinValue)));
      Assert.Equal("<Person>9999-12-31</Person>", WriteValue(w => w.WriteValue(DateTime.MaxValue, "yyyy-MM-dd")));
      Assert.Equal("<Person>test</Person>", WriteValue(w => w.WriteValue("test", (wr, state) => wr.Write(state))));
      Assert.Equal("<Person>test</Person>", WriteValue(w => w.WriteValue("test".ToCharArray(), 0, "test".Length)));

#if NETCOREAPP3_1
      Assert.Equal("<Person>-1.7976931348623157E+308</Person>", WriteValue(w => w.WriteValue(double.MinValue)));
#else
      Assert.Equal("<Person>-1.79769313486232E+308</Person>", WriteValue(w => w.WriteValue(double.MinValue)));
#endif

#if !NET462
      Assert.Equal("<Person>test</Person>", WriteValue(w => w.WriteValue("test".AsSpan())));
      Assert.Equal("<Person>te&amp;st</Person>", WriteValue(w => w.WriteValue("te&st".AsSpan(), escape: true)));
      Assert.Equal("<Person>te&st</Person>", WriteValue(w => w.WriteValue("te&st".AsSpan(), escape: false)));
      Assert.Equal("<Person>9999-12-31</Person>", WriteValue(w => w.WriteValue(DateTime.MaxValue, "yyyy-MM-dd".AsSpan())));
#endif
    }

    [Fact]
    public void WriteValue_Randomised()
    {
      var s = GenerateRandomString((ushort)DateTime.UtcNow.Ticks % 10000);
      Assert.Equal(WriteValueUsingXmlWriter(w => w.WriteValue(s)), WriteValueUsingLightXmlWriter(w => w.WriteValue(s)));
      Assert.Equal(WriteValueUsingXmlWriter(w => w.WriteChars(s.ToCharArray(), 0, s.Length)), WriteValueUsingLightXmlWriter(w => w.WriteChars(s.ToCharArray(), 0, s.Length)));
#if !NET462
      Assert.Equal(WriteValueUsingXmlWriter(w => w.WriteValue(s)), WriteValueUsingLightXmlWriter(w => w.WriteValue(s.AsSpan())));
#endif
    }

    private static string WriteValueUsingLightXmlWriter(Action<LightXmlWriter> writeAction)
    {
      var sb = new StringBuilder();
      using (var writer = new LightXmlWriter(new StringWriter(sb, CultureInfo.InvariantCulture)))
      {
        writer.WriteStartElement("Person");
        writeAction(writer);
        writer.WriteEndElement("Person");
      }
      return sb.ToString();
    }

    private static string WriteValueUsingXmlWriter(Action<XmlWriter> writeAction)
    {
      var sb = new StringBuilder();
      using (var writer = XmlWriter.Create(new StringWriter(sb, CultureInfo.InvariantCulture), new XmlWriterSettings { OmitXmlDeclaration = true }))
      {
        writer.WriteStartElement("Person");
        writeAction(writer);
        writer.WriteEndElement();
      }
      return sb.ToString();
    }

    private static string GenerateRandomString(int length)
    {
      var sb = new StringBuilder(length);
      Random random = new Random();

      char letter;

      for (int i = 0; i < length; i++)
      {
        double flt = random.NextDouble();
        int shift = Convert.ToInt32(Math.Floor(25 * flt));
        letter = Convert.ToChar(shift + 65);
        sb.Append(letter);
      }
      return sb.ToString();
    }
  }
}
