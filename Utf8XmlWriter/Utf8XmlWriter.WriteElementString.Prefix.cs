using XmlTools.Helpers;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write elements.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    public void WriteElementString(string? prefix, string name, string? ns, string? value, bool escapeValue = true)
    {
      WriteStartElement(prefix, name, ns);
      WriteValue(value, escapeValue);
      this.stream.WriteByte((byte)'<');
      this.stream.WriteByte((byte)'/');
      if (prefix != null)
      {
        this.stream.WriteUtf8EncodedString(prefix);
        this.stream.WriteByte((byte)':');
      }

      this.stream.WriteUtf8EncodedString(name);
      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }
  }
}
