package System.Web

import System.NotImplementedException
import System.Collections.Specialized.NameValueCollection
import System.NotImplementedException
import org.apache.commons.lang3.StringEscapeUtils
import java.net.URLEncoder
import java.net.URLDecoder

object HttpUtility 
{
  def UrlEncode(s:String):String =
  {
    return URLEncoder.encode(s, "UTF-8");
  }
  
  def UrlDecode(s:String):String = 
  {
    //TODO: This does not behave the same way as the .net version on invalid sequences.  For example, the string "I like 2% Milk" returns unchanged in .net, but this function throws. 
    return URLDecoder.decode(s, "UTF-8");
  }
  
  def HtmlEncode(s:String):String =
  {
    if (s == null)
      return "";
    
    return StringEscapeUtils.escapeHtml4(s).replace("'", "&#39;");
  }
  
  def JavaScriptStringEncode(s:String, addQuotes:Boolean):String =
  {
    if (s == null)
      return if (addQuotes) "\"\"" else "";
    val encoded = StringEscapeUtils.escapeEcmaScript(s);
    return if (addQuotes) "\"" + encoded + "\"" else encoded; 
  }
  
  def HtmlAttributeEncode(s:String):String = 
  {
    return HtmlEncode(s).replaceAll("'", "&#x27;");
  }
  
  def ParseQueryString(query:String):NameValueCollection =
  {
    val ret = new NameValueCollection();
    
    val pairs = query.split("&");
    for (pair <- pairs) {
        val idx = pair.indexOf("=");
        ret(URLDecoder.decode(pair.substring(0, idx), "UTF-8")) = URLDecoder.decode(pair.substring(idx + 1), "UTF-8");
    }
    return ret;
  }

}