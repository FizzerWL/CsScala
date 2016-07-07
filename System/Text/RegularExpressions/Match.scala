package System.Text.RegularExpressions;

import java.util.regex.Matcher
import System.NotImplementedException

class Match(matcher:Matcher, findNext:Boolean = true)
{
	val Groups = new GroupCollection(matcher, findNext);
	def Success:Boolean = !Groups._isEmpty;
}