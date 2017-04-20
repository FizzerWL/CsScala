package System.IO;

import java.io.InputStreamReader

class StreamReader(s: Stream) {

  val _bufReader = new java.io.BufferedReader(if (s._textinput != null) s._textinput else new InputStreamReader(s._input));

  def ReadToEnd(): String =
    {
      var sb = new StringBuilder();
      var line: String = null;
      do {
        line = _bufReader.readLine();
        if (line != null) {
          sb.append(line);
          sb.append("\n");
        }
      } while (line != null);
      return sb.toString();
    }

  def ReadLine(): String =
    {
      return _bufReader.readLine();
    }

  def Dispose() {

  }
}