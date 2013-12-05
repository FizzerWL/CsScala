package System.Collections.Specialized

import java.util.TreeMap
import scala.collection.JavaConverters._

class NameValueCollection 
{
  val _map = new TreeMap[String, String]();

  def apply(index:Int):String = 
  {
    return _map.get(index);
  }
  def apply(name:String):String = 
  {
    return _map.get(name);
  }
  
  def Keys:Traversable[String] = {
    return _map.keySet().asScala;
  }
}