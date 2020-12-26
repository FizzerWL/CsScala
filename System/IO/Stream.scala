package System.IO

import java.io._;

class Stream(input:InputStream, output:OutputStream, textinput:Reader, textoutput:Writer)
{
  var _input:InputStream = input;
  var _output:OutputStream = output;
  var _textinput:Reader = textinput;
  var _textoutput:Writer = textoutput;
  final var CanRead = true;

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