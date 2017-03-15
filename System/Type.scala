package System

class Type(obj:Class[_])
{
  final val _obj = obj;
  
  def Name:String = obj.getSimpleName();
  def FullName:String = obj.getName();

  
  
  override def equals(other:Any):Boolean = 
  {
    
    if (!other.isInstanceOf[Type])
      return false;
    
    return other.asInstanceOf[Type]._obj == _obj;
  }
  override def hashCode():Int = _obj.hashCode();
}