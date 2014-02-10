package System.Text.RegularExpressions;

import java.util.regex.Matcher

class Match(matcher:Matcher, findNext:Boolean = true)
{

	val Groups = new GroupCollection(matcher, findNext);
}