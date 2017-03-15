package System.Collections;

class DictionaryEntry(k:Any, v:Any)
{
	final val Key = k;
	final val Value = v;
	
	override def equals(other:Any):Boolean = {
	  if (!other.isInstanceOf[DictionaryEntry])
	    return false;
	  val o = other.asInstanceOf[DictionaryEntry];
	  return o.Key.equals(Key) && o.Value.equals(Value);
	}
	override def hashCode():Int = k.hashCode() + v.hashCode();
}