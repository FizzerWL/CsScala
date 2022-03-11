package System;

import java.net.URL

object Uri { 
  def TryCreate(str:String, kind:Int, out:CsRef[Uri]):Boolean = {
    try {
      out.Value = new Uri(str);
      return true;
      
    } catch {
      case ex: java.net.MalformedURLException => return false;
    }
    
  }
    
}

class Uri(str: String) {
  val _url = new URL(str);

  override def toString(): String = _url.toString()

  def Query: String = {
    val q = _url.getQuery();
    if (q == null || q.length() == 0)
      return "";
    else
      return "?" + q;
  }
  
  def LocalPath:String = _url.getPath();
  def PathAndQuery:String = _url.getPath() + Query;
  def Host:String = _url.getHost();
  def Authority:String = _url.getAuthority();
  def Scheme:String = _url.getProtocol();
  
  override def equals(other:Any):Boolean = 
  {
    if (!other.isInstanceOf[Uri])
      return false;
    return other.asInstanceOf[Uri]._url.equals(_url);
  }
  override def hashCode():Int = _url.hashCode();
}