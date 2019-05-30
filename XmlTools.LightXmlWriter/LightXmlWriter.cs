﻿using System;
using System.IO;

namespace XmlTools
{
  /// <summary>
  /// Light implementation of XmlWriter equivalent designed to be as close as 
  /// possible of XmlWriter usage & behaviour with most common settings (no pretty-print, no xml declaration, etc.)
  /// </summary>
  public class LightXmlWriter : IDisposable
  {
    private readonly TextWriter writer;
    private bool writingStartElement = false;
    private bool writingAttribute = false;
    private bool valueWritten = false;

    public LightXmlWriter(TextWriter writer)
    {
      this.writer = writer;
    }

    public TextWriter Writer => this.writer;

    // Writes out a start tag with the specified local name with no namespace.
    public void WriteStartElement(string name)
    {
      if (this.writingStartElement)
      {
        this.writer.Write('>');
      }
      this.writer.Write('<');
      this.writer.Write(name);
      this.writingStartElement = true;
      this.valueWritten = false;
    }

    // Writes out the specified start tag and associates it with the given namespace.
    public void WriteStartElement(string name, string ns)
    {
      if (this.writingStartElement)
      {
        this.writer.Write('>');
      }
      this.writer.Write('<');
      this.writer.Write(name);
      if (ns != null)
      {
        this.writer.Write(" xmlns=\"");
        WriteXmlString(ns, escape: false);
        this.writer.Write('"');
      }
      this.writingStartElement = true;
      this.valueWritten = false;
    }

    // Writes out the specified start tag and associates it with the given namespace and prefix.
    public void WriteStartElement(string prefix, string name, string ns)
    {
      if (this.writingStartElement)
      {
        this.writer.Write('>');
      }
      this.writer.Write('<');
      this.writer.Write(prefix);
      this.writer.Write(':');
      this.writer.Write(name);

      if (ns != null)
      {
        this.writer.Write(" xmlns:");
        this.writer.Write(prefix);
        this.writer.Write('=');
        this.writer.Write('"');
        WriteXmlString(ns, escape: false);
        this.writer.Write('"');
      }

      this.writingStartElement = true;
      this.valueWritten = false;
    }

    // Closes one element of specified tag name.
    public void WriteEndElement(string name)
    {
      if (this.valueWritten)
      {
        if (this.writingStartElement)
        {
          this.writer.Write('>');
        }
        this.writer.Write('<');
        this.writer.Write('/');
        this.writer.Write(name);
      }
      else if (this.writingStartElement)
      {
        this.writer.Write('/');
      }
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingStartElement = false;
    }

    // Closes one element of specified tag name and prefix.
    public void WriteEndElement(string prefix, string name)
    {
      if (this.valueWritten)
      {
        if (this.writingStartElement)
        {
          this.writer.Write('>');
        }
        this.writer.Write('<');
        this.writer.Write('/');
        this.writer.Write(prefix);
        this.writer.Write(':');
        this.writer.Write(name);
      }
      else if (this.writingStartElement)
      {
        this.writer.Write('/');
      }
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingStartElement = false;
    }

    public void WriteElement(string name, string value, bool escapeValue = true)
    {
      WriteElementString(name, value, escapeValue);
    }

    public void WriteElementString(string name, string value, bool escapeValue = true)
    {
      WriteStartElement(name);
      if (value == null)
      {
        WriteEndElement(name);
      }
      else
      {
        this.writer.Write('>');
        WriteXmlString(value, escapeValue);
        this.writer.Write('<');
        this.writer.Write('/');
        this.writer.Write(name);
        this.writer.Write('>');
        this.valueWritten = true;
      }
      this.writingStartElement = false;
    }

    public void WriteElementString(string prefix, string name, string ns, string value, bool escapeValue = true)
    {
      WriteStartElement(prefix, name, ns);
      this.writer.Write('>');
      WriteXmlString(value, escapeValue);
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(prefix);
      this.writer.Write(':');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingStartElement = false;
    }

    public void WriteElement(string name, int value)
    {
      WriteStartElement(name);
      this.writer.Write('>');
#if NETCOREAPP2_1 || NETCOREAPP2_2
      if (value < 0)
      {
        this.writer.Write('-');
        value = -value;
      }
      int i = 0;
      Span<char> chars = stackalloc char[10];
      while (value != 0)
      {
        chars[i] = (char)('0' + value % 10);
        value /= 10;
        i++;
      }
      for (int j = i - 1; j >= 0; j--)
      {
        this.writer.Write(chars[j]);
      }
#else
      this.writer.Write(value);
#endif
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingStartElement = false;
    }

    public void WriteElement(string name, double value)
    {
      WriteStartElement(name);
      this.writer.Write('>');
      WriteValue(value);
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingStartElement = false;
    }

    public void WriteElement(string name, char value)
    {
      WriteStartElement(name);
      this.writer.Write('>');
      WriteValue(value);
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingStartElement = false;
    }

    public void WriteElement(string name, bool value)
    {
      WriteStartElement(name);
      this.writer.Write('>');
      WriteValue(value);
      this.writer.Write('<');
      this.writer.Write('/');
      this.writer.Write(name);
      this.writer.Write('>');
      this.valueWritten = true;
      this.writingStartElement = false;
    }

    public void WriteStartAttribute(string name)
    {
      WriteStartAttributeImpl(name);
      this.writingAttribute = true;
    }

