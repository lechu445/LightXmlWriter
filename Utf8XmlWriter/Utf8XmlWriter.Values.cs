using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write values.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    private static readonly char[] ValueInvalidChars = { '<', '"' };

    private static Span<byte> TrueBytes => new byte[] { (byte)'T', (byte)'r', (byte)'u', (byte)'e' };

    private static Span<byte> FalseBytes => new byte[] { (byte)'F', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };

    public void WriteValue(XmlExcapedValue value)
    {
      WriteRawValue(value.Utf8Bytes);
    }

    public void WriteValue(string? value, bool escape)
    {
      if (this.writingAttribute)
      {
        WriteXmlString(value, escape);
        return;
      }

      if (this.writingElement)
      {
        this.stream.WriteByte((byte)'>');
        writingElement = false;
      }

      WriteXmlValueString(value, escape);
      this.valueWritten = true;
    }

    public void WriteValue(ReadOnlySpan<char> value, bool escape)
    {
      if (this.writingAttribute)
      {
        WriteXmlString(value, escape);
        return;
      }

      if (this.writingElement)
      {
        this.stream.WriteByte((byte)'>');
        writingElement = false;
      }

      WriteXmlValueString(value, escape);
      this.valueWritten = true;
    }

    public void WriteValue(byte value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(int value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(double value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(decimal value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(long value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(bool value)
    {
      WriteRawValue(value ? TrueBytes : FalseBytes);
    }

    public void WriteValue(string? value)
    {
      WriteValue(value.AsSpan());
    }

    public void WriteValue(ReadOnlySpan<char> value)
    {
      if (!NeedsEscape(value))
      {
        var encoding = System.Text.Encoding.UTF8;
        int size = encoding.GetMaxByteCount(value.Length);
        Span<byte> buffer = stackalloc byte[size];
        int bytesWritten = encoding.GetBytes(value, buffer);
        ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);
        WriteRawValue(formatted);
      }
      else
      {
        WriteEscapedValue(value);
      }

      static bool NeedsEscape(ReadOnlySpan<char> span) => span.IndexOfAny(ValueInvalidChars) != -1;
    }

    public void WriteValue(DateTime value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(DateTimeOffset value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(TimeSpan value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(Guid value)
    {
      Span<byte> buffer = stackalloc byte[256];
      Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
      ReadOnlySpan<byte> formatted = buffer.Slice(0, bytesWritten);

      WriteRawValue(formatted);
    }

    public void WriteValue(DateTime value, ReadOnlySpan<char> format)
    {
      Span<char> chars = stackalloc char[256];
      if (value.TryFormat(chars, out int charsWritten, format))
      {
        Span<byte> buffer = stackalloc byte[256];
        int bytesWritten = System.Text.Encoding.UTF8.GetBytes(chars, buffer);
        WriteRawValue(buffer.Slice(0, bytesWritten));
      }
      else
      {
        Debug.Assert(false, "buffer was too small");
        var str = value.ToString(format.ToString());
        WriteRawValue(System.Text.Encoding.UTF8.GetBytes(str));
      }
    }

    public void WriteValue(DateTimeOffset value, ReadOnlySpan<char> format)
    {
      Span<char> chars = stackalloc char[256];
      if (value.TryFormat(chars, out int charsWritten, format))
      {
        Span<byte> buffer = stackalloc byte[256];
        int bytesWritten = System.Text.Encoding.UTF8.GetBytes(chars, buffer);
        WriteRawValue(buffer.Slice(0, bytesWritten));
      }
      else
      {
        Debug.Assert(false, "buffer was too small");
        var str = value.ToString(format.ToString());
        WriteRawValue(System.Text.Encoding.UTF8.GetBytes(str));
      }
    }

    public void WriteValue(TimeSpan value, ReadOnlySpan<char> format)
    {
      Span<char> chars = stackalloc char[256];
      if (value.TryFormat(chars, out int charsWritten, format))
      {
        Span<byte> buffer = stackalloc byte[256];
        int bytesWritten = System.Text.Encoding.UTF8.GetBytes(chars, buffer);
        WriteRawValue(buffer.Slice(0, bytesWritten));
      }
      else
      {
        Debug.Assert(false, "buffer was too small");
        var str = value.ToString(format.ToString());
        WriteRawValue(System.Text.Encoding.UTF8.GetBytes(str));
      }
    }

    private void WriteRawValue(ReadOnlySpan<byte> value)
    {
      if (this.writingAttribute)
      {
        this.stream.Write(value);
      }
      else
      {
        if (this.writingElement)
        {
          this.stream.WriteByte((byte)'>');
          writingElement = false;
        }

        this.stream.Write(value);
        this.valueWritten = true;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlString(string? value, bool escape)
    {
      WriteXmlString(value.AsSpan(), escape);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlString(ReadOnlySpan<char> value, bool escape)
    {
      if (escape)
      {
        WriteEscaped(value, escapeValue: false);
      }
      else
      {
        WriteValue(value);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlValueString(string? value, bool escape)
    {
      WriteXmlValueString(value.AsSpan(), escape);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlValueString(ReadOnlySpan<char> value, bool escape)
    {
      if (escape)
      {
        WriteEscaped(value, escapeValue: true);
      }
      else
      {
        WriteValue(value);
      }
    }

    private void WriteEscapedValue(ReadOnlySpan<char> value)
    {
      if (this.writingAttribute)
      {
        WriteEscaped(value, escapeValue: true);
      }
      else
      {
        if (this.writingElement)
        {
          this.stream.WriteByte((byte)'>');
          writingElement = false;
        }

        WriteEscaped(value, escapeValue: true);
        this.valueWritten = true;
      }
    }
  }
}
