using System;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write escaped values.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    private static readonly char[] EscapeChars = new[] { '<', '>', '\"', '\'', '&' };
    private static readonly char[] EscapeCharsForValue = new[] { '<', '>', '\'', '&' };

    private static ReadOnlySpan<byte> lt => new byte[] { (byte)'&', (byte)'l', (byte)'t', (byte)';' };
    private static ReadOnlySpan<byte> gt => new byte[] { (byte)'&', (byte)'g', (byte)'t', (byte)';' };
    private static ReadOnlySpan<byte> quot => new byte[] { (byte)'&', (byte)'q', (byte)'u', (byte)'o', (byte)'t', (byte)';' };
    private static ReadOnlySpan<byte> amp => new byte[] { (byte)'&', (byte)'a', (byte)'m', (byte)'p', (byte)';' };

    private void WriteEscaped(ReadOnlySpan<char> str, bool escapeValue)
    {
      if (escapeValue)
      {
        foreach (var ch in str)
        {
          WriteEscapeSequenceForValue(ch);
        }
      }
      else
      {
        foreach (var ch in str)
        {
          WriteEscapeSequence(ch);
        }
      }
    }

    private void WriteEscapeSequence(char c)
    {
      switch (c)
      {
        case '<': this.stream.Write(lt); break;
        case '>': this.stream.Write(gt); break;
        case '\"': this.stream.Write(quot); break;
        case '&': this.stream.Write(amp); break;
        default: this.stream.WriteByte((byte)c); break;
      }
    }

    private void WriteEscapeSequenceForValue(char c)
    {
      switch (c)
      {
        case '<': this.stream.Write(lt); break;
        case '>': this.stream.Write(gt); break;
        case '&': this.stream.Write(amp); break;
        default: this.stream.WriteByte((byte)c); break;
      }
    }
  }
}
