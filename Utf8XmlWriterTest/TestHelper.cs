using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Tools.Text.Xml.Tests
{
  public static class TestHelper
  {
    public static string ToUtf8String(this MemoryStream stream)
      => Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);

    public static string WriteUsingXmlWriter(Action<XmlWriter> action)
    {
      var sb = new StringBuilder(20);
      using var sw = new StringWriter(sb);
      using var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true });
      action(xmlWriter);
      xmlWriter.Flush();
      return sb.ToString();
    }

    public static string WriteUsingLightXmlWriter(Action<Utf8XmlWriter> action)
    {
      using var ms = new MemoryStream();
      using var xmlWriter = new Utf8XmlWriter(ms);
      action(xmlWriter);
      xmlWriter.Flush();

      return Encoding.UTF8.GetString(ms.ToArray());
    }

    public static string GenerateTestString()
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
