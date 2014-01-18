package System.Web.UI

import System.DateTime
import System.IO.TextWriter
import java.io.Writer
import java.io.StringWriter

class HtmlTextWriter(writer:StringWriter) extends TextWriter(writer) 
{
  def this(estimate:Int)
  {
    this(new StringWriter(estimate))
  }
  

  def Write(str:String)
  {
    if (str != null)
      writer.append(str);
  }
  def Write(s:Int)
  {
    writer.append(s.toString());
  }
  def Write(s:Long)
  {
    writer.append(s.toString());
  }
  
  def Write(d:DateTime)
  {
    if (d != null)
    	writer.append(d.toString());
  }
  
  def Write(b:Boolean)
  {
    writer.append(if (b) "true" else "false");
  }
  
  def WriteLine()
  {
    writer.append("\n");
  }
  
  def Position():Int = writer.getBuffer().length;
  def GetSubstring(start:Int):String = writer.getBuffer().substring(start);
  def ToString():String = writer.toString();
}
