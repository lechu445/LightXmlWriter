using System;
using System.Runtime.CompilerServices;

namespace XmlTools
{
  /// <summary>
  /// Contains methods to write escaped values.
  /// </summary>
  public sealed partial class LightXmlWriter
  {
    private static readonly char[] EscapeChars = new[] { '<', '>', '\"', '\'', '&' };

#if !NETSTANDARD1_3
    private void WriteEscaped(ReadOnlySpan<char> str)
    {
      if (str.IsEmpty)
      {
        return;
      }

      int strLen = str.Length;
      int index; // Pointer into the string that indicates the location of the current '&' character
      int newIndex = 0; // Pointer into the string that indicates the start index of the "remaining" string (that still needs to be processed).

      bool foundAnyEscapeChar = false;

      while (true)
      {
        index = str.IndexOfAny(EscapeChars.AsSpan(newIndex));

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

          newIndex = index + 1;
        }
      }
    }
#endif

    private void WriteEscaped(string? str)
    {
      if (str == null)
      {
        return;
      }

      int strLen = str.Length;
      int index; // Pointer into the string that indicates the location of the current '&' character
      int newIndex = 0; // Pointer into the string that indicates the start index of the "remaining" string (that still needs to be processed).

      bool foundAnyEscapeChar = false;

      while (true)
      {
        index = str.IndexOfAny(EscapeChars, newIndex);

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

          newIndex = index + 1;
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
  }
}
