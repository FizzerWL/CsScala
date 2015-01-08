package System.Linq;

import System.Collections.Generic.Dictionary
import System.Collections.Generic.List
import System.DateTime
import scala.reflect._
import scala.collection.JavaConverters._
import System.NotImplementedException

object Enumerable 
{
  def Select[F,T](a:Traversable[F], fn:F=>T):Traversable[T] = 
  {
    return a.map(fn);
  }
  def Select[F,T](a:Traversable[F], fn:(F,Int)=>T):Traversable[T] = 
  {
    var i = 0;
    return a.map(b =>
      {
        var r = fn(b, i);
        i += 1;
        r;
      });
      
  }
  def Where[T](a:Traversable[T], fn:T=>Boolean):Traversable[T] =
  {
    return a.filter(fn);
  }
  
  def ToDictionary[S,K,V](a:Traversable[S], makeKey:S=>K, makeVal:S=>V):Dictionary[K,V] =
  {
    var r = new Dictionary[K,V]();
    for(e <- a)
      r.Add(makeKey(e), makeVal(e));
    return r;
  }
  
  def ToArray[T:ClassTag](a:Traversable[T]):Array[T] =
  {
    return a.toArray;
  }
  
  def ToList[T:ClassTag](a:Traversable[T]):List[T] =
  {
    val ret = new List[T]();
    for(e <- a)
      ret.Add(e);
    return ret;
  }
  
  def Min(a:Traversable[Int]):Int =
  {
    return a.min;
  }
  def Min(a:Traversable[DateTime]):DateTime =
  {
    return a.minBy(_.Ticks);
  }
  def Min[T](a:Traversable[T], fn:T=>DateTime):DateTime =
  {
    return a.map(fn).minBy(_.Ticks);
  }
  def Min[T](a:Traversable[T], fn:T=>Int):Int =
  {
    return a.map(fn).min;
  }
  def Min[T](a:Traversable[T], fn:T=>Float):Float =
  {
    return a.map(fn).min;
  }
  
  def GroupBy[S,K](a:Traversable[S], fn:S=>K):Traversable[IGrouping[K,S]] =
  {
    return a.groupBy(fn).map(g => new IGrouping(g._1, g._2));
  }
  
  def Count[T](a:Traversable[T]):Int =
  {
    return a.count(_ => true);
  }
  def Count[T](a:Traversable[T], pred:T=>Boolean):Int =
  {
    return a.count(pred);
  }
  
  def First[T](a:Traversable[T]):T =
  {
    return a.head;
  }
  def First[T](a:Traversable[T], pred:T=>Boolean):T =
  {
    for(c <- a)
      if (pred(c))
        return c;
    throw new Exception("No elements match");
  }
  def FirstOrDefault[T >: Null](a:Traversable[T], pred:T=>Boolean):T =
  {
    for(c <- a)
      if (pred(c))
        return c;
    return null;
  }
  def FirstOrDefault[T >: Null](a:Traversable[T]):T =
  {
    for(c <- a)
      return c;
    return null;
  }
  def ElementAt[T](a:Traversable[T], index:Int):T =
  {
    var i = index;
    for(c <- a)
    {
      if (i == 0)
        return c;
      i -= 1;
    }
    
    throw new Exception("ElementAt exceeds bounds");
  }
  
  def Last[T](a:Traversable[T]):T =
  {
    var ret:T = a.head;
    
    for(c <- a)
      ret = c;
    return ret;
  }
  def LastOrDefault[T >: Null](a:Traversable[T], pred:T=>Boolean):T =
  {
    var ret:T = null;
    
    for(c <- a)
      if (pred(c))
    	ret = c;
    return ret;
  }
  def LastOrDefault[T >: Null](a:Traversable[T]):T =
  {
    var ret:T = null;
    
    for(c <- a)
      ret = c;
    return ret;
  }
  
  def OrderBy_Int[T](a:Traversable[T], fn:T=>Int):Traversable[T] =
  {
    return a.toBuffer.sortBy(fn);
  }
  def OrderBy_Float[T](a:Traversable[T], fn:T=>Float):Traversable[T] =
  {
    return a.toBuffer.sortBy(fn);
  }
  def OrderBy_Double[T](a:Traversable[T], fn:T=>Double):Traversable[T] =
  {
    return a.toBuffer.sortBy(fn);
  }
  def OrderBy_Long[T](a:Traversable[T], fn:T=>Long):Traversable[T] =
  {
    return a.toBuffer.sortBy(fn);
  }
  def OrderBy_String[T](a:Traversable[T], fn:T=>String):Traversable[T] =
  {
    return a.toBuffer.sortBy(fn);
  }
  def OrderByDescending[T](a:Traversable[T], fn:T=>Double):Traversable[T] =
  {
    return a.toBuffer.sortBy((o:T) => -fn(o));
  }
  
