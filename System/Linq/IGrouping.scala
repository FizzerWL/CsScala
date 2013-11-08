package System.Linq

import System.NotImplementedException

class IGrouping[K,V](k:K, vals:Traversable[V]) extends Traversable[V] 
{
  val Key:K = k;

  def foreach[U](fn:V=>U)
  {
    for(c <- vals)
      fn(c);
  }
}