package System.Runtime.Caching

import System.NotImplementedException
import System.Collections.DictionaryEntry
import System.Collections.Generic.KeyValuePair
//import com.google.common.cache._
import java.util.concurrent.ConcurrentHashMap

object MemoryCache
{
  //TODO: Use a real caching framework that expires and such.  This is just a stub.
  val _cache = new ConcurrentHashMap[String, Any]();
}

class MemoryCache(id:String) extends Traversable[KeyValuePair[String, Any]]
{
  
  def Set(key:String, value:Any, policy:CacheItemPolicy)
  {
    MemoryCache._cache.put(key, value);
  }
  
  def Remove(key:String)
  {
    MemoryCache._cache.remove(key);
  }
  
  def Get(key:String):Any =
  {
    return MemoryCache._cache.get(key);
  }
  def apply(key:String):Any =
  {
    return MemoryCache._cache.get(key);
  }
  
  def foreach[U](fn:KeyValuePair[String, Any]=>U)
  {
    val it = MemoryCache._cache.entrySet().iterator();
    while (it.hasNext())
    {
      val e = it.next();
      fn(new KeyValuePair(e.getKey(), e.getValue()));
    }
  }

}