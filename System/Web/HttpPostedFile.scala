package System.Web

import System.IO.Stream

class HttpPostedFile(contentLength:Int, stream:Stream) 
{
  final val ContentLength = contentLength;
  final val InputStream = stream;

}