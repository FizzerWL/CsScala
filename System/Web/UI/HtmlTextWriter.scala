package System.Web.UI

import System.DateTime
import System.IO.TextWriter
import java.io.Writer
import java.io.StringWriter
import System.CsScala
import System.TimeSpan

class HtmlTextWriter(writer:StringWriter) extends TextWriter(writer) 
{
  def this(estimate:Int)
  {
    this(new StringWriter(estimate))
  }
  
  def Write(s:Int) { writer.append(s.toString()); }
  def Write(s:java.lang.Integer) { writer.append(CsScala.NullableToString(s)); }
  def Write(s:java.lang.Float) { writer.append(CsScala.NullableToString(s)); }
  def Write(s:java.lang.Double) { writer.append(CsScala.NullableToString(s)); }
  def Write(s:Long) { writer.append(s.toString()); }

  def Write(str:String)
  {
    if (str != null)
      writer.append(str);
  }
  
  def Write(d:DateTime)
  {
    if (d != null)
    	writer.append(d.toString());
  }
  def Write(t:TimeSpan)
  {
    if (t != null)
      writer.append(t.toString());
  }
  
  def Write(b:Boolean)
  {
    writer.append(if (b) "true" else "false");
  }
  
  def WriteLine() {  writer.append("\n"); }
  def WriteLine(s:String) { Write(s); WriteLine(); }
  def Position():Int = writer.getBuffer().length;
  def GetSubstring(start:Int):String = writer.getBuffer().substring(start);
  def ToString():String = writer.toString();
}
