package System.Web.UI

import System.DateTime
import System.IO.TextWriter
import java.io.Writer
import java.io.StringWriter
import System.CsScala
import System.TimeSpan

class HtmlTextWriter(writer:StringWriter) extends TextWriter(writer) 
{
  def this(estimate:Int) =
  {
    this(new StringWriter(estimate))
  }
  
  def Write(s:Int):Unit = { writer.append(s.toString()); }
  def Write(s:java.lang.Integer):Unit = { writer.append(CsScala.NullableToString(s)); }
  def Write(s:java.lang.Float):Unit = { writer.append(CsScala.NullableToString(s)); }
  def Write(s:java.lang.Double):Unit = { writer.append(CsScala.NullableToString(s)); }
  def Write(s:Long):Unit = { writer.append(s.toString()); }

  def Write(str:String) =
  {
    if (str != null)
      writer.append(str);
  }
  
  def Write(d:DateTime) =
  {
    if (d != null)
    	writer.append(d.toString());
  }
  def Write(t:TimeSpan) =
  {
    if (t != null)
      writer.append(t.toString());
  }
  
  def Write(b:Boolean) =
  {
    writer.append(if (b) "true" else "false");
  }
  
  def WriteLine():Unit = {  writer.append("\n"); }
  def WriteLine(s:String):Unit = { Write(s); WriteLine(); }
  def Position():Int = writer.getBuffer().length;
  def GetSubstring(start:Int):String = writer.getBuffer().substring(start);
  def ToString():String = writer.toString();
}
