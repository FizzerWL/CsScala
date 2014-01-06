package System.Text.RegularExpressions

import System.NotImplementedException
import java.util.regex.Pattern

object Regex {
  def Replace(pattern: String, input: String, eval: Match => String): String =
    {
      val m = new Regex(pattern)._pattern.matcher(input);

      var strindex = 0;
      val ret = new StringBuffer(input.length());
      while (m.find()) {
        val start = m.start();
        val end = m.end();
        
        
        ret.append(input, strindex, start);
        ret.append(eval(new Match(m)));

        strindex = end;
      }

      ret.append(input, strindex, input.length());

      return ret.toString();

    }
  def Replace(pattern: String, input: String, replace: String): String =
    {
      return input.replaceAll(pattern, replace);
    }
  
  def IsMatch(pattern:String, input:String, opts:Int):Boolean = new Regex(pattern, opts).IsMatch(input);
}

class Regex(pattern: String, options: Int = 0) {
  val _pattern = Pattern.compile(pattern, MakeFlags());

  def MakeFlags(): Int =
    {
      var ret = 0;
      if ((options & RegexOptions.IgnoreCase) > 0)
        ret |= Pattern.CASE_INSENSITIVE;
      if ((options & RegexOptions.Multiline) > 0)
        ret |= Pattern.MULTILINE;

      return ret;
    }

  def IsMatch(input: String): Boolean =
    {
      return _pattern.matcher(input).matches();
    }

  def Match(input: String): Match =
    {
      return new Match(_pattern.matcher(input));
    }
  
  def Replace(input:String, eval:Match => String):String = { 
    throw new NotImplementedException();
  }
}