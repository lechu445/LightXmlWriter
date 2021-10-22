using System.Diagnostics;
using XmlTools.Helpers;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write elements.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    public void WriteEndElement(string? prefix, string name)
    {
      if (this.valueWritten)
      {
        Debug.Assert(!this.writingElement, "The value must always be false");
        this.stream.WriteByte((byte)'<');
        this.stream.WriteByte((byte)'/');
        if (prefix != null)
        {
          this.stream.WriteUtf8EncodedString(prefix);
          this.stream.WriteByte((byte)':');
        }

        this.stream.WriteUtf8EncodedString(name);
      }
      else if (this.writingElement)
      {
        this.stream.WriteByte((byte)'/');
      }

      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteEndElement(XmlEncodedName prefix, XmlEncodedName name)
    {
      if (this.valueWritten)
      {
        Debug.Assert(!this.writingElement, "The value must always be false");
        this.stream.WriteByte((byte)'<');
        this.stream.WriteByte((byte)'/');
        this.stream.Write(prefix.Utf8Bytes);
        this.stream.WriteByte((byte)':');

        this.stream.Write(name.Utf8Bytes);
      }
      else if (this.writingElement)
      {
        this.stream.WriteByte((byte)'/');
      }

      this.stream.WriteByte((byte)'>');
      this.valueWritten = true;
      this.writingElement = false;
    }
  }
}
