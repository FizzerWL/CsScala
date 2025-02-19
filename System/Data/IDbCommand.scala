package System.Data

trait IDbCommand {

  def ExecuteReader():IDataReader;
  def ExecuteNonQuery():Int;
  var CommandText:String;
  def Dispose():Unit;
}