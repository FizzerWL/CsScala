package System.Net

import java.net._;
import java.io._;

class WebHeaderCollection(_req: HttpURLConnection) {

  def Add(name: String, value: String):Unit = {
    if (value != null)
      _req.setRequestProperty(name, value);
  }
}