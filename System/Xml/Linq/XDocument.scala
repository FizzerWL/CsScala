package System.Xml.Linq

import System.NotImplementedException

object XDocument
{
  def Parse(str:String):XDocument = 
  {
    throw new NotImplementedException();
  }
}

class XDocument extends XContainer 
{

  def Root:XElement =
  {
    throw new NotImplementedException();
  }
}