package System

object Buffer 
{

  //Note: .net's Buffer.BlockCopy allows copying from arrays of different types (byte array to float array, for example.)  Java's arraycopy requires they be the same type.
  def BlockCopy(from:AnyRef, fromIndex:Int, to:AnyRef, toIndex:Int, length:Int)
  {
    java.lang.System.arraycopy(from, fromIndex, to, toIndex, length);
  }
}