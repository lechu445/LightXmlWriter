using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using XmlTools.Helpers;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write values.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    ReadOnlySpan<byte> xmlns => new byte[] { (byte)' ', (byte)'x', (byte)'m', (byte)'l', (byte)'n', (byte)'s', (byte)':' };

    public void WriteAttributeString(string? prefix, string name, string? ns, string? value, bool escapeValue = true)
    {
      WriteStartAttribute(prefix, name);
      WriteXmlString(value, escapeValue);
      WriteEndAttribute();
      if (!string.IsNullOrEmpty(ns))
      {
        if (prefix != null)
        {
          this.stream.Write(xmlns);
          this.stream.WriteUtf8EncodedString(prefix);
          this.stream.WriteByte((byte)'=');
          this.stream.WriteByte((byte)'"');
        }
        else
        {
          this.stream.Write(xmlns);
          this.stream.WriteByte((byte)'=');
          this.stream.WriteByte((byte)'"');
        }

        this.stream.WriteUtf8EncodedString(ns);
        this.stream.WriteByte((byte)'"');
      }
    }

    public void WriteAttributeString(XmlEncodedName prefix, XmlEncodedName name, XmlEncodedName ns, string value)
    {
      if (value is null)
      {
        WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, ReadOnlySpan<byte>.Empty);
        return;
      }

      Span<byte> valueBuffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(value)];
      System.Text.Encoding.UTF8.GetBytes(value, valueBuffer);

      WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, valueBuffer);
    }

    public void WriteAttributeString(XmlEncodedName prefix, XmlEncodedName name, XmlEncodedName ns, ReadOnlySpan<char> value)
    {
      Span<byte> valueBuffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(value)];
      System.Text.Encoding.UTF8.GetBytes(value, valueBuffer);

      WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, valueBuffer);
    }

    public void WriteAttributeString(XmlEncodedName prefix, string name, XmlEncodedName ns, string value)
    {
      Span<byte> nameBuffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(name)];
      System.Text.Encoding.UTF8.GetBytes(name, nameBuffer);

      Span<byte> valueBuffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(value)];
      System.Text.Encoding.UTF8.GetBytes(value, valueBuffer);

      WriteAttributeString(prefix.Utf8Bytes, nameBuffer, ns.Utf8Bytes, valueBuffer);
    }

    public void WriteAttributeString(XmlEncodedName prefix, XmlEncodedName name, XmlEncodedName ns, XmlEncodedAttributeValue value)
    {
      WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, value.Utf8Bytes);
    }

    public void WriteAttributeString(XmlEncodedName prefix, XmlEncodedName name, XmlEncodedName ns, bool value)
    {
      ReadOnlySpan<byte> valueBytes = value ? TrueBytes : FalseBytes;
      WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, valueBytes);
    }

    public void WriteAttributeString(XmlEncodedName prefix, XmlEncodedName name, XmlEncodedName ns, byte value)
    {
      Span<byte> valueBuffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, valueBuffer, out int bytesWritten);

      WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, valueBuffer);
    }

    public void WriteAttributeString(XmlEncodedName prefix, XmlEncodedName name, XmlEncodedName ns, int value)
    {
      Span<byte> valueBuffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, valueBuffer, out int bytesWritten);

      WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, valueBuffer);
    }

    public void WriteAttributeString(XmlEncodedName prefix, XmlEncodedName name, XmlEncodedName ns, double value)
    {
      Span<byte> valueBuffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, valueBuffer, out int bytesWritten);

      WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, valueBuffer);
    }

    public void WriteAttributeString(XmlEncodedName prefix, XmlEncodedName name, XmlEncodedName ns, decimal value)
    {
      Span<byte> valueBuffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, valueBuffer, out int bytesWritten);

      WriteAttributeString(prefix.Utf8Bytes, name.Utf8Bytes, ns.Utf8Bytes, valueBuffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteAttributeString(ReadOnlySpan<byte> prefix, ReadOnlySpan<byte> name, ReadOnlySpan<byte> ns, ReadOnlySpan<byte> value)
    {
      WriteStartAttributeImpl(prefix, name);
      WriteRawValue(value);
      WriteEndAttribute();
      this.stream.Write(xmlns);
      this.stream.Write(prefix);
      this.stream.WriteByte((byte)'=');
      this.stream.WriteByte((byte)'"');
      this.stream.Write(ns);
      this.stream.WriteByte((byte)'"');
    }
  }
}
