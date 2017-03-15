package System

object TimeSpan
{
  final val MaxValue = new TimeSpan(9223372036854775807L);
  final val MinValue = new TimeSpan(-9223372036854775808L);
  final val Zero = new TimeSpan(0);
  
  def FromTicks(ticks:Long):TimeSpan = new TimeSpan(ticks);
  def FromMilliseconds(mills:Double):TimeSpan = new TimeSpan((mills * 10000.0).toLong);
  def FromSeconds(secs:Double):TimeSpan = new TimeSpan((secs *     10000000.0).toLong);
  def FromMinutes(mins:Double):TimeSpan = new TimeSpan((mins *    600000000.0).toLong);
  def FromHours(hours:Double):TimeSpan = new TimeSpan((hours *  36000000000.0).toLong);
  def FromDays(days:Double):TimeSpan = new TimeSpan((days *    864000000000.0).toLong);
  def FromYears(years:Double):TimeSpan = new TimeSpan((years *     3.15569e14).toLong);
}

class TimeSpan(ticks:Long = 0)
{
  final val Ticks:Long = ticks;
  
  override def toString():String =
  {
    return TotalDays.toInt + "." + Hours + ":" + Minutes + ":" + Seconds + "." + Milliseconds;
  }

  def TotalMilliseconds:Double = Ticks / 10000.0;
  def TotalSeconds:Double = Ticks /   10000000.0; 
  def TotalMinutes:Double = Ticks /  600000000.0;
  def TotalHours:Double = Ticks /  36000000000.0;
  def TotalDays:Double = Ticks /  864000000000.0;

  def Milliseconds:Int = ((Ticks /       10000L) % 1000).toInt;
  def Seconds:Int = ((Ticks /         10000000L) % 60).toInt;
  def Minutes:Int = ((Ticks /        600000000L) % 60).toInt;
  def Hours:Int = ((Ticks /        36000000000L) % 24).toInt;
  def Days:Int = (Ticks /         864000000000L).toInt;
  
  def Add(span:TimeSpan):TimeSpan = new TimeSpan(Ticks + span.Ticks);
  def Subtract(span:TimeSpan):TimeSpan = new TimeSpan(Ticks - span.Ticks);
  
  override def equals(other:Any):Boolean = 
  {
    if (!other.isInstanceOf[TimeSpan])
      return false;
    return other.asInstanceOf[TimeSpan].Ticks == this.Ticks;
  }
}