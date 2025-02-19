package System.Text

import java.io.StringWriter
import System.CsScala

class StringBuilder(sb: java.lang.StringBuilder) {

  def this(capacity: Int) = {
    this(new java.lang.StringBuilder(capacity));
  }
  def this(initial: String) = {
    this(new java.lang.StringBuilder(initial));
  }
  def this() = {
    this(new java.lang.StringBuilder());
  }

  private var _writer: StringWriter = null;
  def SetStringWriter(writer: StringWriter):Unit = {
    _writer = writer;
  }

  def Append(c: Char):StringBuilder = {
    sb.append(c);
    return this;
  }
  def Append(s: String):StringBuilder =  {
    if (s != null)
      sb.append(s);
    
    return this;
  }
  def AppendLine(s: String = null):StringBuilder =  {
    if (s != null)
      sb.append(s);

    sb.append('\n');
    return this;
  }
  def Append(i: Int):StringBuilder =  {
    sb.append(i);
    return this;
  }
  def Append(i: Double):StringBuilder =  {
    sb.append(i);
    return this;
  }
  def Append(s: String, offset: Int, len: Int):StringBuilder =  {
    if (s != null)
      sb.append(s, offset, offset + len);
    
    return this;
  }
  
  def Append(c:Char, repeatCount:Int):StringBuilder = {
    for (i <- 1 to repeatCount)
      sb.append(c);
    
    return this;
  }

  def Insert(index: Int, c: Char):StringBuilder =  {
    sb.insert(index, c);
    return this;
  }
  def Insert(index: Int, s: String):StringBuilder =  {
    if (s != null)
      sb.insert(index, s);
    
    return this;
  }
  def Insert(index: Int, value: String, count:Int):StringBuilder =  {
    var remain = count;
    while (remain > 0)
    {
      sb.insert(index, value);
      remain -= 1;
    }
    return this;
  }

  def Length: Int = sb.length();
   
  override def toString(): String =
    {
      if (_writer == null)
        return sb.toString();
      else
        return _writer.toString();
    }

  def Clear():Unit = {
    sb.setLength(0);
  }

  def Remove(startAt: Int, count: Int):Unit = {
    sb.delete(startAt, startAt + count);
  }
  def Replace(from: String, to: String): StringBuilder =
    {
      var index = sb.indexOf(from);
      while (index != -1) {
        sb.replace(index, index + from.length(), to);
        index += to.length();
        index = sb.indexOf(from, index);
      }
      return this;
    }

  def apply(i: Int): Char = sb.charAt(i);
  def update(i: Int, ch: Char):Unit = {
    sb.setCharAt(i, ch);
  }
  
  def Capacity:Int = 0;
  def Capacity_=(value:Int):Unit = { }
}