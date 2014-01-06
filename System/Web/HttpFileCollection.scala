package System.Web

import System.NotImplementedException

class HttpFileCollection {
  
  def Count:Int = { throw new NotImplementedException(); }
  
  def apply(indx:Int):HttpPostedFile = { throw new NotImplementedException(); }

}