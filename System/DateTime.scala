package System

import java.util.Date
import java.text.DateFormat


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
}

class DateTime(d:Date)
{
  val _d:Date = d;
  
  def this(year:Int, month:Int, day:Int)
  {
    this(new Date(year, month - 1, day));
  }
  
  def this(ticks:Long = 0)
  {
    this(new Date(ticks / 10000));
  }

  def Year:Int =
  {
    return _d.getYear();
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
    throw new NotImplementedException();
  }
  def ToLocalTime():DateTime = 
  {
    throw new NotImplementedException();
  }
  
  def Subtract(other:DateTime):TimeSpan =
  {
    return TimeSpan.FromMillisecondsL(other._d.getTime() - this._d.getTime());
  }
  def Add(span:TimeSpan):DateTime =
  {
    return new DateTime(new Date(span.TotalMillisecondsL + _d.getTime()));
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
