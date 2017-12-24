package System.Text.RegularExpressions

import java.util.regex.Pattern

object Regex {
  def Replace(input: String, pattern: String, eval: Match => String): String = new Regex(pattern).Replace(input, eval);
  def Replace(input: String, pattern: String, replace: String): String = input.replaceAll(pattern, replace);
  def IsMatch(input: String, pattern: String, opts: Int): Boolean = new Regex(pattern, opts).IsMatch(input);
  def Escape(input: String): String = Pattern.quote(input);
  def Matches(input: String, pattern: String): MatchCollection = new Regex(pattern).Matches(input);
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

  def IsMatch(input: String): Boolean = _pattern.matcher(input).find();
  def Match(input: String): Match = new Match(_pattern.matcher(input));
  def Matches(input: String): MatchCollection = new MatchCollection(_pattern.matcher(input));

  def Replace(input: String, replace: String): String = Replace(input, m => replace);
  def Replace(input: String, eval: Match => String): String = {

    val m = _pattern.matcher(input);

    var strindex = 0;
    val ret = new StringBuffer(input.length());
    while (m.find()) {
      val start = m.start();
      val end = m.end();

      ret.append(input, strindex, start);
      ret.append(eval(new Match(m, false)));

      strindex = end;
    }

    ret.append(input, strindex, input.length());

    return ret.toString();

  }
}