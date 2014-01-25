package System;

import java.net.URL

class Uri(str: String) {
  val _url = new URL(str);

  override def toString(): String = _url.toString()

  def Query: String = _url.getQuery();
  def Host:String = _url.getHost();
  def PathAndQuery:String = 
    {
      val q = _url.getQuery();
      if (q == null)
        return _url.getPath();
      else
        return _url.getPath() + "?" + q;
    }
  def Authority:String = _url.getAuthority();
  
  override def equals(other:Any):Boolean = 
  {
    if (!other.isInstanceOf[Uri])
      return false;
    return other.asInstanceOf[Uri]._url.equals(_url);
  }
  override def hashCode():Int = _url.hashCode();
}