using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace XmlTools
{
  /// <summary>
  /// Contains methods to write values.
  /// </summary>
  public sealed partial class LightXmlWriter
  {
    public void WriteValue(string? value, bool escape = true)
    {
      if (this.writingAttribute)
      {
        WriteXmlString(value, escape);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      WriteXmlValueString(value, escape);
      this.valueWritten = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <param name="escape"></param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public void WriteValue(char[] value, int index, int count, bool escape = true)
      => WriteChars(value, index, count, escape);

    // method exists for compatability with XmlWriter
    public void WriteChars(char[] value, int index, int count, bool escape = true)
    {
      if (this.writingAttribute)
      {
        if (escape)
        {
#if NETSTANDARD1_3
          WriteEscaped(new string(value, index, count), escapeValue: false);
#else
          WriteEscaped(value.AsSpan(index, count), escapeValue: false);
#endif
        }
        else
        {
          this.writer.Write(value, index, count);
        }

        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      if (escape)
      {
#if NETSTANDARD1_3
        WriteEscaped(new string(value, index, count), escapeValue: true);
#else
        WriteEscaped(value.AsSpan(index, count), escapeValue: true);
#endif
      }
      else
      {
        this.writer.Write(value, index, count);
      }

      this.valueWritten = true;
    }

    public void WriteValue<TArg>(TArg arg, Action<TextWriter, TArg> writeAction)
    {
      if (this.writingAttribute)
      {
        writeAction(this.writer, arg);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      writeAction(this.writer, arg);
      this.valueWritten = true;
    }

    public void WriteValue(int value)
    {
      if (this.writingAttribute)
      {
        this.writer.Write(value);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(double value)
    {
      if (this.writingAttribute)
      {
        this.writer.Write(value);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(decimal value)
    {
      if (this.writingAttribute)
      {
        this.writer.Write(value);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(char value, bool escape = true)
    {
      if (this.writingAttribute)
      {
        WriteXmlChar(value, escape);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      WriteXmlCharValue(value, escape);
      this.valueWritten = true;
    }

    public void WriteValue(long value)
    {
      if (this.writingAttribute)
      {
        this.writer.Write(value);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(bool value)
    {
      if (this.writingAttribute)
      {
        this.writer.Write(value);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      this.writer.Write(value);
      this.valueWritten = true;
    }

#if NETSTANDARD1_3
    public void WriteValue(DateTime value, string? format = null)
    {
      if (this.writingAttribute)
      {
        this.writer.Write(value.ToString(format));
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      this.writer.Write(value.ToString(format));
      this.valueWritten = true;
    }
#else
    public void WriteValue(DateTime value, ReadOnlySpan<char> format)
    {
      if (this.writingAttribute)
      {
        Write(value, format);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      Write(value, format);
      this.valueWritten = true;

      void Write(DateTime value, ReadOnlySpan<char> format)
      {
        Span<char> buffer = stackalloc char[Math.Min(Math.Max(format.Length, 20), 255)];
        if (value.TryFormat(buffer, out int charsWritten, format))
        {
          this.writer.Write(buffer.Slice(0, charsWritten));
        }
        else
        {
          this.writer.Write(value.ToString(format.ToString()));
        }
      }
    }

    public void WriteValue(ReadOnlySpan<char> value, bool escape = true)
    {
      if (this.writingAttribute)
      {
        WriteXmlString(value, escape);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      WriteXmlValueString(value, escape);
      this.valueWritten = true;
    }
#endif

#if !NETSTANDARD1_3
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlString(ReadOnlySpan<char> value, bool escape = true)
    {
      if (escape)
      {
        WriteEscaped(value, escapeValue: false);
      }
      else
      {
        this.writer.Write(value);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlValueString(ReadOnlySpan<char> value, bool escape = true)
    {
      if (escape)
      {
        WriteEscaped(value, escapeValue: true);
      }
      else
      {
        this.writer.Write(value);
      }
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlValueString(string? value, bool escape)
    {
      if (escape)
      {
        WriteEscaped(value, escapeValue: true);
      }
      else
      {
        this.writer.Write(value);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlString(string? value, bool escape)
    {
      if (escape)
      {
        WriteEscaped(value, escapeValue: false);
      }
      else
      {
        this.writer.Write(value);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlChar(char value, bool escape)
    {
      if (escape)
      {
        WriteEscapeSequence(value);
      }
      else
      {
        this.writer.Write(value);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlCharValue(char value, bool escape)
    {
      if (escape)
      {
        WriteEscapeSequenceForValue(value);
      }
      else
      {
        this.writer.Write(value);
      }
    }
  }
}
