package System
import scala.util.control._
import java.util.UUID
import java.nio.ByteBuffer
import scala.collection.mutable.ArrayBuffer
import scala.reflect._
import java.util.ArrayList
import scala.collection.JavaConverters._
import org.apache.http.util.ExceptionUtils
import java.nio.ByteOrder

object CsScala {
  val cscontinue = new Breaks;
  val csbreak = new Breaks;

  val ushortMaxValue = 65535;
  val ushortMinValue = 0;
  val uintMinValue = 0;
  val uintMaxValue = 4294967295L;

  def Join(seperator: String, strs: Traversable[String]): String =
    {
      return strs.mkString(seperator);
    }

  def TryParseInt(s: String, out: CsRef[Int]): Boolean =
    {
      try {
        out.Value = s.toInt;
        return true;
      }
      catch {
        case e: NumberFormatException => return false;
      }
    }
  def TryParseLong(s: String, out: CsRef[Long]): Boolean =
    {
      try {
        out.Value = s.toLong;
        return true;
      }
      catch {
        case e: NumberFormatException => return false;
      }
    }
  def TryParseFloat(s: String, out: CsRef[Float]): Boolean =
    {
      try {
        out.Value = s.toFloat;
        return true;
      }
      catch {
        case e: NumberFormatException => return false;
      }
    }
  def TryParseDouble(s: String, out: CsRef[Double]): Boolean =
    {
      try {
        out.Value = s.toDouble;
        return true;
      }
      catch {
        case e: NumberFormatException => return false;
      }
    }
  def TryParseBoolean(s: String, out: CsRef[Boolean]): Boolean =
    {
      try {
        out.Value = s.toBoolean;
        return true;
      }
      catch {
        case e: NumberFormatException => return false;
      }
    }

  def IsNullOrEmpty(s: String): Boolean =
    {
      return s == null || s.isEmpty();
    }

  def GetType(obj: AnyRef): Type =
    {
      return new Type(obj.getClass());
    }

  def Sort[T](buf: ArrayBuffer[T], cmp: (T, T) => Int) {
    buf.sortWith((f, s) => cmp(f, s) < 0);
  }

  def Sort(buf: ArrayBuffer[Int]) {
    buf.sortBy(a => a);
  }

  def Contains[T](a: Traversable[T], item: T): Boolean =
    {
      for (e <- a)
        if (e == item)
          return true;
      return false;
    }

  def ToArray[T: ClassTag](buf: ArrayList[T]): Array[T] =
    {
      return buf.toArray().asInstanceOf[Array[T]];
    }

  def IsNaN(d: Double): Boolean =
    {
      return d.isNaN();
    }

  def IsInfinity(d: Double): Boolean =
    {
      return d.isInfinity;
    }

  def StringContains(haystack: String, needle: String): Boolean =
    {
      return haystack.indexOf(needle) != -1;
    }

  def As[T >: Null: ClassTag](item: Any): T =
    {
      var c = classTag[T].runtimeClass;

      if (c.isInstance(item))
        return item.asInstanceOf[T];
      else
        return null;

    }

  def Equals(str1: String, str2: String, comparison: Int): Boolean =
    {
      if (comparison == StringComparison.OrdinalIgnoreCase) {
        return str1.equalsIgnoreCase(str2);
      }
      else
        throw new NotImplementedException("mode = " + comparison);
    }

  def IsNullOrWhiteSpace(s: String): Boolean =
    {
      return s == null || s.trim().isEmpty();
    }
  def IsWhiteSpace(c: Char): Boolean = c.isWhitespace;

  def Split(str: String, chrs: Array[Char], options: Int): Array[String] =
    {
      var ret = str.split(chrs);
      if (options == StringSplitOptions.RemoveEmptyEntries)
        return ret.filter(s => !IsNullOrEmpty(s)).toArray;
      else
        return ret;
    }
  def EndsWith(str: String, endsWith: String, comparison: Int): Boolean =
    {
      if (comparison == StringComparison.OrdinalIgnoreCase)
        return str.regionMatches(true, str.length - endsWith.length, endsWith, 0, endsWith.length);
      else
        throw new NotImplementedException();
    }
  def StartsWith(str: String, startsWith: String, comparison: Int): Boolean =
    {
      if (comparison == StringComparison.OrdinalIgnoreCase)
        return str.regionMatches(true, 0, startsWith, 0, startsWith.length);
      else
        throw new NotImplementedException();
    }

  def Coalesce[T >: Null](a: T, b: T): T =
    {
      if (a == null)
        return b;
      else
        return a;
    }

  def Contains[T](a: Array[T], item: T): Boolean =
    {
      var i = 0;
      while (i < a.length) {
        if (a(i) == item)
          return true;
        i += 1;
      }
      return false;
    }

