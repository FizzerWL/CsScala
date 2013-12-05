package System;

import java.net.URL

class Uri(str: String) {
  val _url = new URL(str);

  def Query: String = _url.getQuery();

  override def toString(): String =
    {
      return _url.toString();
    }

}