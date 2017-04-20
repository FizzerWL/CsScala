package System.Collections.Generic


class Dictionary_ValueCollection[T](i:java.util.Collection[T]) extends Traversable[T]
{
  
  def Count:Int = 
  {
    return i.size();
  }
  def foreach[U](fn:T=>U)
  {
    val it = i.iterator;
	while (it.hasNext()) 
	{
	   fn(it.next());
	}
  }
  
  def iterator():java.util.Iterator[T] = i.iterator();

}