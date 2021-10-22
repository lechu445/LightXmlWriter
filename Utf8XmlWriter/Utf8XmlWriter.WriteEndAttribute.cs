using System;
using System.Runtime.CompilerServices;

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

    public void WriteEndAttribute(XmlExcapedName name)
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
