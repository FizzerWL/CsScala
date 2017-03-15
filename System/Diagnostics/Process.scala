package System.Diagnostics;
import System.NotImplementedException;

object Process {

  final val _current = new Process("java"); //simple hack so that GetCurrentProcess().ProcessName works
  def GetCurrentProcess(): Process = _current;

  def Start(path: String): Process =
    {
      throw new NotImplementedException();
    }
}

class Process(name: String) {

  var HasExited: Boolean = false;
  var ProcessName: String = name;

  def Kill() {
    throw new NotImplementedException();
  }

}