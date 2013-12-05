package System.Web;
import System.Collections.Generic.Dictionary
import System.NotImplementedException
import java.util.HashMap

object HttpContext {

  final val _threadContext = new ThreadLocal[HttpContext]();
  
  def Current: HttpContext = _threadContext.get();
  def Current_=(v:HttpContext) = _threadContext.set(v);
}

class HttpContext 
{

  var Request = new HttpRequest();
  var Response = new HttpResponse();
  var Items = new HttpItems();
}

class HttpItems {
  final val _hash = new HashMap[String, Any]();
  
  def Contains(key:String):Boolean = _hash.containsKey(key)
  def Add(key:String, value:Any) { _hash.put(key, value); }
  
  def apply(key:String):Any = _hash.get(key);
}