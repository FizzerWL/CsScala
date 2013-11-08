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
  val _started = java.lang.System.nanoTime();
  
  def Elapsed:TimeSpan = 
  {
    return TimeSpan.FromMilliseconds((java.lang.System.nanoTime() - _started) / 1000000.0);
  }
}