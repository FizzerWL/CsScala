package System.Web;
import System.Collections.Generic.Dictionary
import java.util.HashMap
import System.Web.SessionState.HttpSessionState
import System.Web.UI.Page

object HttpContext {

  final val _threadContext = new ThreadLocal[HttpContext]();
  
  def Current: HttpContext = _threadContext.get();
  def Current_=(v:HttpContext) = _threadContext.set(v);
}

class HttpContext
{

  final val Request = new HttpRequest();
  final val Response = new HttpResponse();
  final val Items = new HttpItems();
  final val Session = new HttpSessionState(this);
  final var Log:StringBuffer = null;
  
  override def toString():String = Request.toString();
}

class HttpItems {
  final val _hash = new HashMap[String, Any]();
  
  def Contains(key:String):Boolean = _hash.containsKey(key)
  def Add(key:String, value:Any) { _hash.put(key, value); }
  def Remove(key:String) { _hash.remove(key); }
  
  def apply(key:String):Any = _hash.get(key);
  def update(key:String, value:Any) { _hash.put(key, value); }
}