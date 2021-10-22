using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write values.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    public void WriteAttributeString(XmlExcapedName name, XmlExcapedAttributeValue value)
    {
      WriteAttributeString(name, value.Utf8Bytes);
    }

    public void WriteAttributeString(XmlExcapedName name, bool value)
    {
      WriteAttributeString(name, value ? TrueBytes : FalseBytes);
    }

    public void WriteAttributeString(XmlExcapedName name, byte value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlExcapedName name, int value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlExcapedName name, long value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlExcapedName name, double value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlExcapedName name, decimal value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlExcapedName name, char value)
    {
      Span<byte> buffer = stackalloc byte[256];
      ReadOnlySpan<char> valueSpan = MemoryMarshal.CreateReadOnlySpan(ref value, 1);
      int bytesWritten = System.Text.Encoding.UTF8.GetBytes(valueSpan, buffer);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, XmlExcapedAttributeValue value)
    {
      WriteAttributeString(name, value.Utf8Bytes);
    }

    public void WriteAttributeString(string name, bool value)
    {
      WriteAttributeString(name, value ? TrueBytes : FalseBytes);
    }

    public void WriteAttributeString(string name, byte value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, int value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, long value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, double value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, decimal value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, char value)
    {
      Span<byte> buffer = stackalloc byte[256];
      ReadOnlySpan<char> valueSpan = MemoryMarshal.CreateReadOnlySpan(ref value, 1);
      int bytesWritten = System.Text.Encoding.UTF8.GetBytes(valueSpan, buffer);
      WriteAttributeString(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, string value)
    {
      Span<byte> buffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(value)];
      System.Text.Encoding.UTF8.GetBytes(value, buffer);
      WriteAttributeString(name, buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAttributeString(string name, ReadOnlySpan<char> value)
    {
      Span<byte> buffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(value)];
      System.Text.Encoding.UTF8.GetBytes(value, buffer);
      WriteAttributeString(name, buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteAttributeString(string name, ReadOnlySpan<byte> value)
    {
      Span<byte> nameBytes = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(name)];
      System.Text.Encoding.UTF8.GetBytes(name, nameBytes);

      WriteStartAttributeImpl(nameBytes);
      this.stream.Write(value);
      this.stream.WriteByte((byte)'"');
    }

    private void WriteAttributeString(XmlExcapedName name, ReadOnlySpan<byte> value)
    {
      WriteStartAttributeImpl(name.Utf8Bytes);
      this.stream.Write(value);
      this.stream.WriteByte((byte)'"');
    }
  }
}
