package System

import java.util.concurrent.ThreadLocalRandom

class Random 
{
  val _rnd = new java.security.SecureRandom();
  def Next():Int = _rnd.nextInt();
  def Next(max:Int):Int = _rnd.nextInt(max);
  def Next(min:Int, max:Int):Int = _rnd.nextInt(max - min) + min;
  
  def NextDouble():Double = _rnd.nextDouble();

  //Same as NextDouble() but doesn't use SecureRandom.  Use when we are OK with non-secure-random for increased performance.  About 100x faster.
  def NextDoubleFast():Double = ThreadLocalRandom.current().nextDouble();
}