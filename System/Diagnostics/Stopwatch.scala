package System.Diagnostics

import System.TimeSpan

object Stopwatch
{
  def StartNew():Stopwatch =
  {
    return new Stopwatch();
  }
}
class Stopwatch 
{
  var _started = java.lang.System.nanoTime();
  
  def Elapsed:TimeSpan = 
  {
    return TimeSpan.FromMilliseconds((java.lang.System.nanoTime() - _started) / 1000000.0);
  }
  
  val ElapsedMilliseconds:Long = 0;
  
  def Restart()
  {
    _started = java.lang.System.nanoTime();
  }
}