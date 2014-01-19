package System

import java.util.Date
import java.text.DateFormat
import java.text.SimpleDateFormat
import org.joda.time.format.DateTimeFormat


object DateTime
{
  val MinValue = new DateTime(0L);
  val MaxValue = new DateTime(3155378975999999999L);
  def Parse(s:String):DateTime = new DateTime(new org.joda.time.DateTime(new java.util.Date(s).getTime()));
  def Now:DateTime = new DateTime(org.joda.time.DateTime.now());
  
  final val _dateFormat = DateTimeFormat.forPattern("M/d/yyyy HH:mm:ss");
  
  final val _ticksDiff = 621355968000000000L - 288000000000L;
                         
}

class DateTime(d:org.joda.time.DateTime, extraTicks:Long = 0)
{
  val _d = d;
  val _extraTicks = extraTicks;
  
  if (_d == null)
    throw new ArgumentNullException("Null date");
  
  def this(year:Int, month:Int, day:Int)
  {
    this(new org.joda.time.DateTime(year, month, day, 0, 0));
  }
  
  def this(ticks:Long)
  {
    this(new org.joda.time.DateTime((ticks - DateTime._ticksDiff) / 10000L), ticks % 10000);
  }
  
  def Ticks:Long = _d.getMillis() * 10000 + DateTime._ticksDiff + extraTicks;

    
  def this()
  {
    this(new org.joda.time.DateTime(0L))
  }
  
  def this(jd:java.util.Date)
  {
    this(new org.joda.time.DateTime(jd.getTime()));
  }
  
  override def equals(other:Any):Boolean = 
  {
    if (!other.isInstanceOf[DateTime])
      return false;
    
    val otherDate = other.asInstanceOf[DateTime];
    
    return _d.getMillis() == otherDate._d.getMillis() && _extraTicks == otherDate._extraTicks;
  }
  
  override def toString():String = DateTime._dateFormat.print(_d);
  
  def toString(format:String):String =
  {
    val sdf = format.replace('f', 'S') //milliseconds are f in .net and S in SimpleDateFormat
    return DateTimeFormat.forPattern(sdf).print(_d);
  }
  def ToShortDateString():String = Year + "/" + Month + "/" + Day;

  def Year:Int = _d.getYear();
  def Month:Int = _d.getMonthOfYear();
  def Day:Int = _d.getDayOfMonth();
  def Hour:Int = _d.getHourOfDay();
  def Minute:Int = _d.getMinuteOfHour();
  def Second:Int = _d.getSecondOfMinute();

  def Subtract(other:DateTime):TimeSpan = new TimeSpan(this.Ticks - other.Ticks);
  def Subtract(other:TimeSpan):DateTime = new DateTime(this.Ticks - other.Ticks);
  def Add(span:TimeSpan):DateTime = new DateTime(Ticks + span.Ticks);
  def AddYears(num:Double):DateTime = Add(TimeSpan.FromYears(num));
  def AddDays(num:Double):DateTime = Add(TimeSpan.FromDays(num)); 
  def AddHours(num:Double):DateTime = Add(TimeSpan.FromHours(num));
  def AddMinutes(num:Double):DateTime = Add(TimeSpan.FromMinutes(num));
  def AddMilliseconds(num:Double):DateTime = Add(TimeSpan.FromMilliseconds(num));
  def AddSeconds(num:Double):DateTime = Add(TimeSpan.FromSeconds(num));

  def ToUniversalTime():DateTime = this;  //TODO
  def ToLocalTime():DateTime = this; //TODO
}
