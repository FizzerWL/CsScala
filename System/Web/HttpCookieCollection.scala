package System.Web;
import java.util.HashMap
import scala.collection.JavaConverters._

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
  
  def iterator: Iterator[String] = _map.keySet.iterator().asScala;
  
  def AllKeys:Array[String] = _map.keySet().toArray(new Array[String](0));

}