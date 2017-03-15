package System

class Random 
{
  val _rnd = new java.security.SecureRandom();
  def Next(max:Int):Int =
  {
    return _rnd.nextInt(max);
  }
  
  def NextDouble():Double =
  {
    return _rnd.nextDouble();
  }

}