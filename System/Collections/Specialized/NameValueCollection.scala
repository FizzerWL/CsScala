package System.Collections.Specialized

import java.util.TreeMap
import scala.collection.JavaConverters._

class NameValueCollection(caseInsensitive:Boolean = false) 
{
  private val _map = new TreeMap[String, String]();

  def apply(index:Int):String = 
  {
    return _map.get(index);
  }
  def apply(name:String):String = 
  {
    if (caseInsensitive)
      return _map.get(name.toLowerCase());
    else
      return _map.get(name);
  }
  
  def AllKeys:Array[String] = {
    return _map.keySet().asScala.toArray;
  }
  
  def update(name:String, value:String)
  {
    if (caseInsensitive)
      _map.put(name.toLowerCase(), value);
    else
      _map.put(name, value);
  }
  
  def Count:Int = _map.size();
  
  override def toString():String = _map.toString();
  
}