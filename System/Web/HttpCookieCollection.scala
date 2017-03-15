package System.Web;
import System.NotImplementedException;
import java.util.HashMap

class HttpCookieCollection extends Traversable[String]
{
  
  final val _map = new HashMap[String, HttpCookie]();

  def apply(name: String): HttpCookie = _map.get(name);

  def Remove(name: String) {
    _map.remove(name);
  }
  def Add(cookie: HttpCookie) {
    _map.put(cookie.Name, cookie);
  }
  
  def foreach[U](fn:String=>U)
  {
    val it = _map.keySet().iterator();
    while (it.hasNext())
      fn(_map.get(it.next()).Name);
  }
  
  def AllKeys:Array[String] = _map.keySet().toArray(new Array[String](0));

}