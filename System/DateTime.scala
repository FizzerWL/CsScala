package System

import java.util.Date
import java.text.DateFormat
import java.text.SimpleDateFormat
import org.joda.time.format.DateTimeFormat
import org.joda.time.DateTimeZone
import org.joda.time.format.DateTimeParser
import org.joda.time.ReadableInstant
import org.joda.time.Instant
import org.joda.time.format.DateTimeFormatterBuilder

object DateTime {
  final val MinValueTicks = 0L;
  final val MaxValueTicks = 3155378975999999999L;
  final val MinValue = new DateTime(MinValueTicks);
  final val MaxValue = new DateTime(MaxValueTicks);
  final val MinValueMillis = MinValue._d.getMillis();
  final val MaxValueMillis = MaxValue._d.getMillis();

  private final val _patterns = Array("M/d/yyyy HH:mm:ss",
    "M/d/yyyy hh:mm:ss a",
    "yyyy/MM/dd HH:mm:ss",
    "E, d MMM yyyy HH:mm:ss",
    "E, d MMM yyyy HH:mm:ss Z",
    "yyyy/MM/dd",
    "M/d/yyyy",
    "yyyy-MM-dd");

  final val _parser = new DateTimeFormatterBuilder().append(null, _patterns.map(DateTimeFormat.forPattern(_).getParser())).toFormatter();
  def Parse(s: String): DateTime = { val t = _parser.parseMillis(s); new DateTime(new Instant(t + _currZone.getOffset(t))) };
  
  def TryParse(s:String, out:CsRef[System.DateTime]):Boolean = {
    try { out.Value = Parse(s); return true; }
    catch {
      case e: IllegalArgumentException => return false; 
    }
  }

  def Now: DateTime = new DateTime(Instant.now(), 0, DateTimeKind.Utc).ToLocalTime();

  final val _dateFormat = DateTimeFormat.forPattern("M/d/yyyy HH:mm:ss");

  final val _ticksDiff = 621355968000000000L; //ticks from DateTime.MinValue to 1970/1/1
  final val _currZone = DateTimeZone.getDefault(); //we assume the time zone never changes for the duration of the program.
}

class DateTime(d: org.joda.time.Instant, extraTicks: Long = 0, kind: Int = DateTimeKind.Unspecified) {
  val _d = d;
  val _extraTicks = extraTicks;

  if (_d == null)
    throw new ArgumentNullException("Null date");

  def this(year: Int, month: Int, day: Int, hour: Int, minute: Int, second: Int, k: Int) {
    this(new org.joda.time.DateTime(year, month, day, hour, minute, second, DateTimeZone.UTC).toInstant(), 0, k);
  }

  def this(year: Int, month: Int, day: Int) {
    this(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
  }

  def this(ticks: Long, k: Int) {
    this(new org.joda.time.Instant((ticks - DateTime._ticksDiff) / 10000L), ticks % 10000, k);
  }
  def this(ticks: Long) {
    this(ticks, DateTimeKind.Unspecified);
  }

  def Ticks: Long = _d.getMillis() * 10000 + DateTime._ticksDiff + extraTicks;

  def this() {
    this(new org.joda.time.Instant(0L))
  }

  def this(jd: java.util.Date) {
    this(new org.joda.time.Instant(jd.getTime()));
  }
  def this(jd: java.util.Date, k: Int) {
    this(new org.joda.time.Instant(jd.getTime()), 0, k);
  }

  override def equals(other: Any): Boolean =
    {
      if (!other.isInstanceOf[DateTime])
        return false;

      val otherDate = other.asInstanceOf[DateTime];

      return _d.getMillis() == otherDate._d.getMillis() && _extraTicks == otherDate._extraTicks;
    }
  override def hashCode(): Int = _d.hashCode();

  override def toString(): String = DateTime._dateFormat.print(_d);

  //Note: This toString implementation only supports a small subset of possible format strings.  Be sure to test any you need extensively.
  def toString(format: String): String =
    {
      if (format == null || format.length() == 0)
        return toString();
      
      if (format == "d")
        return ToShortDateString();
      
      val sdf = format.replace('f', 'S') //milliseconds are f in .net and S in SimpleDateFormat
        .replace("ddd", "E"); //day of week is ddd in .net, and E in SimpleDateFormat. (note this will break other strings like "dddd", which are not supported) 
      return DateTimeFormat.forPattern(sdf).print(_d);
    }
  def ToShortDateString(): String = DateTimeFormat.forPattern("M/d/yyyy").print(_d);

  def ToLongString(): String = toString() + " Ticks=" + Ticks + ", Kind=" + DateTimeKind.toString(Kind) + ", ms=" + _d.getMillis();

  def Year: Int = _d.getChronology().year().get(_d.getMillis());
  def Month: Int = _d.getChronology().monthOfYear().get(_d.getMillis());
  def Day: Int = _d.getChronology().dayOfMonth().get(_d.getMillis());
  def Hour: Int = _d.getChronology().hourOfDay().get(_d.getMillis());
  def Minute: Int = _d.getChronology().minuteOfHour().get(_d.getMillis());
  def Second: Int = _d.getChronology().secondOfMinute().get(_d.getMillis());
  def Millisecond: Int = _d.getChronology().millisOfSecond().get(_d.getMillis());

  def Subtract(other: DateTime): TimeSpan = new TimeSpan(this.Ticks - other.Ticks);
  def Subtract(other: TimeSpan): DateTime = new DateTime(this.Ticks - other.Ticks, Kind);
  def Add(span: TimeSpan): DateTime = new DateTime(Ticks + span.Ticks, Kind);
  def AddYears(num: Double): DateTime = Add(TimeSpan.FromYears(num));
  def AddDays(num: Double): DateTime = Add(TimeSpan.FromDays(num));
  def AddHours(num: Double): DateTime = Add(TimeSpan.FromHours(num));
  def AddMinutes(num: Double): DateTime = Add(TimeSpan.FromMinutes(num));
  def AddMilliseconds(num: Double): DateTime = Add(TimeSpan.FromMilliseconds(num));
  def AddSeconds(num: Double): DateTime = Add(TimeSpan.FromSeconds(num));

  def TimeZoneOffsetMillis: Int = DateTime._currZone.getOffset(_d);

  final val Kind: Int = kind;
  def ToUniversalTime(): DateTime = if (Kind == DateTimeKind.Utc) this else new DateTime(_d.minus(TimeZoneOffsetMillis), extraTicks, DateTimeKind.Utc);
  def ToLocalTime(): DateTime = if (Kind == DateTimeKind.Local) this else new DateTime(_d.plus(TimeZoneOffsetMillis), extraTicks, DateTimeKind.Local);

  def getMillis(): Long = if (Kind == DateTimeKind.Utc) _d.getMillis() else _d.getMillis() - TimeZoneOffsetMillis;
}
