package System.IO

import System.NotImplementedException
import System.NotImplementedException

class BinaryWriter(s:Stream)
{
  val BaseStream:Stream = s;

  def Write(b:Byte)
  {
    throw new NotImplementedException();
  }
  
  def Write(bytes:Array[Byte])
  {
    throw new NotImplementedException();
  }
  
  def Write(d:Double)
  {
    throw new NotImplementedException();
  }
  
  def Write(b:Boolean)
  {
    throw new NotImplementedException();
  }
  def Write(s:String)
  {
    throw new NotImplementedException();
  }
}