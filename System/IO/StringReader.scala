package System.IO;
import System.NotImplementedException;

class StringReader(str: String) {

  var _pos: Int = 0;

  def ReadLine(): String =
    {
      if (_pos >= str.length)
        return null;

      val newPos = str.indexOf('\n', _pos);
      if (newPos == -1)
      {
        val ret = str.substring(_pos);
        _pos = str.length;
        return Trim(ret);
      }
      
      val ret = str.substring(_pos, newPos - _pos);
      _pos = newPos + 1;
      return Trim(ret);
      
      
    }
  
  private def Trim(s:String):String =
  {
    return System.CsScala.Trim(s, Array('\r'));
  }

  def Dispose() {

  }

}