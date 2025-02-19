package System

class Type(val clazz:Class[_])
{
  def Name:String = clazz.getSimpleName();
  def FullName:String = clazz.getName();

  def IsAssignableFrom(other:Type):Boolean = {

    return clazz.isAssignableFrom(other.clazz)
  }
  
  override def equals(other:Any):Boolean = 
  {
    
    if (!other.isInstanceOf[Type])
      return false;
    
    return other.asInstanceOf[Type].clazz == clazz;
  }

  override def hashCode():Int = clazz.hashCode();
}