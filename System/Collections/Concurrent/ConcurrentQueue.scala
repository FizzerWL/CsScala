package System.Collections.Concurrent

import System.CsRef

class ConcurrentQueue[T](_queue:java.util.concurrent.ConcurrentLinkedQueue[T])
{
  def this()
  {
    this(new java.util.concurrent.ConcurrentLinkedQueue[T]());
  }
  
  def this(initial:Traversable[T])
  {
    this(new java.util.concurrent.ConcurrentLinkedQueue[T]());
    
    for(c <- initial)
      _queue.add(c);
  }
  
  def Count:Int = _queue.size();
  def Enqueue(t:T) { _queue.add(t); }
  def Peek():T = _queue.peek();
  def Clear() { _queue.clear(); }
  
  def TryDequeue(result:CsRef[T]):Boolean = {
    result.Value = _queue.poll();
    return result.Value != null;
  }
  
  def Poll() = _queue.poll();
}