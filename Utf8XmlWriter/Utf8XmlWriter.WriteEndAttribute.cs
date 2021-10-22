using System;

namespace Tools.Text.Xml
{
  /// <summary>
  /// Contains methods to write values.
  /// </summary>
  public sealed partial class Utf8XmlWriter
  {
    public void WriteEndAttribute(string name)
    {
      WriteEndAttribute();
    }

    public void WriteEndAttribute(ReadOnlySpan<char> name)
    {
      WriteEndAttribute();
    }

    public void WriteEndAttribute(XmlEncodedName name)
    {
      WriteEndAttribute();
    }

    public void WriteEndAttribute()
    {
      this.stream.WriteByte((byte)'"');
      this.writingAttribute = false;
    }
  }
}
