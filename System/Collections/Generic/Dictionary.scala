package System.Collections.Generic

import java.util.HashMap
import System.CsRef

class Dictionary[K, V](initialMap:HashMap[K,V]) extends Iterable[KeyValuePair[K,V]] 
{
  val _map = initialMap;
  
  
  def this(initialCount:Int) =
  {
    this(new HashMap[K,V](initialCount));
  }
  
  def this() =
  {
      this(new HashMap[K,V]());
  }
  
  def this(other:Dictionary[K,V]) =
  {
    this(other.Count);
    for(pair <- other)
      Add(pair.Key, pair.Value);
  }
  
  def Add(k:K, v:V) =
  {
    if (ContainsKey(k))
      throw new Exception("Key already exists");
    _map.put(k, v);
  }
  
  def ContainsKey(k:K):Boolean = 
  {
    return _map.containsKey(k);
  }
  
  def Remove(k:K):Boolean =
  {
    return _map.remove(k) != null;
  }
  
  def apply(k:K):V =
  {
    val r = _map.get(k);
    if (r == null && !ContainsKey(k))
      throw new KeyNotFoundException();
    return r;
  }
  
  def update(k:K, v:V) =
  {
    _map.put(k, v);
  }
  
  override def iterator: Iterator[KeyValuePair[K,V]] = new Iterator[KeyValuePair[K,V]] {
    private val it = _map.entrySet().iterator()
    override def hasNext: Boolean = it.hasNext
    override def next(): KeyValuePair[K,V] = { val e = it.next(); return new KeyValuePair(e.getKey(), e.getValue()); }
  }
  
  def Keys:Dictionary_KeyCollection[K] = 
  {
    return new Dictionary_KeyCollection(_map.keySet());
  }
  def Values:Dictionary_ValueCollection[V] = 
  {
    return new Dictionary_ValueCollection(_map.values());
  }
  
  def Count:Int = {
    return _map.size;
  }
  
  def TryGetValue(k:K, out:CsRef[V]):Boolean =
  {
    out.Value = _map.get(k);
    return out.Value != null || _map.containsKey(k);
  }
  
  def Clone():Dictionary[K,V] =
  {
    return new Dictionary[K,V](_map.clone().asInstanceOf[HashMap[K,V]]);
  }
  def Clear() =
  {
    _map.clear();
  }
}