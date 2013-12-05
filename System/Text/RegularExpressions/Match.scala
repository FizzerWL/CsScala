package System.Text.RegularExpressions;

import java.util.regex.Matcher

class Match(matcher:Matcher)
{

	val Groups = new GroupCollection(matcher);
}