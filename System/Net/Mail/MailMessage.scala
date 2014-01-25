package System.Net.Mail;

class MailMessage {
  final val ReplyToList = new MailAddressCollection();
  final val To = new MailAddressCollection();
  var Body: String = null;
  var From: MailAddress = null;
  var IsBodyHtml: Boolean = false;
  var Subject: String = null;
}