package System.IO

import System.NotImplementedException
import java.io._
import System.NotImplementedException

class MemoryStream(input:InputStream, output:OutputStream) extends Stream(input,output,null,null) 
{
  def this(bytes:Array[Byte])
  {
    this(new ByteArrayInputStream(bytes), null);
  }
  
  def this()
  {
    this(null, new ByteArrayOutputStream());
  }
  
  def this(sizeEstimate:Int)
  {
    this(null, new ByteArrayOutputStream(sizeEstimate));
  }

  var _overrideArray:Array[Byte] = null;
  def ToArray():Array[Byte] =
  {
    if (_overrideArray != null)
      return _overrideArray;
    return _output.asInstanceOf[ByteArrayOutputStream].toByteArray();
  }
  
  def Seek(offset:Long, loc:Int)
  {
    //This assumes you're seeking and output stream back to the beginning to begin using it as inputstream.  Other uses are not supported.
    if (loc != SeekOrigin.Begin || offset != 0)
      throw new NotImplementedException("TODO");
    
    _input = new ByteArrayInputStream(ToArray());
    _output = null;
  }
  
  def Length:Int = ToArray().length;
  
}