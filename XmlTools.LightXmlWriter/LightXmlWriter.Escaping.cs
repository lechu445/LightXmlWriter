using System;

namespace XmlTools
{
  /// <summary>
  /// Contains methods to write escaped values.
  /// </summary>
  public sealed partial class LightXmlWriter
  {
    private static readonly char[] EscapeChars = new[] { '<', '>', '\"', '\'', '&' };
    private static readonly char[] EscapeCharsForValue = new[] { '<', '>', '\'', '&' };

    private void WriteEscaped(ReadOnlySpan<char> str, bool escapeValue)
    {
      if (str.IsEmpty)
      {
        return;
      }

      int index; // Pointer into the string that indicates the location of the current '&' character
      int newIndex = 0; // Pointer into the string that indicates the start index of the "remaining" string (that still needs to be processed).

      char[] escapeChars = escapeValue ? EscapeCharsForValue : EscapeChars;

      while (true)
      {
        str = str.Slice(newIndex);
        index = str.IndexOfAny(escapeChars);

        if (index == -1)
        {
          WriteSpan(str);
          return;
        }
        else
        {
          WriteSpan(str.Slice(0, index));
          if (escapeValue)
          {
            WriteEscapeSequenceForValue(str[index]);
          }
          else
          {
            WriteEscapeSequence(str[index]);
          }

          newIndex = index + 1;
        }
      }
    }

    private void WriteEscaped(string? str, bool escapeValue)
    {
      if (str == null)
      {
        return;
      }

      int index; // Pointer into the string that indicates the location of the current '&' character
      int newIndex = 0; // Pointer into the string that indicates the start index of the "remaining" string (that still needs to be processed).

      bool foundAnyEscapeChar = false;

      char[] escapeChars = escapeValue ? EscapeCharsForValue : EscapeChars;

      while (true)
      {
        index = str.IndexOfAny(escapeChars, newIndex);

        if (index == -1)
        {
          if (!foundAnyEscapeChar)
          {
            this.writer.Write(str);
          }
          else
          {
            WriteEscaped(str, newIndex);
          }

          return;
        }
        else
        {
          foundAnyEscapeChar = true;

          WriteSpan(str.AsSpan(newIndex, index - newIndex));

          if (escapeValue)
          {
            WriteEscapeSequenceForValue(str[index]);
          }
          else
          {
            WriteEscapeSequence(str[index]);
          }

          newIndex = index + 1;
        }
      }
    }

    private void WriteEscaped(string str, int newIndex)
    {
      WriteSpan(str.AsSpan(newIndex, str.Length - newIndex));
    }

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

    private void WriteEscapeSequenceForValue(char c)
    {
      switch (c)
      {
        case '<': this.writer.Write("&lt;"); break;
        case '>': this.writer.Write("&gt;"); break;
        case '&': this.writer.Write("&amp;"); break;
        default: this.writer.Write(c); break;
      }
    }
  }
}
