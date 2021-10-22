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
    /// <summary>
    /// Writes out a start tag with the specified local name.
    /// </summary>
    /// <param name="name">The local name of the element.</param>
    public void WriteStartElement(XmlExcapedName name)
    {
      if (this.writingElement)
      {
        this.stream.WriteByte((byte)'>');
      }

      this.stream.WriteByte((byte)'<');
      this.stream.Write(name.Utf8Bytes);
      this.writingElement = true;
      this.valueWritten = false;
    }

    /// <summary>
    /// Writes out a start tag with the specified local name.
    /// </summary>
    /// <param name="name">The local name of the element.</param>
    public void WriteStartElement(string name)
    {
      if (this.writingElement)
      {
        this.stream.WriteByte((byte)'>');
      }

      this.stream.WriteByte((byte)'<');

      Span<byte> buffer = stackalloc byte[Encoding.UTF8.GetByteCount(name)];
      int count = Encoding.UTF8.GetBytes(name.AsSpan(), buffer);
      Debug.Assert(count == buffer.Length);

      this.stream.Write(buffer);
      this.writingElement = true;
      this.valueWritten = false;
    }
  }
}
