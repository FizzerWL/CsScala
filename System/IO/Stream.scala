package System.IO

import java.io._;
import System.NotImplementedException

class Stream(input: InputStream, output: OutputStream, textinput: Reader, textoutput: Writer) {
  var _input: InputStream = input;
  var _output: OutputStream = output;
  var _textinput: Reader = textinput;
  var _textoutput: Writer = textoutput;
  final var CanRead = true;

  def Dispose():Unit = {
    if (_input != null)
      _input.close();
    if (_output != null)
      _output.close();
    if (_textinput != null)
      _textinput.close();
    if (_textoutput != null)
      _textoutput.close();
  }

  def Close():Unit = {
    Dispose();
  }

  def Write(bytes: Array[Byte], offset: Int, length: Int):Unit = {
    _output.write(bytes, offset, length);
  }
  def Read(bytes: Array[Byte], offset: Int, length: Int): Int = {
    return _input.read(bytes, offset, length);
  }
  def CopyTo(mem: MemoryStream):Unit = {
    throw new NotImplementedException();
  }
}