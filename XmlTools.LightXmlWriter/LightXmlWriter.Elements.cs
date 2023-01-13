using System;
using System.Diagnostics;

namespace XmlTools
{
  /// <summary>
  /// Contains methods to write elements.
  /// </summary>
  public sealed partial class LightXmlWriter
  {
    /// <summary>
    /// Writes out a start tag with the specified local name.
    /// </summary>
    /// <param name="name">The local name of the element.</param>
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

    /// <summary>
    /// Writes out the specified start tag and associates it with the given namespace.
    /// </summary>
    /// <param name="name">The local name of the element.</param>
    /// <param name="ns">The namespace URI to associate with the element.</param>
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
  }
}
