package System.Net.Mail;
import System.NotImplementedException;
import java.util.ArrayList

class MailAddressCollection {

  final val _coll = new ArrayList[MailAddress]();
  def Add(addr: MailAddress) { _coll.add(addr); }
    
}