    public void WriteStartAttribute(string prefix, string name)
    {
      this.writer.Write(' ');
      this.writer.Write(prefix);
      this.writer.Write(':');
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

    public void WriteAttributeString(string prefix, string name, string ns, string value, bool escapeValue = true)
    {
      WriteStartAttribute(prefix, name);
      WriteXmlString(value, escapeValue);
      WriteEndAttribute();
      if (!string.IsNullOrEmpty(ns))
      {
        this.writer.Write(" xmlns:");
        this.writer.Write(prefix);
        this.writer.Write('=');
        this.writer.Write('"');
        this.writer.Write(ns);
        this.writer.Write('"');
      }
    }

    public void WriteAttributeString(string name, string value, bool escapeValue = true)
    {
      WriteAttribute(name, value, escapeValue);
    }

    public void WriteAttribute(string name, string value, bool escapeValue = true)
    {
      WriteStartAttributeImpl(name);
      WriteXmlString(value, escapeValue);
      this.writer.Write('"');
    }

    public void WriteAttribute(string name, int value)
    {
      WriteStartAttributeImpl(name);

#if NETCOREAPP2_1 || NETCOREAPP2_2
      if (value < 0)
      {
        this.writer.Write('-');
        value = -value;
      }
      int i = 0;
      Span<char> chars = stackalloc char[10];
      while (value != 0)
      {
        chars[i] = (char)('0' + value % 10);
        value /= 10;
        i++;
      }
      for (int j = i - 1; j >= 0; j--)
      {
        this.writer.Write(chars[j]);
      }
#else
      this.writer.Write(value);
#endif
      this.writer.Write('"');
    }

    public void WriteAttribute(string name, char value)
    {
      WriteStartAttributeImpl(name);
      this.writer.Write(value);
      this.writer.Write('"');
    }

    public void WriteAttribute(string name, double value)
    {
      WriteStartAttributeImpl(name);
      this.writer.Write(value);
      this.writer.Write('"');
    }

    public void WriteAttribute(string name, decimal value)
    {
      WriteStartAttributeImpl(name);
      this.writer.Write(value);
      this.writer.Write('"');
    }

    public void WriteAttribute(string name, bool value)
    {
      WriteStartAttributeImpl(name);
      this.writer.Write(value);
      this.writer.Write('"');
    }

    public void WriteRaw(string value)
    {
      WriteValue(value, escape: false);
    }

    public void WriteString(string value, bool escape = true)
    {
      WriteValue(value, escape);
    }

    public void WriteValue(string value, bool escape = true)
    {
      if (this.writingAttribute)
      {
        WriteXmlString(value, escape);
        return;
      }
      if (this.writingStartElement)
      {
        this.writer.Write('>');
        writingStartElement = false;
      }
      WriteXmlString(value, escape);
      this.valueWritten = true;
    }

    public void WriteValue(int value)
    {
      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(double value)
    {
      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(decimal value)
    {
      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(char value)
    {
      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(long value)
    {
      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(bool value)
    {
      this.writer.Write(value);
      this.valueWritten = true;
    }

    public void WriteValue(DateTime value)
    {
      this.writer.Write(value);
      this.valueWritten = true;
    }

#if NETCOREAPP2_1 || NETCOREAPP2_2
    public void WriteSpanValue(ReadOnlySpan<char> value)
    {
      this.writer.Write(value);
      this.valueWritten = true;
    }
#endif

    public void Dispose()
    {
      this.writer?.Dispose();
    }

    private void WriteStartAttributeImpl(string name)
    {
      this.writer.Write(' ');
      this.writer.Write(name);
      this.writer.Write('=');
      this.writer.Write('"');
    }

    private void WriteXmlString(string value, bool escape = true)
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

    #region WriteEscaped(value)
    // This is port of SecurityElement.Escape(value) that writes directly into this.writer instead of string creation

    private static readonly char[] s_escapeChars = new char[] { '<', '>', '\"', '\'', '&' };
    private static readonly string[] s_escapeStringPairs = new string[]
    {
      // these must be all once character escape sequences or a new escaping algorithm is needed
      "<", "&lt;",
      ">", "&gt;",
      "\"", "&quot;",
      "'", "&apos;",
      "&", "&amp;"
    };

    private void WriteEscaped(string str)
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
            return;
          }
          else
          {
#if NETCOREAPP2_1 || NETCOREAPP2_2
            this.writer.Write(str.AsSpan(newIndex, strLen - newIndex));
#else
            this.writer.Write(str.Substring(newIndex, strLen - newIndex));
#endif
            return;
          }
        }
        else
        {
          foundAnyEscapeChar = true;
#if NETCOREAPP2_1 || NETCOREAPP2_2
          this.writer.Write(str.AsSpan(newIndex, index - newIndex));
#else
          this.writer.Write(str.Substring(newIndex, index - newIndex));
#endif
          WriteEscapeSequence(str[index]);

          newIndex = (index + 1);
        }
      }
    }

    private void WriteEscapeSequence(char c)
    {
      int iMax = s_escapeStringPairs.Length;

      for (int i = 0; i < iMax; i += 2)
      {
        string strEscSeq = s_escapeStringPairs[i];

        if (strEscSeq[0] == c)
        {
          this.writer.Write(s_escapeStringPairs[i + 1]);
          return;
        }
      }
      this.writer.Write(c);
    }
    #endregion
  }
}
