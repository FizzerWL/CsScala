package System.Net;

class Cookie(name: String, value: String, path:String = null, domain:String = null) {

  var Name = name;
  var Value = value;
  var Path = path;
  var Domain = domain;
  
  //.net seems OK with having just a host name as the domain, but the jvm expects a full uri.  Doesn't seem to matter if it's http or https, it still works.
  if (Domain != null && Domain.indexOf("://") == -1)
    Domain = "https://" + Domain;
}