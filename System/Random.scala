package System

class Random 
{
  val _rnd = new java.security.SecureRandom();


  def Next():Int =
  {
    return _rnd.nextInt();
  }

  def Next(max:Int):Int =
  {
    return _rnd.nextInt(max);
  }

  def Next(min:Int, max:Int):Int =
  {
    return _rnd.nextInt(max - min) + min;
  }
  
  def NextDouble():Double =
  {
    return _rnd.nextDouble();
  }

}