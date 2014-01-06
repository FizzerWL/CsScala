package System.Web;
import System.NotImplementedException;
import java.util.HashMap

class HttpCookieCollection extends Traversable[HttpCookie]
{
  
  final val _map = new HashMap[String, HttpCookie]();

  def apply(name: String): HttpCookie =
    {
      return _map.get(name);
    }

  def Remove(name: String) {
    _map.remove(name);
  }
  def Add(cookie: HttpCookie) {
    _map.put(cookie.Name, cookie);
  }
  
  def foreach[U](fn:HttpCookie=>U)
  {
    val it = _map.keySet().iterator();
    while (it.hasNext())
      fn(_map.get(it.next()));
  }

}