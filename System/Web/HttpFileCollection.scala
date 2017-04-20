package System.Web

import java.util.ArrayList

class HttpFileCollection extends Traversable[HttpPostedFile] 
{

  final val Collection = new ArrayList[HttpPostedFile]();
  
  def Count = Collection.size();

  def apply(indx: Int): HttpPostedFile = {
    return Collection.get(indx);
  }
  
  def foreach[U](fn:HttpPostedFile=>U)
  {
    val it = Collection.iterator();
    while (it.hasNext())
      fn(it.next());
  }

}