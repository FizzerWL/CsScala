package System.Net.Mail;

import java.util.ArrayList

class MailAddressCollection {

  final val _coll = new ArrayList[MailAddress]();
  def Add(addr: MailAddress) { _coll.add(addr); }
    
}