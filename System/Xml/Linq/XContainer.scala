package System.Xml.Linq

import System.NotImplementedException

class XContainer 
{
  def Add(obj:Any)
  {
    throw new NotImplementedException();
  }
  
  def Elements():Traversable[XElement] =
  {
    throw new NotImplementedException();
  }
  def Attributes():Traversable[XAttribute] =
  {
    throw new NotImplementedException();
  }
  def Element(name:String):XElement =
  {
    throw new NotImplementedException();
  }

}