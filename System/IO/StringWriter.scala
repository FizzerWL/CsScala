package System.IO

class StringWriter extends TextWriter(new java.io.StringWriter()) {

  override def toString():String = _writer.toString();
}