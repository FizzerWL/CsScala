package System.Linq

class IGrouping[K,V](k:K, vals:Iterable[V]) extends Iterable[V] 
{
  val Key:K = k;

  def iterator:Iterator[V] = vals.iterator;
}