package System.Text.RegularExpressions

import java.util.regex.Matcher

class MatchCollection(matcher: Matcher) extends Iterable[Match] {

  //TODO: Allow iterating more than once
  def iterator: Iterator[Match] = new Iterator[Match] {
    override def hasNext: Boolean = matcher.find();
    override def next(): Match = new Match(matcher, false);
  }

}