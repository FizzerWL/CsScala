package System.Net

import java.net.HttpURLConnection
import System.Uri


object HttpWebRequest {
  def Create(url: String): WebRequest = Create(new Uri(url));
  def Create(url: Uri): WebRequest = new HttpWebRequest(url._url.openConnection().asInstanceOf[HttpURLConnection])
}
class HttpWebRequest(_req: HttpURLConnection) extends WebRequest(_req) {

}