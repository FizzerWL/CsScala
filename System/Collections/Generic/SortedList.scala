package System.Collections.Generic

import java.util.HashMap

class SortedList[K,V]
{
  val _map = new HashMap[K,V]();

  def Add(k:K, v:V)
  {
    _map.put(k, v);
  }
  
  def apply(k:K):V =
  {
    return _map.get(k);
  }
}