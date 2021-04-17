using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

#if NETSTANDARD1_3
using System.Buffers;
#endif

namespace XmlTools
{
  /// <summary>
  /// Light implementation of XmlWriter equivalent designed to be as close as
  /// possible of XmlWriter usage &amp; behaviour with most common settings (no pretty-print, no xml declaration, etc.)
  /// </summary>
  public sealed partial class LightXmlWriter : IDisposable
  {
    private readonly TextWriter writer;
    private bool writingElement = false;
    private bool writingAttribute = false;
    private bool valueWritten = false;
#if NETSTANDARD1_3
    private char[] buffer;
#endif

#if NETSTANDARD1_3
    /// <summary>Initializes a new instance of the <see cref="LightXmlWriter"/> class.</summary>
    /// <param name="writer">TextWriter which LightXmlWriter directly writes to.</param>
    /// <param name="bufferSize">Size of internal buffer used for single operation on string.</param>
    /// <exception cref="ArgumentNullException">writer property is null.</exception>
    public LightXmlWriter(TextWriter writer, int bufferSize = 1024)
    {
      this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
      this.buffer = ArrayPool<char>.Shared.Rent(bufferSize);
    }
#else
    /// <summary>Initializes a new instance of the <see cref="LightXmlWriter"/> class.</summary>
    /// <param name="writer">TextWriter which LightXmlWriter directly writes to.</param>
    /// <exception cref="ArgumentNullException">writer property is null.</exception>
    public LightXmlWriter(TextWriter writer)
    {
      this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }
#endif

    /// <summary>Gets TextWriter which LightXmlWriter directly writes to.</summary>
    public TextWriter Writer => this.writer;

    public void Dispose()
    {
#if NETSTANDARD1_3
      if (this.buffer != null)
      {
        ArrayPool<char>.Shared.Return(this.buffer);
        this.buffer = null!;
      }
#endif
      this.writer.Dispose();
    }

    // Writes out a start tag with the specified local name with no namespace.
    public void WriteStartElement(string name)
    {
      if (this.writingElement)
      {
        this.writer.Write('>');
      }

      this.writer.Write('<');
      this.writer.Write(name);
      this.writingElement = true;
      this.valueWritten = false;
    }

    // Writes out the specified start tag and associates it with the given namespace.
    public void WriteStartElement(string name, string? ns)
    {
      if (this.writingElement)
      {
        this.writer.Write('>');
      }

      this.writer.Write('<');
      this.writer.Write(name);
      if (ns != null)
      {
        this.writer.Write(" xmlns=\"");
        this.writer.Write(ns);
        this.writer.Write('"');
      }

      this.writingElement = true;
      this.valueWritten = false;
    }

    // Writes out the specified start tag and associates it with the given namespace and prefix.
    public void WriteStartElement(string? prefix, string name, string? ns)
    {
      if (this.writingElement)
      {
        this.writer.Write('>');
      }

      this.writer.Write('<');
      if (prefix != null)
      {
        this.writer.Write(prefix);
        this.writer.Write(':');
      }

      this.writer.Write(name);

      if (ns != null && prefix != null)
      {
        this.writer.Write(" xmlns:");
        this.writer.Write(prefix);
        this.writer.Write('=');
        this.writer.Write('"');
        this.writer.Write(ns);
        this.writer.Write('"');
      }

      this.writingElement = true;
      this.valueWritten = false;
    }

