package System.Web;
import java.util.HashMap

class HttpCookieCollection extends Iterable[String]
{
  
  final val _map = new HashMap[String, HttpCookie]();

  def apply(name: String): HttpCookie = _map.get(name);

  def Remove(name: String):Unit = {
    _map.remove(name);
  }
  def Add(cookie: HttpCookie):Unit = {
    _map.put(cookie.Name, cookie);
  }
  
  def iterator: Iterator[String] = new Iterator[String] {
    private val it = _map.entrySet().iterator();
    override def hasNext: Boolean = it.hasNext()
    override def next(): String = _map.get(it.next()).Name;
  }  
  
  def AllKeys:Array[String] = _map.keySet().toArray(new Array[String](0));

}