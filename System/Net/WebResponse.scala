package System.Net;
import System.IO.Stream
import java.io.FileNotFoundException
import java.io.IOException
import java.net.HttpURLConnection

class WebResponse(_req: HttpURLConnection, _isError: Boolean) {

  val ContentLength: Int = _req.getContentLength();
  def GetResponseStream(): Stream =
    {
      if (_isError)
        return new Stream(_req.getErrorStream(), null, null, null);

      try {
        return new Stream(_req.getInputStream(), null, null, null)
      } catch {
        case e: FileNotFoundException               => throw new WebException("404 file not found", e, new HttpWebResponse(_req, null, true));
        case e: IOException                         => throw new WebException(e.getMessage(), e, new HttpWebResponse(_req, null, true));
        case e: java.net.ConnectException           => throw new WebException(e.getMessage(), e, new HttpWebResponse(_req, null, true));
        case e: java.net.SocketException            => throw new WebException(e.getMessage(), e, new HttpWebResponse(_req, null, true));
        case e: javax.net.ssl.SSLHandshakeException => throw new WebException(e.getMessage(), e, new HttpWebResponse(_req, null, true));
      }
    }

  def Headers(name: String): String = _req.getHeaderField(name);

  def Dispose():Unit = {

  }
  def Close():Unit = {
    
  }
}