package System.Web.UI

import System.DateTime

class HtmlTextWriter(est:Int) {
  
  val sb = new StringBuilder(est);

  def Write(str:String)
  {
    sb.append(str);
  }
  def Write(s:Int)
  {
    sb.append(s);
  }
  def Write(s:Long)
  {
    sb.append(s);
  }
  
  def Write(d:DateTime)
  {
    if (d != null)
    	sb.append(d.toString());
  }
  
  def Write(b:Boolean)
  {
    sb.append(b);
  }
  
  def WriteLine()
  {
    sb.append("\n");
  }
  
  def ToString():String = sb.toString();
}
