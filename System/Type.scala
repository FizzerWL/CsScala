package System

class Type(obj:Class[_])
{
  final val _obj = obj;
  
  val Name:String = obj.getName();

  
  
  override def equals(other:Any):Boolean = 
  {
    if (!other.isInstanceOf[Type])
      return false;
    return other.asInstanceOf[Type]._obj == _obj;
  }
}