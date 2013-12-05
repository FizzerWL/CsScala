package System.Configuration

import System.Collections.Generic.Dictionary
import org.jdom2.input.SAXBuilder
import scala.collection.JavaConversions._

object ConfigurationManager
{

  
  final val AppSettings = new Dictionary[String, String]();
  
  
  //TODO: For now, user projects must call ConfigurationManager.init with a path to their config file. 
  def init(configFilePath:String)
  {  
    val builder = new SAXBuilder();
    
    val doc = builder.build(new java.io.File(configFilePath));
    val root = doc.getRootElement();
    for (e <- root.getChildren("add"))
      AppSettings.Add(e.getAttributeValue("key"), e.getAttributeValue("value"));
    
    
   
  }
}

