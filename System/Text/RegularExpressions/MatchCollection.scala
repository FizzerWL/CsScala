package System.Text.RegularExpressions

import java.util.regex.Matcher

class MatchCollection(matcher: Matcher) extends Traversable[Match] {

  def foreach[U](fn: Match => U) {
    while (matcher.find())
      fn(new Match(matcher, false));

  }

}