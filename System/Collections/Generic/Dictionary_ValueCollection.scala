package System.Collections.Generic

import scala.collection.JavaConverters._


class Dictionary_ValueCollection[T](i:java.util.Collection[T]) extends Iterable[T]
{
  
  def Count:Int =  i.size();
  def iterator():Iterator[T] = i.iterator().asScala;

}