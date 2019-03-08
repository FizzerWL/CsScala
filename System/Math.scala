package System

object Math
{
	def Round(value:Float):Float = {
		Round(value, 0)
	}
	def Round(value:Float, decimals:Int):Float = {
		val intValue = java.lang.Math.floor(value).floatValue();
		val decimalValue = value - intValue;
		if (decimals > 0) {
			intValue + new java.math.BigDecimal(decimalValue).round(new java.math.MathContext(decimals, java.math.RoundingMode.HALF_EVEN)).floatValue()
		}
		else if (decimalValue > 0.5f) {
			intValue + 1f
		}
		else if (decimalValue < 0.5f) {
			intValue
		}
		else {
			if ((intValue % 2) == 0) {
				intValue
			}
			else {
				intValue + 1f
			}
		}
	}

	def Round(value:Double):Double = {
		Round(value, 0)
	}
	def Round(value:Double, decimals:Int):Double = {
		val intValue = java.lang.Math.floor(value).doubleValue();
		val decimalValue = value - intValue;
		if (decimals > 0) {
			intValue + new java.math.BigDecimal(decimalValue).round(new java.math.MathContext(decimals, java.math.RoundingMode.HALF_EVEN)).doubleValue()
		}
		else if (decimalValue > 0.5) {
			intValue + 1f
		}
		else if (decimalValue < 0.5) {
			intValue
		}
		else {
			if ((intValue % 2) == 0) {
				intValue
			}
			else {
				intValue + 1
			}
		}
	}
}
