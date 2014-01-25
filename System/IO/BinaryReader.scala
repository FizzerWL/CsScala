package System.IO

import System.NotImplementedException
import System.InvalidOperationException
import java.nio.ByteBuffer
import System.Text.Encoding;
import java.nio.ByteOrder

class BinaryReader(s:Stream)
{
  val BaseStream = s;

  def ReadBytes(num:Int):Array[Byte] = 
  {
    val ret = new Array[Byte](num);
    val numRead = s._input.read(ret, 0, num);
    if (numRead != num)
      throw new EndOfStreamException();
    return ret;
  }
  
  private var _internalBuffer:Array[Byte] = null;
  private var _byteBuffer:ByteBuffer = null;
  
  def MakeBuffer(size:Int)
  {
    _internalBuffer = new Array[Byte](size);
    _byteBuffer = ByteBuffer.wrap(_internalBuffer);
    _byteBuffer.order(ByteOrder.LITTLE_ENDIAN);
  }
  MakeBuffer(256);
  
  private def FillInternalBuffer(num:Int):Array[Byte] =
  {
    if (num > _internalBuffer.length)
      MakeBuffer(num);
        
    
    val numRead = s._input.read(_internalBuffer, 0, num);
    if (numRead != num)
      throw new EndOfStreamException();
    return _internalBuffer;
  }
  
  def Read7BitEncodedInt():Int =
  {
    var num = 0;
	var num2 = 0;
	while (num2 != 35)
	{
		var b = this.ReadByte();
		num |= (b & 127).toInt << num2;
		num2 += 7;
		if ((b & 128) == 0)
			return num;
	}
	throw new Exception("Format_Bad7BitInt32");
  }
  
  def ReadDouble():Double =
  {
    FillInternalBuffer(8)
    return _byteBuffer.getDouble(0);
  }
  
  def ReadBoolean():Boolean =
  {
    return ReadByte() != 0;
  }
  
  def ReadByte():Byte =
  {
    return s._input.read().toByte;
  }
  def ReadUInt16():Int =
  {
    FillInternalBuffer(2);
    return _byteBuffer.getShort(0);
  }
  def ReadInt16():Short =
  {
    FillInternalBuffer(2);
    return _byteBuffer.getShort(0);
  }
  def ReadInt32():Int =
  {
    FillInternalBuffer(4);
    return _byteBuffer.getInt(0);
  }
  def ReadUInt32():Long =
  {
    FillInternalBuffer(4);
    return _byteBuffer.getInt(0);
  }
  def ReadString():String =
  {
    val length = this.Read7BitEncodedInt();
    if (length == 0)
      return "";
	val bytes = this.FillInternalBuffer(length);
	
	return new String(bytes, 0, length, "UTF-8");
  }
  def ReadSingle():Float =
  {
    FillInternalBuffer(4);
    return _byteBuffer.getFloat(0);
  }
  def ReadInt64():Long =
  {
    FillInternalBuffer(8);
    return _byteBuffer.getLong(0);
  }
  
  def Dispose()
  {
    
  }
}