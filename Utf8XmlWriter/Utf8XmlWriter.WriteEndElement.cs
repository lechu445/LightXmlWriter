using System;
using System.Diagnostics;
using System.Text;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write elements.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    // Closes one element of specified tag name.
    public void WriteEndElement(XmlExcapedName name)
    {
      if (this.valueWritten)
      {
        Debug.Assert(!this.writingElement, "The value must always be false");
        this.stream.WriteByte((byte)'<');
        this.stream.WriteByte((byte)'/');
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

    // Closes one element of specified tag name.
    public void WriteEndElement(string name)
    {
      if (this.valueWritten)
      {
        Debug.Assert(!this.writingElement, "The value must always be false");
        this.stream.WriteByte((byte)'<');
        this.stream.WriteByte((byte)'/');

        Span<byte> buffer = stackalloc byte[Encoding.UTF8.GetByteCount(name)];
        int count = Encoding.UTF8.GetBytes(name.AsSpan(), buffer);
        Debug.Assert(count == buffer.Length);

        this.stream.Write(buffer);
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
