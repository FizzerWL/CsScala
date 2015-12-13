package System.Collections.Specialized

import java.util.TreeMap
import scala.collection.JavaConverters._

class NameValueCollection(caseInsensitive:Boolean = false) 
{
  private val _map = new TreeMap[String, String]();

  def apply(name:String):String = 
  {
    if (caseInsensitive)
      return _map.get(name.toLowerCase());
    else
      return _map.get(name);
  }
  
  def GetKey(index:Int):String = _map.keySet().toArray()(index).asInstanceOf[String];
  def apply(index:Int):String = _map.get(GetKey(index));
  
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