    // Closes one element of specified tag name.
    public void WriteEndElement(string name)
    {
      if (this.valueWritten)
      {
        Debug.Assert(!this.writingElement, "The value must always be false");
        this.writer.Write('<');
        this.writer.Write('/');
        this.writer.Write(name);
      }
      else if (this.writingElement)
      {
        this.writer.Write('/');
      }

      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    // Closes one element of specified tag name and prefix.
    public void WriteEndElement(string? prefix, string name)
    {
      if (this.valueWritten)
      {
        Debug.Assert(!this.writingElement, "The value must always be false");
        this.writer.Write('<');
        this.writer.Write('/');
        if (prefix != null)
        {
          this.writer.Write(prefix);
          this.writer.Write(':');
        }

        this.writer.Write(name);
      }
      else if (this.writingElement)
      {
        this.writer.Write('/');
      }

      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(string name, string? value, bool escapeValue = true)
    {
      WriteStartElement(name);
      if (value == null)
      {
        WriteEndElement(name);
      }
      else
      {
        WriteValue(value, escapeValue);
        this.writer.Write('<');
        this.writer.Write('/');
        this.writer.Write(name);
        this.writer.Write('>');
        this.valueWritten = true;
      }

      this.writingElement = false;
    }

    public void WriteElementString(string name, DateTime value, string? format)
    {
      WriteStartElement(name);
      WriteValue(value, format);
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }

#if !NETSTANDARD1_3
    public void WriteElementString(string name, ReadOnlySpan<char> value, bool escapeValue = true)
    {
      WriteStartElement(name);
      WriteValue(value, escapeValue);
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }
#endif

    public void WriteElementString(string? prefix, string name, string? ns, string? value, bool escapeValue = true)
    {
      WriteStartElement(prefix, name, ns);
      WriteValue(value, escapeValue);
      this.writer.Write('<');
      this.writer.Write('/');
      if (prefix != null)
      {
        this.writer.Write(prefix);
        this.writer.Write(':');
      }

      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(string name, int value)
    {
      WriteStartElement(name);
      this.writer.Write('>');
#if NETSTANDARD1_3
      this.writer.Write(value);
#else
      if (value < 0)
      {
        this.writer.Write('-');
        value = -value;
      }

      int i = 0;
      Span<char> chars = stackalloc char[10];
      do
      {
        value = Math.DivRem(value, 10, out int remainder);
        chars[i] = (char)('0' + remainder);
        i++;
      }
      while (value != 0);

      for (int j = i - 1; j >= 0; j--)
      {
        this.writer.Write(chars[j]);
      }

#endif
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(string name, double value)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(string name, char value, bool escapeValue = true)
    {
      WriteStartElement(name);
      WriteValue(value);
      this.writer.Write('<');
      this.writer.Write('/');
      WriteXmlChar(value, escapeValue);
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteElementString(string name, bool value)
    {
      WriteStartElement(name);
      if (value)
      {
        this.writer.Write(">True</");
      }
      else
      {
        this.writer.Write(">False</");
      }

      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingElement = false;
    }

    public void WriteStartAttribute(string name)
    {
      WriteStartAttributeImpl(name);
      this.writingAttribute = true;
    }

    public void WriteStartAttribute(string? prefix, string name)
    {
      this.writer.Write(' ');
      if (prefix != null)
      {
        this.writer.Write(prefix);
        this.writer.Write(':');
      }

      this.writer.Write(name);
      this.writer.Write('=');
      this.writer.Write('"');
      this.writingAttribute = true;
    }

    public void WriteEndAttribute(string name)
    {
      WriteEndAttribute();
    }

    public void WriteEndAttribute()
    {
      this.writer.Write('"');
      this.writingAttribute = false;
    }

    public void WriteAttributeString(string? prefix, string name, string? ns, string? value, bool escapeValue = true)
    {
      WriteStartAttribute(prefix, name);
      WriteXmlString(value, escapeValue);
      WriteEndAttribute();
      if (!string.IsNullOrEmpty(ns))
      {
        this.writer.Write(" xmlns");
        if (prefix != null)
        {
          this.writer.Write(':');
          this.writer.Write(prefix);
        }

        this.writer.Write('=');
        this.writer.Write('"');
        this.writer.Write(ns);
        this.writer.Write('"');
      }
    }

    public void WriteAttributeString(string name, string? value, bool escapeValue = true)
    {
      WriteStartAttributeImpl(name);
      WriteXmlString(value, escapeValue);
      this.writer.Write('"');
    }

#if !NETSTANDARD1_3
    public void WriteAttributeString(string name, ReadOnlySpan<char> value, bool escapeValue = true)
    {
      WriteStartAttributeImpl(name);
      WriteXmlString(value, escapeValue);
      this.writer.Write('"');
    }
#endif

    public void WriteAttributeString(string name, int value)
    {
      WriteStartAttributeImpl(name);

#if NETSTANDARD1_3
      this.writer.Write(value);
#else
      if (value < 0)
      {
        this.writer.Write('-');
        value = -value;
      }

      int i = 0;
      Span<char> chars = stackalloc char[10];
      do
      {
        value = Math.DivRem(value, 10, out int remainder);
        chars[i] = (char)('0' + remainder);
        i++;
      }
      while (value != 0);

      for (int j = i - 1; j >= 0; j--)
      {
        this.writer.Write(chars[j]);
      }
#endif
      this.writer.Write('"');
    }

    public void WriteAttributeString(string name, char value, bool escapeValue = true)
    {
      WriteStartAttributeImpl(name);
      WriteXmlChar(value, escapeValue);
      this.writer.Write('"');
    }

    public void WriteAttributeString(string name, double value)
    {
      WriteStartAttributeImpl(name);
      this.writer.Write(value);
      this.writer.Write('"');
    }

    public void WriteAttributeString(string name, decimal value)
    {
      WriteStartAttributeImpl(name);
      this.writer.Write(value);
      this.writer.Write('"');
    }

    public void WriteAttributeString(string name, bool value)
    {
      WriteStartAttributeImpl(name);
      this.writer.Write(value);
      this.writer.Write('"');
    }

    public void WriteAttributeString<TArg>(string name, TArg arg, Action<TextWriter, TArg> writeValueAction)
    {
      WriteStartAttributeImpl(name);
      writeValueAction(this.writer, arg);
      this.writer.Write('"');
    }

    public void WriteRaw(string value)
    {
      WriteValue(value, escape: false);
    }

    public void WriteString(string? value, bool escape = true)
    {
      WriteValue(value, escape);
    }

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

      WriteXmlString(value, escape);
      this.valueWritten = true;
    }

    public void WriteChars(char[] value, int index, int count)
    {
      if (this.writingAttribute)
      {
        this.writer.Write(value, index, count);
        return;
      }

      if (this.writingElement)
      {
        this.writer.Write('>');
        writingElement = false;
      }

      this.writer.Write(value, index, count);
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

      WriteXmlChar(value, escape);
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

      WriteXmlString(value, escape);
      this.writer.Write(value);
      this.valueWritten = true;
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteStartAttributeImpl(string name)
    {
      this.writer.Write(' ');
      this.writer.Write(name);
      this.writer.Write('=');
      this.writer.Write('"');
    }

#if !NETSTANDARD1_3
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlString(ReadOnlySpan<char> value, bool escape = true)
    {
      if (escape)
      {
        WriteEscaped(value);
      }
      else
      {
        this.writer.Write(value);
      }
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteXmlString(string? value, bool escape)
    {
      if (escape)
      {
        WriteEscaped(value);
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
  }
}
