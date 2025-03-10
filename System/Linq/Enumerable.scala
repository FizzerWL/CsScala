package System.Linq;

import System.Collections.Generic.Dictionary
import System.Collections.Generic.List
import System.DateTime
import scala.reflect._
import scala.collection.JavaConverters._

object Enumerable 
{
  def Select[F,T](a:Iterable[F], fn:F=>T):Iterable[T] = 
  {
    return a.map(fn);
  }
  def Select[F,T](a:Iterable[F], fn:(F,Int)=>T):Iterable[T] = 
  {
    var i = 0;
    return a.map(b =>
      {
        var r = fn(b, i);
        i += 1;
        r;
      });
      
  }
  def Where[T](a:Iterable[T], fn:T=>Boolean):Iterable[T] =
  {
    return a.filter(fn);
  }
  
  def ToDictionary[S,K,V](a:Iterable[S], makeKey:S=>K, makeVal:S=>V):Dictionary[K,V] =
  {
    var r = new Dictionary[K,V]();
    for(e <- a)
      r.Add(makeKey(e), makeVal(e));
    return r;
  }
  
  def ToArray[T:ClassTag](a:Iterable[T]):Array[T] =
  {
    return a.toArray;
  }
  
  def ToList[T:ClassTag](a:Iterable[T]):List[T] =
  {
    val ret = new List[T]();
    for(e <- a)
      ret.Add(e);
    return ret;
  }
  
  def Min(a:Iterable[Int]):Int =
  {
    return a.min;
  }
  def Min(a:Iterable[Long]):Long =
  {
    return a.min;
  }
  def Min(a:Iterable[Double]):Double =
  {
    return a.min;
  }
  def Min(a:Iterable[DateTime]):DateTime =
  {
    return a.minBy(_.Ticks);
  }
  def Min[T](a:Iterable[T], fn:T=>DateTime):DateTime =
  {
    return a.map(fn).minBy(_.Ticks);
  }
  def Min[T](a:Iterable[T], fn:T=>Int):Int =
  {
    return a.map(fn).min;
  }
  def Min[T](a:Iterable[T], fn:T=>Long):Long =
  {
    return a.map(fn).min;
  }
  def Min[T](a:Iterable[T], fn:T=>Float):Float =
  {
    return a.map(fn).min;
  }
  def Min[T](a:Iterable[T], fn:T=>Double):Double =
  {
    return a.map(fn).min;
  }
  
  def GroupBy[S,K](a:Iterable[S], fn:S=>K):Iterable[IGrouping[K,S]] =
  {
    return a.groupBy(fn).map(g => new IGrouping(g._1, g._2));
  }
  
  def Count[T](a:Iterable[T]):Int =
  {
    return a.count(_ => true);
  }
  def Count[T](a:Iterable[T], pred:T=>Boolean):Int =
  {
    return a.count(pred);
  }
  
  def First[T](a:Iterable[T]):T =
  {
    return a.head;
  }
  def First[T](a:Iterable[T], pred:T=>Boolean):T =
  {
    val it = a.iterator;
    while (it.hasNext) {
      val c = it.next;
      if (pred(c))
        return c;
    }
    throw new Exception("No elements match");
  }
  def FirstOrDefault[T >: Null](a:Iterable[T], pred:T=>Boolean):T =
  {
    val it = a.iterator;
    while (it.hasNext) {
      val c = it.next;
      if (pred(c))
        return c;
    }
    return null;
  }
  def FirstOrDefault[T >: Null](a:Iterable[T]):T =
  {
    val it = a.iterator;
    if (it.hasNext)
      return it.next;
    else
      return null;
  }
  def ElementAt[T](a:Iterable[T], index:Int):T =
  {
    var i = index;
    val it = a.iterator;
    while (it.hasNext)
    {
      val c = it.next;
      if (i == 0)
        return c;
      i -= 1;
    }
    
    throw new Exception("ElementAt exceeds bounds");
  }
  
  def Last[T](a:Iterable[T]):T =
  {
    var ret:T = a.head;
    val it = a.iterator;

    while (it.hasNext)
      ret = it.next;
    
    return ret;
  }
  def Last[T](a:Iterable[T], pred:T=>Boolean):T = {
    val rev = a.toBuffer.reverse;

    val it = rev.iterator;
    while (it.hasNext) {
      val c = it.next;
      if (pred(c))
        return c;
    }    
    throw new Exception("No elements match");
  }
  def LastOrDefault[T >: Null](a:Iterable[T], pred:T=>Boolean):T = {
    val rev = a.toBuffer.reverse;
    val it = rev.iterator;
    
    while (it.hasNext) {
      val c = it.next;
      if (pred(c))
        return c;
    }
    
    return null;
  }
  def LastOrDefault[T >: Null](a:Iterable[T]):T =
  {
    var ret:T = null;
    
    val it = a.iterator;
    while (it.hasNext)
      ret = it.next;
    return ret;
  }
  
