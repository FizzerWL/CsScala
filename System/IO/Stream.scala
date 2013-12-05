package System.IO

import System.NotImplementedException
import java.io._;

class Stream(input:InputStream, output:OutputStream, textinput:Reader, textoutput:Writer)
{
  val _input:InputStream = input;
  val _output:OutputStream = output;
  val _textinput:Reader = textinput;
  val _textoutput:Writer = textoutput;

  def Dispose()
  {
    if (_input != null)
      _input.close();
    if (_output != null)
      _output.close();
    if (_textinput != null)
      _textinput.close();
    if (_textoutput != null)
      _textoutput.close();
  }
  
  def Close()
  {
    Dispose();
  }
  
  def Write(bytes:Array[Byte], offset:Int, length:Int)
  {
    _output.write(bytes, offset, length);
  }
  def Read(bytes:Array[Byte], offset:Int, length:Int):Int =
  {
    return _input.read(bytes, offset, length);
  }
}