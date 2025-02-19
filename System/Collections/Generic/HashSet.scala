package System.Collections.Generic

import System.Linq.Enumerable
import scala.collection.JavaConverters._

class HashSet[T] extends Iterable[T] {
  val _set = new java.util.HashSet[T]();

  def this(initial: Iterable[T]) = {
    this();
    for (e <- initial)
      Add(e);
  }

  def Add(t: T): Boolean = _set.add(t);

  def Contains(t: T): Boolean = _set.contains(t);

  def Remove(t: T): Boolean =
    {
      return _set.remove(t);
    }

  def RemoveWhere(pred: T => Boolean): Int =
    {
      val list = new System.Collections.Generic.List[Any]();
      for (e <- this)
        if (pred(e))
          list.Add(e);

      for (n <- list)
        _set.remove(n);

      return list.Count;
    }

  def iterator:Iterator[T] = _set.iterator().asScala;

  def Count: Int = _set.size;

  def Clear():Unit = {
    _set.clear();
  }

  override def equals(other: Any): Boolean = {
    if (!classOf[HashSet[T]].isAssignableFrom(other.getClass()))
      return false;
    val o = other.asInstanceOf[HashSet[T]];
    if (o.Count != this.Count)
      return false;
    val it = _set.iterator();
    while (it.hasNext())
      if (!o.Contains(it.next()))
        return false;
    return true;
  }
}