package System.Net.Mail;

class MailMessage
{

	
	var ReplyToList:MailAddressCollection = null;
	var To:MailAddressCollection = null;
	var Body:String = null;
	var From:MailAddress = null;
	var IsBodyHtml:Boolean = false;
	var Subject:String = null;
}