  def OrderBy_Int[T](a:Iterable[T], fn:T=>Int):Iterable[T] = {
    return a.toBuffer.sortBy(fn);
  }
  def OrderBy_Float[T](a:Iterable[T], fn:T=>Float):Iterable[T] = {
    return a.toBuffer.sortBy(fn);
  }
  def OrderBy_Double[T](a:Iterable[T], fn:T=>Double):Iterable[T] = {
    return a.toBuffer.sortBy(fn);
  }
  def OrderBy_Long[T](a:Iterable[T], fn:T=>Long):Iterable[T] = {
    return a.toBuffer.sortBy(fn);
  }
  def OrderBy_String[T](a:Iterable[T], fn:T=>String):Iterable[T] = {
    return a.toBuffer.sortBy(fn);
  }
  
  
  def OrderByDescending_Int[T](a:Iterable[T], fn:T=>Int):Iterable[T] = {
    return a.toBuffer.sortBy((o:T) => -fn(o));
  }
  def OrderByDescending_Float[T](a:Iterable[T], fn:T=>Float):Iterable[T] =
  {
    return a.toBuffer.sortBy((o:T) => -fn(o));
  }
  def OrderByDescending_Double[T](a:Iterable[T], fn:T=>Double):Iterable[T] =
  {
    return a.toBuffer.sortBy((o:T) => -fn(o));
  }
  def OrderByDescending_Long[T](a:Iterable[T], fn:T=>Long):Iterable[T] =
  {
    return a.toBuffer.sortBy((o:T) => -fn(o));
  }
  def OrderByDescending_String[T](a:Iterable[T], fn:T=>String):Iterable[T] =
  {
    return a.toBuffer.sortBy(fn).reverse;
  }
  
  def OrderByDescending[T](a:Iterable[T], fn:T=>Double):Iterable[T] =
  {
    return a.toBuffer.sortBy((o:T) => -fn(o));
  }
  
  def OfType[T,K:ClassTag](a:Iterable[T]):Iterable[K] =
  {
    var c = classTag[K].runtimeClass;
    var ret = new List[K]();
    
    for (e <- a)
    {
      if (c.isInstance(e))
        ret.Add(e.asInstanceOf[K]);
    }
    
    return ret;
    
    /*return a.flatMap(e => e match
      {
      case e: K => e :: Nil
      case _ => Nil
      });
    */
  }
  
  def Sum(a:Iterable[Double]):Double =
  {
    var ret:Double = 0;
    for (e <- a)
      ret += e;
    return ret;
  }
  def Sum(a:Iterable[Float]):Float =
  {
    var ret:Float = 0;
    for (e <- a)
      ret += e;
    return ret;
  }
  def Sum(a:Iterable[Int]):Int =
  {
    var ret:Int = 0;
    for (e <- a)
      ret += e;
    return ret;
  }
  def Sum[T](a:Iterable[T], fn:T=>Int):Int =
  {
    var ret:Int = 0;
    for (e <- a)
      ret += fn(e);
    return ret;
  }
  def Sum[T](a:Iterable[T], fn:T=>Long):Long =
  {
    var ret:Long = 0;
    for (e <- a)
      ret += fn(e);
    return ret;
  }
  def Sum[T](a:Iterable[T], fn:T=>Double):Double =
  {
    var ret:Double = 0;
    for (e <- a)
      ret += fn(e);
    return ret;
  }
  def Sum[T](a:Iterable[T], fn:T=>Float):Float =
  {
    var ret:Float = 0;
    for (e <- a)
      ret += fn(e);
    return ret;
  }
  
  def Range(startAt:Int, count:Int):Iterable[Int] =
  {
    return startAt until (count + startAt);
  }
  
  def Concat[T](a:Iterable[T], b:Iterable[T]):Iterable[T] = 
  {
    return a ++ b;
  }
  def Except[T](a:Iterable[T], b:Iterable[T]):Iterable[T] = 
  {
    return a.toBuffer.diff(b.toBuffer);
  }
  
