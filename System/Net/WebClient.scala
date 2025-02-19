package System.Net;

import java.net._;
import java.io._;

class WebClient {

  def DownloadData(url: String): Array[Byte] = {
    val req = WebRequest.Create(url);

    val res = req.GetResponse();
    val stream = res.GetResponseStream();

    val rd = new ByteArrayOutputStream();
    val buf = new Array[Byte](1024);

    var read = 0;
    while {
      read = stream.Read(buf, 0, buf.length);
      if (read != -1)
        rd.write(buf, 0, read);
      read != -1;
    } do ();

    rd.flush();

    return rd.toByteArray();

  }

  def Dispose():Unit = {

  }
}