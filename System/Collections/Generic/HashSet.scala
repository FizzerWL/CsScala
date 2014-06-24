package System.Collections.Generic


class HashSet[T] extends Traversable[T] 
{
  val _set = new java.util.HashSet[T]();
  
  def this(initial:Traversable[T])
  {
    this();
    for(e <- initial)
      Add(e);
  }

  def Add(t:T):Boolean =
  {
    return _set.add(t);
  }
  
  def Contains(t:T):Boolean = 
  {
    return _set.contains(t);
  }
  
  def Remove(t:T):Boolean = 
  {
   	return _set.remove(t);
  }
  
  def foreach[U](fn:T=>U)
  {
    val it = _set.iterator();
    while (it.hasNext())
      fn(it.next());
  }
  
  def Count:Int = 
  {
    return _set.size;
  }
  
  def Clear() {
    _set.clear();
  }
  
  override def equals(other:Any):Boolean = {
    if (!other.isInstanceOf[HashSet[T]])
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