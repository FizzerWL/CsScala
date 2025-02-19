package System.Web

import java.util.ArrayList
import scala.collection.JavaConverters._

class HttpFileCollection extends Iterable[HttpPostedFile] 
{

  final val Collection = new ArrayList[HttpPostedFile]();
  
  def Count = Collection.size();

  def apply(indx: Int): HttpPostedFile = {
    return Collection.get(indx);
  }
  

  def iterator:Iterator[HttpPostedFile] = Collection.iterator().asScala;

}