package System.Net

import System.Uri
import System.IO.Stream
import java.net._;
import java.io._;
import scala.collection.JavaConverters._
import System.IO.MemoryStream

object WebRequest {
  def Create(url: String): WebRequest = Create(new Uri(url));
  def Create(url: Uri): WebRequest = new HttpWebRequest(url._url.openConnection().asInstanceOf[HttpURLConnection])
}

class WebRequest(_req: HttpURLConnection) {
  var Proxy: Any = _;

  java.lang.System.getProperties().setProperty("https.protocols", "TLSv1,TLSv1.1,TLSv1.2");

  def Method: String = _req.getRequestMethod();
  def Method_=(m: String) = _req.setRequestMethod(m);

  def ContentType: String = _req.getRequestProperty("Content-Type");
  def ContentType_=(c: String) = _req.setRequestProperty("Content-Type", c);

  def ContentLength: Int = _req.getRequestProperty("Content-Length").toInt;
  def ContentLength_=(c: Int) = _req.setRequestProperty("Content-Length", c.toString);

  def Timeout: Int = _req.getConnectTimeout();
  def Timeout_=(t: Int) = _req.setConnectTimeout(t);

  def ReadWriteTimeout: Int = _req.getReadTimeout();
  def ReadWriteTimeout_=(t: Int) = _req.setReadTimeout(t);

  def Accept: String = _req.getRequestProperty("Accept");
  def Accept_=(t: String) = _req.setRequestProperty("Accept", t);

  def UserAgent: String = _req.getRequestProperty("User-Agent");
  def UserAgent_=(t: String) = _req.setRequestProperty("User-Agent", t);

  var CookieContainer: CookieContainer = null;
  val Headers = new WebHeaderCollection(_req);

  private var _requestStream: MemoryStream = null;

  def GetResponse(): WebResponse = {
    if (CookieContainer != null && CookieContainer.Count > 0) {
      val msCookieManager = new java.net.CookieManager();
      for (cookie <- CookieContainer._cookies.values().asScala) {
        val c = new java.net.HttpCookie(cookie.Name, cookie.Value);
        c.setVersion(0);
        msCookieManager.getCookieStore().add(new java.net.URI(cookie.Domain), c);
      }

      _req.setRequestProperty("Cookie", msCookieManager.getCookieStore().getCookies().asScala.mkString(";"))
    }
    try {
      if (_requestStream != null) {
        _req.setDoOutput(true);
        _req.getOutputStream().write(_requestStream.ToArray());
      }

      return new HttpWebResponse(_req, CookieContainer, false);
    } catch {
      case ex: ConnectException     => throw new WebException(ex.getMessage(), ex);
      case ex: UnknownHostException => throw new WebException(ex.getMessage(), ex);
    }
  }
  def GetRequestStream(): Stream = {
    if (_requestStream == null)
      _requestStream = new MemoryStream(500);
    return _requestStream;
  }
}