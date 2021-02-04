﻿using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace XmlTools
{
  /// <summary>
  /// Light implementation of XmlWriter equivalent designed to be as close as
  /// possible of XmlWriter usage & behaviour with most common settings (no pretty-print, no xml declaration, etc.)
  /// </summary>
  public sealed class LightXmlWriter : IDisposable
  {
    private readonly TextWriter writer;
    private bool writingElement = false;
    private bool writingAttribute = false;
    private bool valueWritten = false;
#if NETSTANDARD1_3
    private char[] buffer;
#endif

    public TextWriter Writer => this.writer;

#if NETSTANDARD1_3
    public LightXmlWriter(TextWriter writer, int bufferSize = 1024)
    {
      this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
      this.buffer = ArrayPool<char>.Shared.Rent(bufferSize);
    }
#else
    public LightXmlWriter(TextWriter writer)
    {
      this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }
#endif

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
        Debug.Assert(!this.writingElement);
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
        Debug.Assert(!this.writingElement);
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
      } while (value != 0);
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
      } while (value != 0);
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

#region WriteEscaped(value)
    // This is port of SecurityElement.Escape(value) that writes directly into this.writer instead of string creation

    private static readonly char[] s_escapeChars = new[] { '<', '>', '\"', '\'', '&' };

#if !NETSTANDARD1_3
    private void WriteEscaped(ReadOnlySpan<char> str)
    {
      if (str.IsEmpty)
        return;

      int strLen = str.Length;
      int index; // Pointer into the string that indicates the location of the current '&' character
      int newIndex = 0; // Pointer into the string that indicates the start index of the "remaining" string (that still needs to be processed).

      bool foundAnyEscapeChar = false;

      while (true)
      {
        index = str.IndexOfAny(s_escapeChars.AsSpan(newIndex));

        if (index == -1)
        {
          if (!foundAnyEscapeChar)
          {
            this.writer.Write(str);
            return;
          }
          else
          {
            this.writer.Write(str.Slice(newIndex, strLen - newIndex));
            return;
          }
        }
        else
        {
          foundAnyEscapeChar = true;
          this.writer.Write(str.Slice(newIndex, index - newIndex));
          WriteEscapeSequence(str[index]);

          newIndex = (index + 1);
        }
      }
    }
#endif

    private void WriteEscaped(string? str)
    {
      if (str == null)
        return;

      int strLen = str.Length;
      int index; // Pointer into the string that indicates the location of the current '&' character
      int newIndex = 0; // Pointer into the string that indicates the start index of the "remaining" string (that still needs to be processed).

      bool foundAnyEscapeChar = false;

      while (true)
      {
        index = str.IndexOfAny(s_escapeChars, newIndex);

        if (index == -1)
        {
          if (!foundAnyEscapeChar)
          {
            this.writer.Write(str);
          }
          else
          {
            WriteEscaped(str, index, newIndex);
          }
          return;
        }
        else
        {
          foundAnyEscapeChar = true;
#if NETSTANDARD1_3
          if (TryCopyToBuffer(str, newIndex, index - newIndex))
          {
            this.writer.Write(this.buffer, 0, index - newIndex);
          }
          else
          {
            this.writer.Write(str.Substring(newIndex, index - newIndex));
          }
#else
          this.writer.Write(str.AsSpan(newIndex, index - newIndex));
#endif
          WriteEscapeSequence(str[index]);

          newIndex = (index + 1);
        }
      }
    }

    private void WriteEscaped(string str, int index, int newIndex)
    {
#if NETSTANDARD1_3
      if (TryCopyToBuffer(str, newIndex, index - newIndex))
      {
        this.writer.Write(this.buffer, 0, index - newIndex);
      }
      else
      {
        this.writer.Write(str.Substring(newIndex, index - newIndex));
      }
#else
      this.writer.Write(str.AsSpan(newIndex, str.Length - newIndex));
#endif
    }

#if NETSTANDARD1_3
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryCopyToBuffer(string str, int startIndex, int count)
    {
      if (count > this.buffer.Length)
      {
        return false;
      }
      else
      {
        str.CopyTo(startIndex, this.buffer, 0, count);
        return true;
      }
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteEscapeSequence(char c)
    {
      switch (c)
      {
        case '<': this.writer.Write("&lt;"); break;
        case '>': this.writer.Write("&gt;"); break;
        case '\"': this.writer.Write("&quot;"); break;
        case '&': this.writer.Write("&amp;"); break;
        default: this.writer.Write(c); break;
      }
    }
#endregion
  }
}
