package System

object Buffer 
{

  def BlockCopy(from:AnyRef, fromIndex:Int, to:AnyRef, toIndex:Int, length:Int)
  {
    java.lang.System.arraycopy(from, fromIndex, to, toIndex, length);
  }
}