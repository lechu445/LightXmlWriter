using System;
using System.Runtime.CompilerServices;

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
    public void WriteStartElement(XmlEncodedName name)
    {
      WriteStartElementImpl(name.Utf8Bytes);
    }

    /// <summary>
    /// Writes out a start tag with the specified local name.
    /// </summary>
    /// <param name="name">The local name of the element.</param>
    public void WriteStartElement(string name)
    {
      Span<byte> nameBuffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(name)];
      System.Text.Encoding.UTF8.GetBytes(name, nameBuffer);
      WriteStartElementImpl(nameBuffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteStartElementImpl(ReadOnlySpan<byte> name)
    {
      if (this.writingElement)
      {
        this.stream.WriteByte((byte)'>');
      }

      this.stream.WriteByte((byte)'<');
      this.stream.Write(name);
      this.writingElement = true;
      this.valueWritten = false;
    }
  }
}
