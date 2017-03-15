package System.Collections.Generic

class Queue[T](_queue:scala.collection.mutable.Queue[T]) extends Traversable[T] 
{
  def this()
  {
    this(new scala.collection.mutable.Queue[T]());
  }
  
  def this(initial:Traversable[T])
  {
    this(new scala.collection.mutable.Queue[T]());
    
    for(c <- initial)
      _queue.enqueue(c);
  }
  
  def Count:Int = _queue.length;
  def Enqueue(t:T) { _queue.enqueue(t); }
  def Dequeue():T = _queue.dequeue();
  def Peek():T = System.Linq.Enumerable.First(_queue);
  def Clear() { _queue.clear(); }
  def foreach[U](fn:T=>U) { _queue.foreach(fn); }
}