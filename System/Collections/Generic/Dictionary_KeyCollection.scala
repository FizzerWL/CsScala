package System.Collections.Generic

import scala.collection.JavaConverters._


class Dictionary_KeyCollection[T](i:java.util.Set[T]) extends Iterable[T] 
{

  def Count:Int = i.size();
  
  def iterator():Iterator[T] = i.iterator().asScala;

}