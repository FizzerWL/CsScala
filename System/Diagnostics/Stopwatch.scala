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
  
  def Elapsed:TimeSpan = TimeSpan.FromMilliseconds((java.lang.System.nanoTime() - _started) / 1000000.0);
  def ElapsedMilliseconds:Long = (java.lang.System.nanoTime() - _started) / 1000000L;
  
  def Restart()
  {
    _started = java.lang.System.nanoTime();
  }
}