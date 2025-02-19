package System.Collections.Generic

class KeyValuePair[K,V](k:K, v:V) 
{
  final val Key:K = k;
  final val Value:V = v;
  
  override def equals(other:Any):Boolean = 
  {
    if (!classOf[KeyValuePair[K, V]].isAssignableFrom(other.getClass()))
      return false;
    
    val o = other.asInstanceOf[KeyValuePair[K,V]];
    return Key.equals(o.Key) && Value.equals(o.Value);
  }
  
  override def hashCode():Int = Key.hashCode() + Value.hashCode();
}