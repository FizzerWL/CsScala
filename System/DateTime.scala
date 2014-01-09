package System

import java.util.Date
import java.text.DateFormat
import java.text.SimpleDateFormat


object DateTime
{
  def Now:DateTime =
  {
    return new DateTime(new Date());
  }
  
  val MinValue = new DateTime(0);
  val MaxValue = new DateTime(3155378975999999999L);
  def Parse(s:String):DateTime =
  {
    return new DateTime(new Date(s));
  }
  
  //final val _dateFormat = new SimpleDateFormat("MM/d/yyyy hh:mm:ss a");
  final val _dateFormat = new SimpleDateFormat("M/d/yyyy HH:mm:ss");
}

class DateTime(d:Date)
{
  val _d:Date = d;
  
  if (_d == null)
    throw new ArgumentNullException("Null date");
  
  def this(year:Int, month:Int, day:Int)
  {
    this(new Date(year - 1900, month - 1, day));
  }
  
  def this(ticks:Long)
  {
    this(new Date(ticks / 10000));
  }
  def this()
  {
    this(new Date(0))
  }
  
  override def equals(other:Any):Boolean = 
  {
    if (!other.isInstanceOf[DateTime])
      return false;
    return other.asInstanceOf[DateTime]._d.getTime() == _d.getTime();
  }
  
  override def toString():String =
  {
    return DateTime._dateFormat.format(_d);
  }
  
  def toString(format:String):String =
  {
    val sdf = format.replace('f', 'S') //milliseconds are f in .net and S in SimpleDateFormat
    return new SimpleDateFormat(sdf).format(_d);
  }
  def ToShortDateString():String =
  {
    return Year + "/" + Month + "/" + Day;
  }


  def Year:Int =
  {
    return _d.getYear() + 1900;
  }
  def Month:Int =
  {
    return _d.getMonth() + 1;
  }
  def Day:Int =
  {
    return _d.getDay();
  }
  def Hour:Int =
  {
    return _d.getHours();
  }
  def Minute:Int =
  {
    return _d.getMinutes();
  }
  def Second:Int =
  {
    return _d.getSeconds();
  }
  def Ticks:Long =
  {
    return _d.getTime() * 10000;
  }

  def ToUniversalTime():DateTime = 
  {
    return this; //TODO
  }
  def ToLocalTime():DateTime = 
  {
    return this; //TODO
  }
  
  def Subtract(other:DateTime):TimeSpan =
  {
    return TimeSpan.FromMillisecondsL(this._d.getTime() - other._d.getTime());
  }
  def Subtract(other:TimeSpan):DateTime =
  {
    throw new NotImplementedException();
  }
  def Add(span:TimeSpan):DateTime =
  {
    return new DateTime(new Date(span.TotalMillisecondsL + _d.getTime()));
  }
  def AddYears(num:Double):DateTime =
  {
    return new DateTime(new Date(TimeSpan.FromYears(num).TotalMillisecondsL + this._d.getTime()));
  }
  def AddDays(num:Double):DateTime =
  {
    return new DateTime(new Date(TimeSpan.FromDays(num).TotalMillisecondsL + this._d.getTime()));
  }
  def AddHours(num:Double):DateTime =
  {
    return new DateTime(new Date(TimeSpan.FromHours(num).TotalMillisecondsL + this._d.getTime()));
  }
  def AddMinutes(num:Double):DateTime =
  {
    return new DateTime(new Date(TimeSpan.FromMinutes(num).TotalMillisecondsL + this._d.getTime()));
  }
  def AddMilliseconds(num:Double):DateTime =
  {
    return new DateTime(new Date((num + this._d.getTime()).toLong));
  }
  def AddSeconds(num:Double):DateTime =
  {
    return new DateTime(new Date(TimeSpan.FromSeconds(num).TotalMillisecondsL + this._d.getTime()));
  }
}