  def OfType[T,K:ClassTag](a:Traversable[T]):Traversable[K] =
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
  
  def Sum(a:Traversable[Double]):Double =
  {
    var ret:Double = 0;
    for (e <- a)
      ret += e;
    return ret;
  }
  def Sum(a:Traversable[Int]):Int =
  {
    var ret:Int = 0;
    for (e <- a)
      ret += e;
    return ret;
  }
  def Sum[T](a:Traversable[T], fn:T=>Int):Int =
  {
    var ret:Int = 0;
    for (e <- a)
      ret += fn(e);
    return ret;
  }
  def Sum[T](a:Traversable[T], fn:T=>Long):Long =
  {
    var ret:Long = 0;
    for (e <- a)
      ret += fn(e);
    return ret;
  }
  def Sum[T](a:Traversable[T], fn:T=>Double):Double =
  {
    var ret:Double = 0;
    for (e <- a)
      ret += fn(e);
    return ret;
  }
  
  def Range(startAt:Int, count:Int):Traversable[Int] =
  {
    return startAt until (count + startAt);
  }
  
  def Concat[T](a:Traversable[T], b:Traversable[T]):Traversable[T] = 
  {
    return a ++ b;
  }
  def Except[T](a:Traversable[T], b:Traversable[T]):Traversable[T] = 
  {
    return a.toBuffer.diff(b.toBuffer);
  }
  
  def Single[T](a:Traversable[T]):T =
  {
    val it = a.toIterator;
    
    if (!it.hasNext)
      throw new Exception("Single called on empty collection");
    
    val ret = it.next;
    if (it.hasNext)
      throw new Exception("Single called with too many elements");
    return ret;
  }
  def Single[T](a:Traversable[T], pred:T=>Boolean):T =
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
  def SingleOrDefault[T >: Null](a:Traversable[T]):T =
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
  def SingleOrDefault[T >: Null](a:Traversable[T], pred:T=>Boolean):T =
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
  
  def SelectMany[F,T](a:Traversable[F], fn:F=>Traversable[T]):Traversable[T] =
  {
    return a.flatMap(fn);
  }
  def SelectMany[F,T](a:Traversable[F], fn:(F,Int)=>Traversable[T]):Traversable[T] =
  {
    var i = 0;
    return a.map(e => 
      {
        var r = fn(e, i);
        i += 1;
        r;
      }).flatten;
  }
  
  def Distinct[T](a:Traversable[T]):Traversable[T] =
  {
    return a.toBuffer.distinct;
  }
  
  def Any[T](a:Traversable[T], pred:T=>Boolean):Boolean =
  {
    return a.exists(pred);
  }
  def Any[T](a:Traversable[T]):Boolean =
  {
    return !a.isEmpty;
  }
  
  def Max[T](a:Traversable[T], fn:T=>DateTime):DateTime =
  {
    return a.map(fn).maxBy(_.Ticks);
  }
  def Max[T](a:Traversable[T], fn:T=>Long):Long =
  {
    return a.map(fn).max;
  }
  def Max[T](a:Traversable[T], fn:T=>Int):Int =
  {
    return a.map(fn).max;
  }
  def Max[T](a:Traversable[T], fn:T=>Float):Float =
  {
    return a.map(fn).max;
  }
  def Max(a:Traversable[DateTime]):DateTime =
  {
    return a.maxBy(_.Ticks);
  }
  def Max(a:Traversable[Int]):Int =
  {
    return a.max;
  }

  def Take[T](a:Traversable[T], numtoTake:Int):Traversable[T] =
  {
    return a.take(numtoTake);
  }
  def All[T](a:Traversable[T], fn:T=>Boolean):Boolean =
  {
    return a.forall(fn);
  }
  
  def Intersect[T](a:Traversable[T], b:Traversable[T]):Traversable[T] = 
  {
    return a.toBuffer.intersect(b.toBuffer);
  }
  
  def Cast[T](a:Traversable[Any]):Traversable[T] =
  {
    return a.map(_.asInstanceOf[T]);
  }
  
  def Union[T](a:Traversable[T], b:Traversable[T]):Traversable[T] =
  {
    return a.toBuffer.union(b.toBuffer);
  }
  
  def Aggregate[T](a:Traversable[T], fn:(T,T)=>T):T =
  {
    return a.reduceLeft(fn); 
  }
  
  def Skip[T](a:Traversable[T], numToSkip:Int):Traversable[T] =
  {
    return a.drop(numToSkip);
  }
  
  def Reverse[T](a:Traversable[T]):Traversable[T] =
  {
    return a.toBuffer.reverse;
  }
  
  def Average[T](a:Traversable[T], fn:T=>Double):Double = 
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
  
  
  def Average[T](a:Traversable[Double]):Double = 
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


