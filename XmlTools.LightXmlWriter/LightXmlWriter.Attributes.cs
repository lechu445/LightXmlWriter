using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace XmlTools
{
  /// <summary>
  /// Contains methods to write attributes.
  /// </summary>
  public sealed partial class LightXmlWriter
  {
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteStartAttributeImpl(string name)
    {
      this.writer.Write(' ');
      this.writer.Write(name);
      this.writer.Write('=');
      this.writer.Write('"');
    }
  }
}