  def Trim(str: String, chars: Array[Char]): String =
    {
      if (str.length == 0)
        return str;

      var i: Int = 0;
      while (Contains(chars, str(i)) && i < str.length)
        i += 1;

      var e: Int = str.length - 1;
      while (Contains(chars, str(e)) && e > 0)
        e -= 1;

      if (e < 0)
        return "";

      return str.substring(i, e + 1);
    }
  def TrimEnd(str: String, chars: Array[Char]): String =
    {
      if (str.length == 0)
        return str;

      var e: Int = str.length - 1;
      while (Contains(chars, str(e)) && e > 0)
        e -= 1;

      if (e < 0)
        return "";

      return str.substring(0, e + 1);
    }
  def TrimStart(str: String, chars: Array[Char]): String =
    {
      if (str.length == 0)
        return str;

      var i: Int = 0;
      while (Contains(chars, str(i)) && i < str.length)
        i += 1;

      return str.substring(i);

    }
  def Remove(s: String, startIndex: Int): String =
    {
      return s.substring(0, startIndex);
    }
  def Remove(s: String, startIndex: Int, count: Int): String =
    {
      return s.substring(0, startIndex) + s.substring(startIndex + count);
    }

  def IsUpper(c: Char): Boolean =
    {
      return java.lang.Character.isUpperCase(c);
    }
  def ToLower(c: Char): Char =
    {
      return java.lang.Character.toLowerCase(c);
    }
  def IsLetterOrDigit(c: Char): Boolean = c.isLetterOrDigit;
  def IsDigit(c: Char): Boolean = c.isDigit;
  def IsLetter(c: Char):Boolean = c.isLetter;

  def ExceptionMessage(e: Exception): String =
    {
      val msg = e.getMessage();
      if (msg == null)
        return "<<no message>>";
      else
        return msg;
    }

  def Substring(s: String, startIndex: Int, len: Int): String =
    {
      return s.substring(startIndex, len + startIndex);
    }

  def Substring(s: String, startIndex: Int): String =
    {
      return s.substring(startIndex);
    }

  //Simulates the C# method of returning an empty string for null nullables
  def NullableToString(i: java.lang.Integer): String =
    {
      if (i == null)
        return "";
      else
        return i.toString();
    }
  def NullableToString(i: java.lang.Float): String =
    {
      if (i == null)
        return "";
      else
        return i.toString();
    }
  def NullableToString(i: java.lang.Double): String =
    {
      if (i == null)
        return "";
      else
        return i.toString();
    }
  @inline def BooleanToString(b:Boolean):String = if (b) "True" else "False"; 

  def ExceptionToString(ex: Throwable): String =
    {
      if (ex == null)
        return "";
      else {
        return ex.getClass().getName() + ": " + ex.getMessage() + "\n" + ex.getStackTrace().map(o => "    " + o).mkString("\n") + (if (ex.getCause() == ex) "" else ("\n\n" + ExceptionToString(ex.getCause())));
      }
    }

  def Concat(strs: Traversable[String]): String = strs.mkString("");

  def IndexOf(str:String, needle:String, compType:Int):Int =
  {
    if (compType == StringComparison.OrdinalIgnoreCase)
      return str.toLowerCase().indexOf(needle.toLowerCase());
    else
      return str.indexOf(needle);
  }
  def IndexOfAny(str: String, chars: Array[Char], startIndex: Int = 0): Int =
    {
      var i = startIndex;
      while (i < str.length()) {
        val char = str.charAt(i)
        for (c <- chars)
          if (c == char)
            return i;
        i += 1;
      }

      return -1;
    }
  
  
  def EnumTryParse(str:String, out:CsRef[Int], fn:String=>Int):Boolean =
  {
    try {

      out.Value = fn(str);
      return true;
    }
    catch {
      case e: MatchError =>
        return false;
    }
  }
  

  @inline def NullCheck(str: String): String = if (str == null) "" else str;
  @inline def NullCheck(i: java.lang.Integer): String = if (i == null) "" else i.toString();
  @inline def NullCheck(i: java.lang.Double): String = if (i == null) "" else i.toString();
  @inline def NullCheck(d: DateTime): String = if (d == null) "" else d.toString();
  @inline def NullCheck(b: java.lang.Boolean): String = if (b == null) "" else b.toString();
  @inline def ByteToInt(b: Byte): Int = b & 0xFF;
  @inline def ByteToString(b: Byte): String = (b & 0xFF).toString();

  def SwapEndian(i: Int): Int = java.lang.Integer.reverseBytes(i);
  def SwapEndian(i: Long): Long = java.lang.Long.reverseBytes(i);
  
  def EmptyGuid():UUID = new UUID(0,0); 
}