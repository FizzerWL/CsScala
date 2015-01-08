package System.Data

trait IDataReader 
{
  def Read():Boolean;

  def apply(fieldName: String): Any;
  def apply(index: Int): Any;

}