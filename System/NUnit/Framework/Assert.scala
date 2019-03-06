package NUnit.Framework

object Assert {

  def Fail() = {
    org.junit.Assert.fail()
  }

  def Fail(msg: String) = {
    org.junit.Assert.fail(msg)
  }

  def IsTrue(a: Boolean) = {
    org.junit.Assert.assertTrue(a)
  }
  def IsTrue(a: Boolean, msg: String) = {
    org.junit.Assert.assertTrue(a)
  }

  def IsFalse(a: Boolean) = {
    org.junit.Assert.assertFalse(a)
  }

  def IsFalse(a: Boolean, msg: String) = {
    org.junit.Assert.assertFalse(msg, a)
  }

  def IsNull(a: Any) = {
    org.junit.Assert.assertNull(a)
  }
  def IsNull(a: Any, msg: String) = {
    org.junit.Assert.assertNull(msg, a)
  }

  def IsNotNull(a: Any) = {
    org.junit.Assert.assertNotNull(a)
  }
  def IsNotNull(a: Any, msg: String) = {
    org.junit.Assert.assertNotNull(msg, a)
  }

  def AreEqual(a: String, b: String) = {
    org.junit.Assert.assertEquals(a, b)
  }

  def AreEqual(a: Short, b: Short) = {
    org.junit.Assert.assertEquals(a, b)
  }

  def AreEqual(a: Int, b: Int) = {
    org.junit.Assert.assertEquals(a, b)
  }

  def AreEqual(a: Float, b: Float) = {
    org.junit.Assert.assertEquals(a, b, 0f)
  }

  def AreEqual(a: Double, b: Double) = {
    org.junit.Assert.assertEquals(a, b, 0d)
  }

  def AreEqual(a: Long, b: Long) = {
    org.junit.Assert.assertEquals(a, b)
  }

  def AreEqual(a: Float, b: Float, delta: Double) = {
    org.junit.Assert.assertEquals(a, b, delta)
  }

  def AreEqual(a: Double, b: Double, delta: Double) = {
    org.junit.Assert.assertEquals(a, b, delta)
  }

  def AreEqual(a: Any, b: Any) = {
    org.junit.Assert.assertEquals(a, b)
  }

  def AreEqual(a: Any, b: Any, msg: String) = {
    org.junit.Assert.assertEquals(msg, a, b)
  }

  def AreEqual(a: String, b: String, msg: String) = {
    org.junit.Assert.assertEquals(msg, a, b)
  }

  def AreEqual(a: Short, b: Short, msg: String) = {
    org.junit.Assert.assertEquals(msg, a, b)
  }

  def AreEqual(a: Int, b: Int, msg: String) = {
    org.junit.Assert.assertEquals(msg, a, b)
  }

  def AreEqual(a: Float, b: Float, msg: String) = {
    org.junit.Assert.assertEquals(msg, a, b, 0f)
  }

  def AreEqual(a: Double, b: Double, msg: String) = {
    org.junit.Assert.assertEquals(msg, a, b, 0d)
  }

  def AreEqual(a: Long, b: Long, msg: String) = {
    org.junit.Assert.assertEquals(msg, a, b)
  }

  def AreNotEqual(a: String, b: String) = {
    org.junit.Assert.assertNotEquals(a, b)
  }

  def AreNotEqual(a: Short, b: Short) = {
    org.junit.Assert.assertNotEquals(a, b)
  }

  def AreNotEqual(a: Int, b: Int) = {
    org.junit.Assert.assertNotEquals(a, b)
  }

  def AreNotEqual(a: Float, b: Float) = {
    org.junit.Assert.assertNotEquals(a, b)
  }

  def AreNotEqual(a: Double, b: Double) = {
    org.junit.Assert.assertNotEquals(a, b)
  }

  def AreNotEqual(a: Long, b: Long) = {
    org.junit.Assert.assertNotEquals(a, b)
  }

  def AreNotEqual(a: Any, b: Any) = {
    org.junit.Assert.assertNotEquals(a, b)
  }

  def AreNotEqual(a: Any, b: Any, msg: String) = {
    org.junit.Assert.assertNotEquals(msg, a, b)
  }

  def AreNotEqual(a: String, b: String, msg: String) = {
    org.junit.Assert.assertNotEquals(msg, a, b)
  }

  def AreNotEqual(a: Short, b: Short, msg: String) = {
    org.junit.Assert.assertNotEquals(msg, a, b)
  }

  def AreNotEqual(a: Int, b: Int, msg: String) = {
    org.junit.Assert.assertNotEquals(msg, a, b)
  }

