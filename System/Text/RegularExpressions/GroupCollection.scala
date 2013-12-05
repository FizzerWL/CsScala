package System.Text.RegularExpressions;
import System.NotImplementedException;
import java.util.regex.Matcher

class GroupCollection(matcher:Matcher)
{
	
	def apply(name:String):Group =
	{
		return new Group(matcher.group(name));
	}
}