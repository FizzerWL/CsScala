package System.Collections.Generic

class Queue[T](_queue:scala.collection.mutable.Queue[T]) extends Iterable[T] 
{
  def this() =
  {
    this(new scala.collection.mutable.Queue[T]());
  }
  
  def this(initial:Iterable[T]) =
  {
    this(new scala.collection.mutable.Queue[T]());
    
    for(c <- initial)
      _queue.enqueue(c);
  }
  
  def Count:Int = _queue.length;
  def Enqueue(t:T):Unit = { _queue.enqueue(t); }
  def Dequeue():T = _queue.dequeue();
  def Peek():T = System.Linq.Enumerable.First(_queue);
  def Clear():Unit = { _queue.clear(); }
  def iterator:Iterator[T] = _queue.iterator;
}