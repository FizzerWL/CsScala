package System.Collections.Generic

import scala.collection.JavaConverters._
import java.util
import java.util.ArrayList
import java.util.Collections
import java.util.Comparator
import System.Linq.Enumerable;

import scala.reflect.ClassTag

class List[T: ClassTag](initialList: ArrayList[T]) extends Iterable[T] {
  final val _list = initialList;

  def this(size: Int) = {
    this(new ArrayList[T](size));
  }

  def this() = {
    this(new ArrayList[T]());
  }
  def this(initial: Iterable[T]) = {
    this(new ArrayList[T]());
    for (e <- initial)
      _list.add(e);
  }

  override def toString(): String = _list.toString();

  def Count: Int = _list.size();
  def Add(a: T):Unit = { _list.add(a); }
  def apply(i: Int): T = _list.get(i);
  def update(i: Int, v: T):Unit = { _list.set(i, v); }
  def RemoveAt(i: Int):Unit = { _list.remove(i); }
  def Clear():Unit = { _list.clear(); }
  def Insert(index: Int, item: T):Unit = { _list.add(index, item); }
  def iterator(): Iterator[T] = _list.iterator().asScala;

  private var _capacity: Int = 0;
  def Capacity = _capacity;
  def Capacity_=(v: Int):Unit = {
    _capacity = v;
    _list.ensureCapacity(v);
  }


  def ForEach(fn: T => Unit):Unit = {
    val it = _list.iterator();
    while (it.hasNext())
      fn(it.next());

  }

  def Reverse():Unit = {
    var i = 0;
    val size = _list.size();
    while (i < size / 2) {
      val s = size - i - 1;
      val t = _list.get(i);
      _list.set(i, _list.get(s));
      _list.set(s, t);
      i += 1;
    }
  }

  def Remove(item: T): Boolean =
    {
      val it = _list.iterator();
      var i = 0;
      while (it.hasNext()) {
        if (it.next() == item) {
          _list.remove(i);
          return true;
        }
        i += 1;
      }

      return false;

    }
  def Contains(item: T): Boolean =
    {
      val it = _list.iterator();
      while (it.hasNext())
        if (it.next() == item)
          return true;

      return false;

    }
  def IndexOf(item: T): Int =
    {
      val it = _list.iterator();
      var i = 0;
      while (it.hasNext()) {
        if (it.next() == item)
          return i;
        i += 1;
      }

      return -1;

    }

  def RemoveRange(startAt: Int, count: Int):Unit = {
    //TODO: Can we do this more efficiently?
    var remain = count;
    while (remain > 0) {
      _list.remove(startAt);
      remain -= 1;
    }
  }

  def RemoveAll(pred: T => Boolean): Int = {
    var removed = 0;
    var i = _list.size() - 1;
    while (i >= 0) {
      val e = _list.get(i);
      if (pred(e)) {
        _list.remove(i);
        removed += 1;
      }
      i -= 1;
    }
    return removed;
  }

  def AddRange(i: Iterable[T]):Unit = {
    for (e <- i)
      _list.add(e);
  }

  def InsertRange(index: Int, a: Iterable[T]):Unit = {
    var i = index;
    for (e <- a) {
      _list.add(i, e);
      i += 1;
    }
  }

  def ToArray(): Array[T] =
    {
      var ret = new Array[T](_list.size());

      var i = 0;
      while (i < ret.length) {
        ret(i) = _list.get(i);
        i += 1;
      }

      return ret;
    }

  class Cmp(fn: (T, T) => Int) extends Comparator[T] {
    def compare(a: T, b: T): Int = fn(a, b);
  }

  class Cmp2(fn: IComparer[T]) extends Comparator[T] {
    def compare(a: T, b: T): Int = fn.Compare(a, b);
  }

  def Sort(fn: (T, T) => Int):Unit = {
    Collections.sort(_list, new Cmp(fn));
  }

  def Sort(startAt: Int, length: Int, comparer: IComparer[T]):Unit = {
    Collections.sort(_list.subList(startAt, startAt + length), new Cmp2(comparer));
  }

  def Sort():Unit = {
    Collections.sort(_list, new Cmp((a, b) =>
      {
        a.asInstanceOf[Comparable[T]].compareTo(b);
      }));
  }

  def FindIndex(fn: T => Boolean): Int = {
    val it = _list.iterator();
    var i: Int = 0;
    while (it.hasNext()) {
      if (fn(it.next()))
        return i;
      i += 1;
    }

    return -1;

  }

  def Exists(fn: T => Boolean): Boolean = {
    FindIndex(fn) >= 0
  }

  def FindAll(fn: T => Boolean) : List[T] = {
    Enumerable.ToList(Enumerable.Where(this, fn))
  }
}