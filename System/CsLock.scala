package System

object CsLock 
{
  def Lock(obj:Any, cb:()=>Unit)
  {
    obj.asInstanceOf[AnyRef].synchronized({ cb(); });
  }

}