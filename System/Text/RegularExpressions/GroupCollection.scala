package System.Text.RegularExpressions;
import System.NotImplementedException
import java.util.regex.Matcher
import java.util.HashMap
import java.util.ArrayList

//Unfortunately, Matcher does not provide a way to get the group names. The only way to know what the group names are is guess and check if it throws, so this algorithm is a bit inefficient.  It could be made simpler by using reflection to read the group names.
class GroupCollection(matcher: Matcher, findNext:Boolean) {

  private final var _currIndex = 0;

  final val _isEmpty = if (findNext) !matcher.find() else false;

  private def tryGetGroupName(name: String): String =
    {
      try {
        return matcher.group(name);
      }
      catch {
        case e: IllegalArgumentException => return null;
      }
    }

  private def Advance() {
    if (matcher.find())
      _currIndex += 1;
    else {
      _currIndex = 0;
      matcher.reset();
      matcher.find();
    }
  }

  def apply(name: String): Group =
    {
      if (!findNext)
        return new Group(tryGetGroupName(name));
      
      if (_isEmpty)
        throw new Exception("Regex did not match");

      val startIndex = _currIndex;

      var ret: String = null;
      while (true) {
        ret = tryGetGroupName(name);
        if (ret != null)
          return new Group(ret);

        Advance();

        if (_currIndex == startIndex)
          throw new Exception("Group name " + name + " not found");

      }
      throw new Exception("never");
    }
  def apply(index: Int): Group = {
    if (!findNext)
      return new Group(matcher.group(index));
    
    if (_isEmpty)
      throw new Exception("Regex did not match");

    val startIndex = _currIndex;
    while (index != _currIndex) {
      Advance();
      if (startIndex == _currIndex)
        throw new Exception("No group at " + index);
    }

    return new Group(matcher.group(1));
  }
}