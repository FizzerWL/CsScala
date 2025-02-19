package System.Data

import System.DateTime

trait IDataReader 
{
  def Read():Boolean;

  def apply(fieldName: String): Any;
  def apply(index: Int): Any;
  
  def GetString(col:Int):String;
  def GetInt32(col:Int):Int;
  def GetInt16(col:Int):Short;
  def GetInt64(col:Int):Long;
  def GetBoolean(col:Int):Boolean;
  def GetDouble(col:Int):Double;
  def GetDateTime(col:Int):DateTime;

  def Dispose():Unit;

}