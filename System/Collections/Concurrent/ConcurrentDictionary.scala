package System.Collections.Concurrent

import System.CsRef
import java.util.concurrent.ConcurrentHashMap
import scala.collection.JavaConverters._
import System.Collections.Generic.KeyValuePair

class ConcurrentDictionary[K, V](_map: ConcurrentHashMap[K, V]) extends Traversable[KeyValuePair[K, V]] {
  def this() {
    this(new ConcurrentHashMap[K, V]());
  }

  def foreach[U](fn: KeyValuePair[K, V] => U) {
    val it = _map.entrySet().iterator();
    while (it.hasNext()) {
      val e = it.next();
      fn(new KeyValuePair(e.getKey(), e.getValue()));
    }
  }

  def TryGetValue(k: K, out: CsRef[V]): Boolean =
    {
      out.Value = _map.get(k);
      return out.Value != null;
    }

  def ContainsKey(k: K): Boolean = _map.containsKey(k);

  def TryRemove(k: K, out: CsRef[V]): Boolean =
    {
      out.Value = _map.remove(k);
      return out.Value != null;
    }
  
  def Remove(k:K):Boolean = _map.remove(k) != null;

  def TryAdd(key: K, value: V): Boolean = _map.putIfAbsent(key, value) == null;

  def GetOrAdd(k: K, add: K => V): V =
    {
      var ret = _map.get(k);
      if (ret != null)
        return ret;
      else {
        //TODO: We should do a lock here, since we could call add and throw away its result.  If add has side-effects, this could be an issue
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

  def AddOrUpdate(k: K, add: V, update: (K, V) => V) {
    val item = _map.putIfAbsent(k, add);

    if (item != null)
      _map.put(k, update(k, item));
  }

  def Keys: Traversable[K] = {
    return _map.keySet().asScala;
  }
  def Values: Traversable[V] = {
    return _map.values().asScala;
  }

  def Clear() {
    _map.clear();
  }

  def Count: Int = _map.size();

}