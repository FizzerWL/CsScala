package System.Net;
import System.NotImplementedException;
import java.util.HashMap

class CookieContainer {
  private val _cookies = new HashMap[String, Cookie]();

  def apply(name: String): Cookie =
    {
      return _cookies.get(name);
    }
  
  def AddRaw(rawStr:String)
  {
    val split = rawStr.split(';');
    val name = split(0).split('=');
    _cookies.put(name(0), new Cookie(name(0), name(1)));
  }
}