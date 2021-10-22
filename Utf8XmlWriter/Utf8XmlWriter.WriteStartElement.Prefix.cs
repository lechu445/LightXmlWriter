using XmlTools.Helpers;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write elements.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    /// <summary>
    /// Writes out the specified start tag and associates it with the given namespace.
    /// </summary>
    /// <param name="name">The local name of the element.</param>
    /// <param name="ns">The namespace URI to associate with the element.</param>
    public void WriteStartElement(string name, string? ns)
    {
      if (this.writingElement)
      {
        this.stream.WriteByte((byte)'>');
      }

      this.stream.WriteByte((byte)'<');
      this.stream.WriteUtf8EncodedString(name);
      if (ns != null)
      {
        this.stream.WriteUtf8EncodedString(" xmlns=\"");
        this.stream.WriteUtf8EncodedString(ns);
        this.stream.WriteByte((byte)'"');
      }

      this.writingElement = true;
      this.valueWritten = false;
    }

    // Writes out the specified start tag and associates it with the given namespace and prefix.
    public void WriteStartElement(string? prefix, string name, string? ns)
    {
      if (this.writingElement)
      {
        this.stream.WriteByte((byte)'>');
      }

      this.stream.WriteByte((byte)'<');
      if (prefix != null)
      {
        this.stream.WriteUtf8EncodedString(prefix);
        this.stream.WriteByte((byte)':');
      }

      this.stream.WriteUtf8EncodedString(name);

      if (ns != null && prefix != null)
      {
        this.stream.WriteUtf8EncodedString(" xmlns:");
        this.stream.WriteUtf8EncodedString(prefix);
        this.stream.WriteByte((byte)'=');
        this.stream.WriteByte((byte)'"');
        this.stream.WriteUtf8EncodedString(ns);
        this.stream.WriteByte((byte)'"');
      }

      this.writingElement = true;
      this.valueWritten = false;
    }
  }
}
