package System.Collections.Generic

import java.util.ArrayList
import java.util.Collections
import java.util.Comparator
import scala.reflect.ClassTag
import System.NotImplementedException

class List[T:ClassTag](initialList:ArrayList[T]) extends Traversable[T]
{
  final val _list = initialList;
  
  def this(size:Int)
  {
    this(new ArrayList[T](size));
  }
  
  def this()
  {
    this(new ArrayList[T]());
  }
  def this(initial:Traversable[T])
  {
    this(new ArrayList[T]());
    for(e <- initial)
      _list.add(e);
  }
  
  
  def Count:Int = _list.size();
  def Add(a:T) { _list.add(a); }
  def apply(i:Int):T = _list.get(i);
  def update(i:Int, v:T) { _list.set(i, v); }
  def RemoveAt(i:Int) { _list.remove(i); }
  def Clear() { _list.clear(); }
  def Insert(index:Int, item:T) { _list.add(index, item); }
  def iterator():java.util.Iterator[T] = _list.iterator();
  
  def foreach[U](fn:T=>U)
  {
    val it = _list.iterator();
    while (it.hasNext())
      fn(it.next());
    
  }
  
  def ForEach(fn:T=>Unit)
  {
    val it = _list.iterator();
    while (it.hasNext())
      fn(it.next());
    
  }

  def Reverse()
  {
    var i = 0;
    val size = _list.size();
    while (i < size / 2)
      {
        val s = size - i - 1;
        val t = _list.get(i);
        _list.set(i, _list.get(s));
        _list.set(s, t);
        i += 1;
      }
  }
  
  def Remove(item:T):Boolean =
  {
    val it = _list.iterator();
    var i = 0;
    while (it.hasNext())
    {
      if (it.next() == item)
      {
        _list.remove(i);
        return true;
      }
      i += 1;
    }
      
    return false;

  }
  def Contains(item:T):Boolean =
  {
    val it = _list.iterator();
    while (it.hasNext())
      if (it.next() == item)
        return true;
      
    return false;

  }
  def IndexOf(item:T):Int =
  {
    val it = _list.iterator();
    var i = 0;
    while (it.hasNext())
    {
      if (it.next() == item)
        return i;
      i += 1;
    }
      
    return -1;

  }
  
  def RemoveRange(startAt:Int, count:Int)
  {
    //TODO: Can we do this more efficiently?
    var remain = count;
    while (remain > 0)
    {
      _list.remove(startAt);
      remain -= 1;
    }
  }
  
  def AddRange(i:Traversable[T]) 
  { 
    for(e <- i)
      _list.add(e); 
  }
  
  def ToArray():Array[T] =
  {
    var ret = new Array[T](_list.size());
    
    var i = 0;
    while (i < ret.length)
    {
      ret(i) = _list.get(i);
      i += 1;
    }
    
    return ret;
  }
  
  class Cmp(fn:(T,T)=>Int) extends Comparator[T]
  {
    def compare(a:T, b:T):Int = fn(a,b);
  }
  
  class Cmp2(fn:IComparer[T]) extends Comparator[T]
  {
    def compare(a:T, b:T):Int = fn.Compare(a,b);
  }
  
  def Sort(fn:(T,T)=>Int)
  {
    Collections.sort(_list, new Cmp(fn));
  }

  def Sort(startAt:Int, length:Int, comparer:IComparer[T])
  {
    Collections.sort(_list.subList(startAt, length), new Cmp2(comparer));
  }

  def Sort()
  {
    Collections.sort(_list, new Cmp((a,b) =>
      {
        a.asInstanceOf[Comparable[T]].compareTo(b);
      }));
  }
 

}