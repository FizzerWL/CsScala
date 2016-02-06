package System.Net;
import java.util.HashMap

class CookieContainer {
  val _cookies = new HashMap[String, Cookie]();

  def apply(name: String): Cookie = {
    return _cookies.get(name);
  }
  
  def Add(c:Cookie) {
    _cookies.put(c.Domain, c);
  }

  def AddRaw(rawStr: String) {
    val split = rawStr.split(';');
    val name = split(0).split('=');
    _cookies.put(name(0), new Cookie(name(0), name(1)));
  }
  
  def Count:Int = _cookies.size();
}