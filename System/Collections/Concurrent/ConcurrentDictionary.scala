package System.Collections.Concurrent

import System.CsRef
import System.NotImplementedException
import java.util.concurrent.ConcurrentHashMap
import scala.collection.JavaConverters._

class ConcurrentDictionary[K, V](_map: ConcurrentHashMap[K, V]) {
  def this() {
    this(new ConcurrentHashMap[K, V]());
  }
  def TryGetValue(k: K, out: CsRef[V]): Boolean =
    {
      out.Value = _map.get(k);
      return out.Value != null;
    }

  def TryRemove(k: K, out: CsRef[V]): Boolean =
    {
      out.Value = _map.remove(k);
      return out.Value != null;
    }

  def GetOrAdd(k: K, add: K => V): V =
    {
      var ret = _map.get(k);
      if (ret != null)
        return ret;
      else
      {
        ret = add(k);
        val prev = _map.putIfAbsent(k, ret);
        if (prev != null)
          return prev;
        else
          return ret;
      }
    }
  def GetOrAdd(k: K, add: V): V =
    {
      val r = _map.putIfAbsent(k, add);
      if (r == null)
        return add;
      else
        return r;
    }

  def AddOrUpdate(k: K, add: V, update: (K, V) => V) 
  {
    val item = _map.putIfAbsent(k, add);
    
    if (item != null)
      _map.put(k, update(k, item));
  }

  def Keys: Traversable[K] = {
    return _map.keySet().asScala;
  }

  def Clear() {
    _map.clear();
  }

}