  def AreNotEqual(a: Float, b: Float, msg: String) = {
    org.junit.Assert.assertNotEquals(msg, a, b)
  }

  def AreNotEqual(a: Double, b: Double, msg: String) = {
    org.junit.Assert.assertNotEquals(msg, a, b)
  }

  def AreNotEqual(a: Long, b: Long, msg: String) = {
    org.junit.Assert.assertNotEquals(msg, a, b)
  }


  def Greater(a: Int, b: Int) = {
    org.junit.Assert.assertTrue(a > b)
  }
  def Greater(a: Float, b: Float) = {
    org.junit.Assert.assertTrue(a > b)
  }
  def Greater(a: Double, b: Double) = {
    org.junit.Assert.assertTrue(a > b)
  }
  def Greater(a: Long, b: Long) = {
    org.junit.Assert.assertTrue(a > b)
  }
  def Greater(a: Int, b: Int, msg: String) = {
    org.junit.Assert.assertTrue(msg, a > b)
  }
  def Greater(a: Float, b: Float, msg: String) = {
    org.junit.Assert.assertTrue(msg, a > b)
  }
  def Greater(a: Double, b: Double, msg: String) = {
    org.junit.Assert.assertTrue(msg, a > b)
  }
  def Greater(a: Long, b: Long, msg: String) = {
    org.junit.Assert.assertTrue(msg, a > b)
  }

  def GreaterOrEqual(a: Int, b: Int) = {
    org.junit.Assert.assertTrue(a >= b)
  }
  def GreaterOrEqual(a: Float, b: Float) = {
    org.junit.Assert.assertTrue(a >= b)
  }
  def GreaterOrEqual(a: Double, b: Double) = {
    org.junit.Assert.assertTrue(a >= b)
  }
  def GreaterOrEqual(a: Long, b: Long) = {
    org.junit.Assert.assertTrue(a >= b)
  }
  def GreaterOrEqual(a: Int, b: Int, msg: String) = {
    org.junit.Assert.assertTrue(msg, a >= b)
  }
  def GreaterOrEqual(a: Float, b: Float, msg: String) = {
    org.junit.Assert.assertTrue(msg, a >= b)
  }
  def GreaterOrEqual(a: Double, b: Double, msg: String) = {
    org.junit.Assert.assertTrue(msg, a >= b)
  }
  def GreaterOrEqual(a: Long, b: Long, msg: String) = {
    org.junit.Assert.assertTrue(msg, a >= b)
  }

  def Less(a: Int, b: Int) = {
    org.junit.Assert.assertTrue(a < b)
  }
  def Less(a: Float, b: Float) = {
    org.junit.Assert.assertTrue(a < b)
  }
  def Less(a: Double, b: Double) = {
    org.junit.Assert.assertTrue(a < b)
  }
  def Less(a: Long, b: Long) = {
    org.junit.Assert.assertTrue(a < b)
  }
  def Less(a: Int, b: Int, msg: String) = {
    org.junit.Assert.assertTrue(msg, a < b)
  }
  def Less(a: Float, b: Float, msg: String) = {
    org.junit.Assert.assertTrue(msg, a < b)
  }
  def Less(a: Double, b: Double, msg: String) = {
    org.junit.Assert.assertTrue(msg, a < b)
  }
  def Less(a: Long, b: Long, msg: String) = {
    org.junit.Assert.assertTrue(msg, a < b)
  }

  def LessOrEqual(a: Int, b: Int) = {
    org.junit.Assert.assertTrue(a <= b)
  }
  def LessOrEqual(a: Float, b: Float) = {
    org.junit.Assert.assertTrue(a <= b)
  }
  def LessOrEqual(a: Double, b: Double) = {
    org.junit.Assert.assertTrue(a <= b)
  }
  def LessOrEqual(a: Long, b: Long) = {
    org.junit.Assert.assertTrue(a <= b)
  }
  def LessOrEqual(a: Int, b: Int, msg: String) = {
    org.junit.Assert.assertTrue(msg, a <= b)
  }
  def LessOrEqual(a: Float, b: Float, msg: String) = {
    org.junit.Assert.assertTrue(msg, a <= b)
  }
  def LessOrEqual(a: Double, b: Double, msg: String) = {
    org.junit.Assert.assertTrue(msg, a <= b)
  }
  def LessOrEqual(a: Long, b: Long, msg: String) = {
    org.junit.Assert.assertTrue(msg, a <= b)
  }
}
