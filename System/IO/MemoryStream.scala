package System.IO

import System.NotImplementedException

class MemoryStream(bytes:Array[Byte] = null) extends Stream 
{

  def Dispose()
  {
    
  }
  def ToArray():Array[Byte] =
  {
    throw new NotImplementedException();
  }
  
}