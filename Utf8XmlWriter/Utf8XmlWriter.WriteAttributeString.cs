using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains WriteAttributeString methods.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    public void WriteAttributeString(XmlEncodedName name, XmlEncodedAttributeValue value)
    {
      WriteAttributeStringImpl(name.Utf8Bytes, value.Utf8Bytes);
    }

    public void WriteAttributeString(XmlEncodedName name, bool value)
    {
      WriteAttributeStringImpl(name.Utf8Bytes, value ? TrueBytes : FalseBytes);
    }

    public void WriteAttributeString(XmlEncodedName name, byte value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name.Utf8Bytes, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlEncodedName name, int value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name.Utf8Bytes, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlEncodedName name, long value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name.Utf8Bytes, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlEncodedName name, double value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name.Utf8Bytes, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlEncodedName name, decimal value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name.Utf8Bytes, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlEncodedName name, char value)
    {
      Span<byte> buffer = stackalloc byte[256];
      ReadOnlySpan<char> valueSpan = MemoryMarshal.CreateReadOnlySpan(ref value, 1);
      int bytesWritten = System.Text.Encoding.UTF8.GetBytes(valueSpan, buffer);
      WriteAttributeStringImpl(name.Utf8Bytes, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(XmlEncodedName name, string? value)
    {
      if (value == null)
      {
        WriteAttributeStringImpl(name.Utf8Bytes, ReadOnlySpan<byte>.Empty);
        return;
      }

      WriteAttributeStringImpl(name.Utf8Bytes, value.AsSpan());
    }

    public void WriteAttributeString(XmlEncodedName name, XmlEncodedValue value)
    {
      WriteAttributeStringImpl(name.Utf8Bytes, value.Utf8Bytes);
    }

    public void WriteAttributeString(string name, XmlEncodedAttributeValue value)
    {
      WriteAttributeStringImpl(name, value.Utf8Bytes);
    }

    public void WriteAttributeString(string name, bool value)
    {
      WriteAttributeStringImpl(name, value ? TrueBytes : FalseBytes);
    }

    public void WriteAttributeString(string name, byte value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, int value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, long value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, double value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, decimal value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      WriteAttributeStringImpl(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, char value)
    {
      Span<byte> buffer = stackalloc byte[256];
      ReadOnlySpan<char> valueSpan = MemoryMarshal.CreateReadOnlySpan(ref value, 1);
      int bytesWritten = System.Text.Encoding.UTF8.GetBytes(valueSpan, buffer);
      WriteAttributeStringImpl(name, buffer.Slice(0, bytesWritten));
    }

    public void WriteAttributeString(string name, string? value)
    {
      if (value is null)
      {
        WriteAttributeStringImpl(name, ReadOnlySpan<byte>.Empty);
        return;
      }

      WriteAttributeString(name, value.AsSpan());
    }

    public void WriteAttributeString(string name, ReadOnlySpan<char> value)
    {
      WriteAttributeString(name.AsSpan(), value);
    }

    public void WriteAttributeString(ReadOnlySpan<char> name, ReadOnlySpan<char> value)
    {
      Span<byte> nameBytes = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(name)];
      System.Text.Encoding.UTF8.GetBytes(name, nameBytes);

      Span<byte> buffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(value)];
      System.Text.Encoding.UTF8.GetBytes(value, buffer);
      WriteAttributeStringImpl(nameBytes, buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAttributeStringImpl(ReadOnlySpan<byte> name, ReadOnlySpan<char> value)
    {
      Span<byte> buffer = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(value)];
      System.Text.Encoding.UTF8.GetBytes(value, buffer);
      WriteAttributeStringImpl(name, buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteAttributeStringImpl(string name, ReadOnlySpan<byte> value)
    {
      Span<byte> nameBytes = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(name)];
      System.Text.Encoding.UTF8.GetBytes(name, nameBytes);
      WriteAttributeStringImpl(nameBytes, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteAttributeStringImpl(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
    {
      WriteStartAttributeImpl(name);
      this.stream.Write(value);
      this.stream.WriteByte((byte)'"');
    }
  }
}
