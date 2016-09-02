package System.Collections.Specialized

import java.util.TreeMap
import scala.collection.JavaConverters._

class NameValueCollection(caseInsensitive: Boolean = false) {
  private val _map = new TreeMap[String, String]();
  private val _lowerMap = new TreeMap[String, String]();

  def apply(name: String): String =
    {
      if (caseInsensitive)
        return _lowerMap.get(name.toLowerCase());
      else
        return _map.get(name);
    }

  def GetKey(index: Int): String = _map.keySet().toArray()(index).asInstanceOf[String];
  def apply(index: Int): String = _map.get(GetKey(index));

  def AllKeys: Array[String] = {
    return _map.keySet().asScala.toArray;
  }

  def update(name: String, value: String) {
    _lowerMap.put(name.toLowerCase(), value);
    _map.put(name, value);
  }

  def Count: Int = _map.size();

  override def toString(): String = _map.toString();

}