  def Single[T](a:Iterable[T]):T =
  {
    val it = a.toIterator;
    
    if (!it.hasNext)
      throw new Exception("Single called on empty collection");
    
    val ret = it.next;
    if (it.hasNext)
      throw new Exception("Single called with too many elements");
    return ret;
  }
  def Single[T](a:Iterable[T], pred:T=>Boolean):T =
  {
    var ret:T = a.head;
    var found = false;
    for(c <- a)
      if (pred(c))
      {
        if (found)
          throw new Exception("Multiple elements match");
        ret = c;
        found = true;
      }
    
    if (!found)
      throw new Exception("No elements match");
    
    return ret;
  }
  def SingleOrDefault[T >: Null](a:Iterable[T]):T =
  {
    if (a == null)
      throw new Exception("SingleOrDefault called on null");
   
    val it = a.toIterator;
    
    if (!it.hasNext)
      return null;
    
    val ret = it.next;
    
    if (it.hasNext)
      throw new Exception("Single called with too many elements");
    
    return ret;
  }
  def SingleOrDefault[T >: Null](a:Iterable[T], pred:T=>Boolean):T =
  {
    var ret:T = null;
    var found = false;
    for(c <- a)
      if (pred(c))
      {
        if (found)
          throw new Exception("Multiple elements match");
        ret = c;
        found = true;
      }
    return ret;
  }
  
  def SelectMany[F,T](a:Iterable[F], fn:F=>Iterable[T]):Iterable[T] =
  {
    return a.flatMap(fn);
  }
  def SelectMany[F,T,R](a:Iterable[F], fn:F=>Iterable[T], resultSelector:(F,T)=>R):Iterable[R] =
  {
    return a.map(e => fn(e).map(e2 => resultSelector(e, e2))).flatten;
  }
  def SelectMany[F,T](a:Iterable[F], fn:(F,Int)=>Iterable[T]):Iterable[T] =
  {
    var i = 0;
    return a.map(e => 
      {
        var r = fn(e, i);
        i += 1;
        r;
      }).flatten;
  }
  
  def Distinct[T](a:Iterable[T]):Iterable[T] =
  {
    return a.toBuffer.distinct;
  }
  
  def Any[T](a:Iterable[T], pred:T=>Boolean):Boolean =
  {
    return a.exists(pred);
  }
  def Any[T](a:Iterable[T]):Boolean =
  {
    return !a.isEmpty;
  }
  
  def Max[T](a:Iterable[T], fn:T=>DateTime):DateTime =
  {
    return a.map(fn).maxBy(_.Ticks);
  }
  def Max[T](a:Iterable[T], fn:T=>Long):Long =
  {
    return a.map(fn).max;
  }
  def Max[T](a:Iterable[T], fn:T=>Int):Int =
  {
    return a.map(fn).max;
  }
  def Max[T](a:Iterable[T], fn:T=>Float):Float =
  {
    return a.map(fn).max;
  }
  def Max[T](a:Iterable[T], fn:T=>Double):Double =
  {
    return a.map(fn).max;
  }
  def Max(a:Iterable[DateTime]):DateTime =
  {
    return a.maxBy(_.Ticks);
  }
  def Max(a:Iterable[Int]):Int =
  {
    return a.max;
  }
  def Max(a:Iterable[Double]):Double =
  {
    return a.max;
  }
  def Max(a:Iterable[Float]):Float =
  {
    return a.max;
  }

  def Take[T](a:Iterable[T], numtoTake:Int):Iterable[T] =
  {
    return a.take(numtoTake);
  }
  def TakeWhile[T](a:Iterable[T], fn:T=>Boolean):Iterable[T] =
  {
    return a.takeWhile(fn);
  }
  def All[T](a:Iterable[T], fn:T=>Boolean):Boolean =
  {
    return a.forall(fn);
  }
  
  def Intersect[T](a:Iterable[T], b:Iterable[T]):Iterable[T] = 
  {
    return a.toBuffer.intersect(b.toBuffer);
  }
  
  def Cast[T](a:Iterable[Any]):Iterable[T] =
  {
    return a.map(_.asInstanceOf[T]);
  }
  
  def Union[T](a:Iterable[T], b:Iterable[T]):Iterable[T] =
  {
    return a.toBuffer.union(b.toBuffer);
  }
  
  def Aggregate[T](a:Iterable[T], fn:(T,T)=>T):T =
  {
    return a.reduceLeft(fn); 
  }
  
  def Skip[T](a:Iterable[T], numToSkip:Int):Iterable[T] =
  {
    return a.drop(numToSkip);
  }
  
  def Reverse[T](a:Iterable[T]):Iterable[T] =
  {
    return a.toBuffer.reverse;
  }
  
  def Average[T](a:Iterable[T], fn:T=>Double):Double = 
  {
    var sum:Double = 0;
    var count:Int = 0;
    for (e <- a)
    {
      sum += fn(e);
      count += 1;
    }
    return sum / count;
  }
  
  def Average[T](a:Iterable[T], fn:T=>Float):Float = 
  {
    var sum:Float = 0;
    var count:Int = 0;
    for (e <- a)
    {
      sum += fn(e);
      count += 1;
    }
    return sum / count;
  }
  
  
  def Average[T](a:Iterable[Double]):Double = 
  {
    var sum:Double = 0;
    var count:Int = 0;
    for (e <- a)
    {
      sum += e;
      count += 1;
    }
    return sum / count;
  }
  
}


