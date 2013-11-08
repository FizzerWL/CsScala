package System.Xml.Linq

import System.NotImplementedException
import System.NotImplementedException
import System.NotImplementedException

class XElement(name:String) extends XContainer
{
  def Attribute(name:String):XAttribute =
  {
    throw new NotImplementedException();
  }
  
  def SetAttributeValue(name:String, value:String)
  {
    throw new NotImplementedException();
  }
  
  def Value:String =
  {
    throw new NotImplementedException();
  }

}