package System.Net.Mail;
import System.NotImplementedException;

class MailAddress(addr:String, displayAs:String = "")
{
  final val Address = addr;
  final val DisplayAs = displayAs;
}