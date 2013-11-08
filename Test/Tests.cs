using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using CsScala;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace UnitTestProject1
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void HelloWorld()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}");
        }
        [TestMethod]
        public void IfStatement()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            string notInitialized;
            int myNum = 0;
            notInitialized = ""InitMe!"";

            if (myNum > 4)
                myNum = 2;
            else if (notInitialized == ""asdf"")
                myNum = 1;
            else
                myNum = 999;

            Console.WriteLine(myNum == 999 ? ""One"" : ""Two"");
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"
object Utilities
{
    def SomeFunction()
    {
		var notInitialized:String = null;
		var myNum:Int = 0;
		notInitialized = ""InitMe!"";

		if (myNum > 4)
		{
			myNum = 2;
		}
		else if (notInitialized == ""asdf"")
		{
			myNum = 1;
		}
		else
		{
			myNum = 999;
		}

		System.Console.WriteLine(if (myNum == 999) ""One"" else ""Two"");

    }
}");
        }

        [TestMethod]
        public void Loops()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            while (true)
            {
                Console.WriteLine(""hi"");
                break;
            }
			
			while (true)
				Console.WriteLine(""nobreak"");

            for (int i=0;i<50;i=i+1)
                Console.WriteLine(i);

            do
            {
                Console.WriteLine(""Dowhile"");
            }
            while (false);

			while (true)
			{
				if (4 == 5)
					continue;
				
			}

			while (true)
			{
				Console.WriteLine(1);
				break;
				Console.WriteLine(2);
				continue;
				Console.WriteLine(3);

			}
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
	  	CsScala.csbreak.breakable
	  	{
		  	while (true)
	        {
	            System.Console.WriteLine(""hi"");
	            CsScala.csbreak.break;
	        }
	  	}

		while (true)
		{
			System.Console.WriteLine(""nobreak"");
		}
		
        { //for
            var i:Int = 0;
            while (i < 50)
            {
                System.Console.WriteLine(i);
                i = i + 1;
            }
        } //end for
        do
        {
            System.Console.WriteLine(""Dowhile"");
        }
        while (false);  

		while (true)
		{
		    CsScala.cscontinue.breakable
		    {
				if (4 == 5)
				{
	    			CsScala.cscontinue.break;
				}
		    }
		}

		CsScala.csbreak.breakable
		{
		    while (true)
		    {
		    	CsScala.cscontinue.breakable
		    	{
	                System.Console.WriteLine(1);
	    			CsScala.csbreak.break;
	                System.Console.WriteLine(2);
	    			CsScala.cscontinue.break;
	                System.Console.WriteLine(3);
		    	}
		    }
		}

    }
}");
        }


        [TestMethod]
        public void EnumerateOnString()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Linq;

namespace Blargh
{
    public class Foo
    {
        public Foo()
        {
            var s = ""hello"";
			var chars = s.ToCharArray();
			foreach(var c in s)
			{
			}
			s.Select(o => o);
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class Foo
{
	{
		var s:String = ""hello"";
		var chars:Array[Char] = s.toCharArray();
		for (c <- s)
		{
		}
		System.Linq.Enumerable.Select(s, (o:Char) => { o; }:Char);
	}
}");
        }

        [TestMethod]
        public void FieldsAndProperties()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Text;

namespace Blargh
{
    class Box
    {
        private float _width;
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public float SetOnly
        {
            set { Console.WriteLine(value); }
        }

        public int GetOnly
        {
            get { return 4; }
        }
        
        public bool IsRectangular = true;
        public char[] Characters = new char[] { 'a', 'b' };
        public static StringBuilder StaticField = new StringBuilder();
        public const int ConstInt = 24;
        public static readonly int StaticReadonlyInt = 5;
        public const string WithQuoteMiddle = @""before""""after"";
        public const string WithQuoteStart = @""""""after"";
        public int MultipleOne, MultipleTwo;
        public readonly int ReadonlyInt = 3;
		public DateTime UninitializedDate;
		public int? UnitializedNullableInt;
		public TimeSpan UninitializedTimeSpan;
		public static DateTime StaticUninitializedDate;
		public static int? StaticUnitializedNullableInt;
		public static TimeSpan StaticUninitializedTimeSpan;

        static Box()
        {
            Console.WriteLine(""cctor"");
        }

        public Box()
        {
            Console.WriteLine(""ctor"");
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Box
{
    var StaticField:System.Text.StringBuilder = new System.Text.StringBuilder();
    val ConstInt:Int = 24;
    var StaticReadonlyInt:Int = 5;
    val WithQuoteMiddle:String = ""before\""after"";
    val WithQuoteStart:String = ""\""after"";
	var StaticUninitializedDate:System.DateTime = new System.DateTime();
	var StaticUnitializedNullableInt:java.lang.Integer = null;
	var StaticUninitializedTimeSpan:System.TimeSpan = new System.TimeSpan();


    def cctor()
    {
        System.Console.WriteLine(""cctor"");
    }

}
class Box
{
    private var _width:Float = 0;
    def Width:Float =
    {
        return _width;
    }
    def Width_=(value:Float) =
    {
        _width = value;
    }
	def SetOnly:Float =
	{
		throw new Exception(""No setter defined"");
	}
    def SetOnly_=(value:Float) =
    {
        System.Console.WriteLine(value);
    }
    def GetOnly:Int =
    {
        return 4;
    }

    var IsRectangular:Boolean = true;
    var Characters:Array[Char] = Array[Char]('a', 'b');

    var MultipleOne:Int = 0;
    var MultipleTwo:Int = 0;
    var ReadonlyInt:Int = 3;
	var UninitializedDate:System.DateTime = new System.DateTime();
	var UnitializedNullableInt:java.lang.Integer = null;
	var UninitializedTimeSpan:System.TimeSpan = new System.TimeSpan();

	{
		System.Console.WriteLine(""ctor"");
	}
}
");
        }

        [TestMethod]
        public void ForStatementWithNoCondition()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            for(;;)
            {
                Console.WriteLine(""Hello, World!"");
            }
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        { //for
            while (true)
            {
                System.Console.WriteLine(""Hello, World!"");
            }
        } //end for
    }
}");
        }

        [TestMethod]
        public void AutomaticProperties()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    class Box
    {
        public float Width
        {
            get;
            set;
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class Box
{
    var Width:Float = 0;
}");
        }

        [TestMethod]
        public void DictionaryAndHashSet()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Linq;
using System.Collections.Generic;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(4, 3);
            Console.WriteLine(dict[4]);
            Console.WriteLine(dict.ContainsKey(8));
            dict.Remove(4);
            foreach(int key in dict.Keys)
                Console.WriteLine(key);
            foreach(int val in dict.Values)
                Console.WriteLine(val);
			foreach(var kv in dict)
				Console.WriteLine(kv.Key + "" "" + kv.Value);
			var dict2 = dict.ToDictionary(o => o.Key, o => o.Value);
			var vals = dict.Values;
            
            HashSet<int> hash = new HashSet<int>();
            hash.Add(999);
            Console.WriteLine(hash.Contains(999));
            hash.Remove(999);
            Console.WriteLine(hash.Contains(999));
            foreach(int hashItem in hash)
                Console.WriteLine(hashItem);
			var z = hash.Select(o => 3).ToArray();
			var g = hash.GroupBy(o => o).Select(o => o.Count()).Min();
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var dict:System.Collections.Generic.Dictionary[Int, Int] = new System.Collections.Generic.Dictionary[Int, Int]();
        dict.Add(4, 3);
        System.Console.WriteLine(dict(4));
        System.Console.WriteLine(dict.ContainsKey(8));
        dict.Remove(4);
        for (key <- dict.Keys)
        {
            System.Console.WriteLine(key);
        }
        for (csval <- dict.Values)
        {
            System.Console.WriteLine(csval);
        }
        
		for (kv <- dict)
		{
			System.Console.WriteLine(kv.Key + "" "" + kv.Value);
		}
		var dict2:System.Collections.Generic.Dictionary[Int, Int] = System.Linq.Enumerable.ToDictionary(dict, (o:System.Collections.Generic.KeyValuePair[Int, Int]) => { o.Key; }:Int, (o:System.Collections.Generic.KeyValuePair[Int, Int]) => { o.Value; }:Int);
		var vals:System.Collections.Generic.Dictionary_ValueCollection[Int] = dict.Values;
        
        var hash:System.Collections.Generic.HashSet[Int] = new System.Collections.Generic.HashSet[Int]();
        hash.Add(999);
        System.Console.WriteLine(hash.Contains(999));
        hash.Remove(999);
        System.Console.WriteLine(hash.Contains(999));
        for (hashItem <- hash)
        {
            System.Console.WriteLine(hashItem);
        }
		var z:Array[Int] = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(hash, (o:Int) => { 3; }:Int));
		var g:Int = System.Linq.Enumerable.Min(System.Linq.Enumerable.Select(System.Linq.Enumerable.GroupBy(hash, (o:Int) => { o; }:Int), (o:System.Linq.IGrouping[Int, Int]) => { System.Linq.Enumerable.Count(o); }:Int));

    }
}");
        }


        [TestMethod]
        public void Lambda()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Collections.Generic;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            Func<int, int> f1 = x => x + 5;
            Console.WriteLine(f1(3));
            Func<int, int> f2 = x => { return x + 6; };
            Console.WriteLine(f2(3));

            List<Action> actions = new List<Action>();
			actions.Add(() => { });
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
		var f1:(Int) => Int = (x:Int) => 
		{ 
		x + 5; 
		}:Int;
		System.Console.WriteLine(f1(3));
		var f2:(Int) => Int = (x:Int) => 
		{ 
		x + 6; 
		}:Int;
		System.Console.WriteLine(f2(3));
		var actions:ArrayBuffer[() => Unit] = new ArrayBuffer[() => Unit]();
		actions.append(() =>
		{
		});
    }
}");
        }

        [TestMethod]
        public void LambdaNoReturn()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            int i = 3;
            Action a = () => i = 4;
            Func<int> b = () => i;
            Foo(() => i = 6);
        }
        public static void Foo(Action a)
        {
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
	def SomeFunction()
    {
        var i:Int = 3;
        var a:() => Unit = () =>
        { 
            i = 4;
        };
        var b:() => Int = () =>
        { 
            i;
        }:Int;
        Blargh.Utilities.Foo(() =>
        {
            i = 6;
        });
    }
    def Foo(a:() => Unit)
    {
    }  
}");
        }

        [TestMethod]
        public void LambdaWithBranchingReturnAndReturnValue()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            Func<bool, bool, int> z = (a, b) => 
			{
				Console.WriteLine(1);
				if (a && b)
					return 1;
				Console.WriteLine(2);
				if (a)
				{
					Console.WriteLine(3);
					return 2;
				}
				Console.WriteLine(4);
				return 3;
			};
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
	def SomeFunction()
    {
        var z:(Boolean, Boolean) => Int = (a:Boolean, b:Boolean) => 
		{
			val __lambdabreak = new Breaks;
			var __lambdareturn:Int = 0;
			__lambdabreak.breakable
			{
				System.Console.WriteLine(1);
				if (a && b)
				{
					__lambdareturn = 1;
					__lambdabreak.break();
				}
				System.Console.WriteLine(2);
				if (a)
				{
					System.Console.WriteLine(3);
					__lambdareturn = 2;
					__lambdabreak.break();
				}
				System.Console.WriteLine(4);
				__lambdareturn = 3;
			}
			__lambdareturn;
		}:Int;
    }
}");
        }


        [TestMethod]
        public void LambdaWithBranchingReturnNoReturnValue()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            Action<bool, bool> z = (a, b) => 
			{
				Console.WriteLine(1);
				if (a && b)
					return;
				Console.WriteLine(2);
				if (a)
				{
					Console.WriteLine(3);
					return;
				}
				Console.WriteLine(4);
			};
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
	def SomeFunction()
    {
        var z:(Boolean, Boolean) => Unit = (a:Boolean, b:Boolean) => 
		{
			val __lambdabreak = new Breaks;
			__lambdabreak.breakable
			{
				System.Console.WriteLine(1);
				if (a && b)
				{
					__lambdabreak.break();
				}
				System.Console.WriteLine(2);
				if (a)
				{
					System.Console.WriteLine(3);
					__lambdabreak.break();
				}
				System.Console.WriteLine(4);
			}
		};
    }
}");
        }

        [TestMethod]
        public void Indexing()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Collections.Generic;

namespace Blargh
{
    public class Foo
    {
        public Foo()
        {
            var dict = new Dictionary<int, int>();
			dict[3] = 4;
			var i = dict[3];
			var array = new int[3];
			array[0] = 1;
			var str = ""hello"";
			var c = str[2];
			var list = new List<int>();
			i = list[0];
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class Foo
{
	{
		var dict:System.Collections.Generic.Dictionary[Int, Int] = new System.Collections.Generic.Dictionary[Int, Int]();
		dict(3) = 4;
		var i:Int = dict(3);
		var array:Array[Int] = new Array[Int](3);
		array(0) = 1;
		var str:String = ""hello"";
		var c:Char = str(2);
		var list:ArrayBuffer[Int] = new ArrayBuffer[Int]();
		i = list(0);
	}
    
}");
        }

        [TestMethod]
        public void NamedParameters()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Text;

namespace Blargh
{

    public class Foo
    {
		public void Bar(int a, int b, int c, int d = 3)
		{
		}

        public Foo()
		{
			Bar(1,2,3,4);
			Bar(1,2,3);
			Bar(a: 1, b: 2, c: 3, d: 4);
			Bar(a: 1, b: 2, c: 3);
			Bar(a: 1, c: 3, b: 2);
			Bar(1, c: 3, b: 2);
			Bar(1, 2, c: 3, d: 4);
		}
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class Foo
{
	def Bar(a:Int, b:Int, c:Int, d:Int = 3)
	{
	}

	{
		Bar(1, 2, 3, 4);
		Bar(1, 2, 3);
		Bar(a = 1, b = 2, c = 3, d = 4);
		Bar(a = 1, b = 2, c = 3);
		Bar(a = 1, c = 3, b = 2);
		Bar(1, c = 3, b = 2);
		Bar(1, 2, c = 3, d = 4);
	}
}");
        }

        [TestMethod]
        public void NestedClasses()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Text;

namespace Blargh
{

    public class Outer
    {
		public class Inner
		{
			public int InnerField;
			public Inner()
			{
				InnerField = 9;
			}
		}

		public Outer()
		{
			var i = new Inner();
			i.InnerField = 4;
		}
    }
}", new[] { @"
package Blargh;
" + WriteImports.StandardImports + @"

class Outer
{
	{
		var i:Blargh.Outer_Inner = new Blargh.Outer_Inner();
		i.InnerField = 4;
	}
}",
  @"
package Blargh;
" + WriteImports.StandardImports + @"

class Outer_Inner
{
	var InnerField:Int = 0;
    
	{
		InnerField = 9;
	}
}"
  
  });
        }


        [TestMethod]
        public void AnonymousTypes()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Text;

namespace Blargh
{

    public class Foo
    {
        public Foo()
		{
			var i = new { Field1 = 3, Field2 = new StringBuilder() };
			Console.WriteLine(i.Field1);
		}
    }
}", new[] { @"
package Blargh;
" + WriteImports.StandardImports + @"

class Foo
{
	{
		var i:Anon_Field1_Int__Field2_System_Text_StringBuilder = new Anon_Field1_Int__Field2_System_Text_StringBuilder(3, new System.Text.StringBuilder());
		System.Console.WriteLine(i.Field1);
	}
}",
 @"
package anonymoustypes;
" + WriteImports.StandardImports + @"

class Anon_Field1_Int__Field2_System_Text_StringBuilder(_Field1:Int, _Field2:System.Text.StringBuilder)
{
	var Field1:Int = _Field1;
	var Field2:System.Text.StringBuilder = _Field2;
}"

  
  });
        }



        [TestMethod]
        public void PreprocessorDirectives()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public class SomeClass
    {
#if CSSCALA
		Some raw text here;
#endif

        public SomeClass()
        {
#if CSSCALA
			Console.WriteLine(""CsScala1"");
#else
			Console.WriteLine(""not1"");
#endif
#if CSSCALA //comment
			Console.WriteLine(""CsScala2"");
#else

			Console.WriteLine(""not2"");
#if nope
			Console.WriteLine(""not3"");
#endif

#endif
			Console.WriteLine(""outside"");

#if CSSCALA
			Console.WriteLine(""CsScala3"");
#endif
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class SomeClass
{
	Some raw text here;

	{
		Console.WriteLine(""CsScala1"");
		Console.WriteLine(""CsScala2"");
		System.Console.WriteLine(""outside"");
		Console.WriteLine(""CsScala3"");
	}
}");
        }


        [TestMethod]
        public void OfType()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System.Text;
using System.Linq;

namespace Blargh
{
    public class SomeClass
    {
        public SomeClass()
        {
            var a = new[] { 1,2,3 };
			var b = a.OfType<StringBuilder>().ToList();
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class SomeClass
{
	{
		var a:Array[Int] = Array(1, 2, 3);
		var b:ArrayBuffer[System.Text.StringBuilder] = System.Linq.Enumerable.ToList(System.Linq.Enumerable.OfType[Int, System.Text.StringBuilder](a));
	}
}");
        }

        [TestMethod]
        public void GlobalKeyword()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public class SomeClass
    {
        public SomeClass()
        {
            global::Blargh.SomeClass c = null;
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class SomeClass
{
    {
        var c:Blargh.SomeClass = null;
    }
}");
        }


        [TestMethod]
        public void DefaultParameter()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public class SomeClass
    {
        public void Foo(int i1, int i2 = 4, string s1 = ""hi"")
        {
        }

        public SomeClass(int i3 = 9)
        {
            Foo(4);
            Foo(5, 6);
            Foo(6, 7, ""eight"");
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class SomeClass(i3:Int = 9)
{
    def Foo(i1:Int, i2:Int = 4, s1:String = ""hi"")
    {
    }

    {
        Foo(4);
        Foo(5, 6);
        Foo(6, 7, ""eight"");
    }
}");
        }

        [TestMethod]
        public void Linq()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Linq;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            int[] e = new int[] { 0, 1, 2, 3 };
            Console.WriteLine(e.First());
            Console.WriteLine(e.First(o => o == 1));
            Console.WriteLine(e.ElementAt(2));
            Console.WriteLine(e.Last());
            Console.WriteLine(e.Select(o => o).Count());
            Console.WriteLine(e.Where(o => o > 0).Count() + 2);
            Console.WriteLine(e.Count(o => true) + 2);

            var dict = e.ToDictionary(o => o, o => 555);
            e.OfType<int>();
			e.OrderBy(o => 4);
			e.OrderBy(o => ""z"");
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
             
		var e:Array[Int] = Array[Int](0, 1, 2, 3);
		System.Console.WriteLine(System.Linq.Enumerable.First(e));
		
		System.Console.WriteLine(System.Linq.Enumerable.First(e, (o:Int) =>
		{
		    o == 1;
		}:Boolean));
		System.Console.WriteLine(System.Linq.Enumerable.ElementAt(e, 2));
		System.Console.WriteLine(System.Linq.Enumerable.Last(e));
		System.Console.WriteLine(System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(e, (o:Int) => { o; }:Int)));
		System.Console.WriteLine(System.Linq.Enumerable.Count(System.Linq.Enumerable.Where(e, (o:Int) => 
		{
		    o > 0;
		}:Boolean)) + 2);
		System.Console.WriteLine(System.Linq.Enumerable.Count(e, (o:Int) =>
		{
		    true;
		}:Boolean) + 2);
		var dict:System.Collections.Generic.Dictionary[Int, Int] = System.Linq.Enumerable.ToDictionary(e, (o:Int) =>
		{
		    o;
		}:Int, (o:Int) =>
		{
		    555;
		}:Int);
		System.Linq.Enumerable.OfType[Int, Int](e);
		System.Linq.Enumerable.OrderBy_Int(e, (o:Int) => { 4; }:Int);
		System.Linq.Enumerable.OrderBy_String(e, (o:Int) => { ""z""; }:String);
	}
}");

        }

        [TestMethod]
        public void GenericClass()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Collections.Generic;

namespace Blargh
{
	public class KeyValueList<K, V> : IEquatable<K>
	{
		private List<KeyValuePair<K, V>> _list = new List<KeyValuePair<K, V>>();

		public void Add(K key, V value)
		{
			this._list.Add(new KeyValuePair<K, V>(key, value));
		}

		public void Insert(int index, K key, V value)
		{
			_list.Insert(index, new KeyValuePair<K, V>(key, value));
		}

		public void Clear()
		{
			_list.Clear();
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public bool Equals(K other)
		{
			throw new NotImplementedException();
		}
	}
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class KeyValueList[K, V] extends System.IEquatable[K]
{
  
    private var _list:ArrayBuffer[System.Collections.Generic.KeyValuePair[K, V]] = new ArrayBuffer[System.Collections.Generic.KeyValuePair[K, V]]();

    def Add(key:K, value:V)
    {
        this._list.append(new System.Collections.Generic.KeyValuePair[K, V](key, value));
    }
    def Insert(index:Int, key:K, value:V)
    {
        _list.insert(index, new System.Collections.Generic.KeyValuePair[K, V](key, value));
    }
    def Clear()
    {
    	_list.clear();
    }
    def RemoveAt(index:Int)
    {
        _list.remove(index);
    }
	def Equals(other:K):Boolean =
	{
		throw new System.NotImplementedException();
	}
}
");
        }

        [TestMethod]
        public void ConstructorCallsBaseConstructor()
        {
            var cs = @"
using System;

namespace Blargh
{
    public class Top
    {
        public Top(int i) { }
    }

    public class Derived : Top
    {
        public Derived() : base(4) { }
    }
}";

            var scala1 = @" 
package Blargh;
" + WriteImports.StandardImports + @"

class Top(i:Int)
{
}";

            var scala2 = @"
package Blargh;
" + WriteImports.StandardImports + @"

class Derived extends Blargh.Top(4)
{
}";

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, cs, new string[] { scala1, scala2 });
        }

        [TestMethod]
        public void ObjectInitilization()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System.Text;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            var sb = new StringBuilder()
            {
                Capacity = 9
            };
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
	def SomeFunction()
	{
		var sb:System.Text.StringBuilder = new System.Text.StringBuilder()
		{
			Capacity = 9;
		};
	}
}
			
			");
        }

        [TestMethod]
        public void UsingStatement()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.IO;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            var usingMe = new MemoryStream();
            using (usingMe)
            {
                Console.WriteLine(""In using"");
				return;
            }
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var usingMe:System.IO.MemoryStream = new System.IO.MemoryStream();
        try
        {
            System.Console.WriteLine(""In using"");
            return;
        }
        finally
        {
            usingMe.Dispose();
        }
    }
}
");
        }

        [TestMethod]
        public void Math()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            int i = 3;
            i += 4;
            i -= 3;
            i *= 4;
            i %= 3;
            i = i + 1;
            i = i % 3;
            i = i - 4;
            i = i * 100;
            double f = i / 3f;
            int hex = 0x00ff;
            i = (int)f;
			var z = (i & hex) == 5;
			var x = (int)(i / 3);
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var i:Int = 3;
        i += 4;
        i -= 3;
        i *= 4;
        i %= 3;
        i = i + 1;
        i = i % 3;
        i = i - 4;
        i = i * 100;
        var f:Double = i / 3f;
        var hex:Int = 0x00ff;
        i = f.toInt;
		var z:Boolean = (i & hex) == 5;
		var x:Int = (i / 3);
    }
}");
        }

        [TestMethod]
        public void Delegates()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public delegate int NamespaceDlg();
    public delegate T TemplatedDelegate<T>(T arg, int arg2);

    public static class Utilities
    {
		public static Action StaticAction;
        public delegate int GetMahNumber(int arg);

        public static void SomeFunction(GetMahNumber getit, NamespaceDlg getitnow, TemplatedDelegate<float> unused)
        {
            Console.WriteLine(getit(getitnow()));
            var a = new[] { getitnow };
			a[0]();
			StaticAction();
			Utilities.StaticAction();
			Blargh.Utilities.StaticAction();
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
	var StaticAction:() => Unit = null;

    def SomeFunction(getit:(Int) => Int, getitnow:() => Int, unused:(Float, Int) => Float)
    {
        System.Console.WriteLine(getit(getitnow()));
		var a:Array[() => Int] = Array(getitnow);
		a(0)();
		Blargh.Utilities.StaticAction();
		Blargh.Utilities.StaticAction();
		Blargh.Utilities.StaticAction();
    }
}");
        }

        [TestMethod]
        public void TypeStatics()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
		static int Foo;
        public static void SomeFunction()
        {
            Blargh.Utilities.Foo = 4;
            Console.WriteLine(int.MaxValue);
            Console.WriteLine(int.MinValue);
            Console.WriteLine(short.MaxValue);
            Console.WriteLine(short.MinValue);
            Console.WriteLine(ushort.MaxValue);
            Console.WriteLine(ushort.MinValue);
            Console.WriteLine(uint.MaxValue);
            Console.WriteLine(uint.MinValue);
            string s = ""123"";
            Console.WriteLine(int.Parse(s) + 1);
            float.Parse(s);
            double.Parse(s);
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
	var Foo:Int = 0;
    def SomeFunction()
    {
        Blargh.Utilities.Foo = 4;
        System.Console.WriteLine(Int.MaxValue);
        System.Console.WriteLine(Int.MinValue);
        System.Console.WriteLine(Short.MaxValue);
        System.Console.WriteLine(Short.MinValue);
        System.Console.WriteLine(System.CsScala.ushortMaxValue);
        System.Console.WriteLine(System.CsScala.ushortMinValue);
        System.Console.WriteLine(System.CsScala.uintMaxValue);
        System.Console.WriteLine(System.CsScala.uintMinValue);
        var s:String = ""123"";
        System.Console.WriteLine(s.toInt + 1);
        s.toFloat;
        s.toDouble;
    }
}");
        }


        [TestMethod]
        public void NullableTypes()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            int? nullableInt = new Nullable<int>();
			float d = 3;
			var cond = nullableInt.HasValue ? (float?)null : ((float)d);
            Console.WriteLine(nullableInt.HasValue);
            int? withValue = new Nullable<int>(8);
            Console.WriteLine(withValue.Value);
			int? implicitNull = null;
			implicitNull = null;
			int? implicitValue = 5;
			implicitValue = 8;
			Foo(3);
			int? n = (int?)null;
        }

		public static int? Foo(int? i)
		{
			return 4;
		}
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var nullableInt:java.lang.Integer = null;
		var d:Float = 3;
		var cond:java.lang.Float = if ((nullableInt != null)) null else (d);
        System.Console.WriteLine((nullableInt != null));
        var withValue:java.lang.Integer = 8;
        System.Console.WriteLine(withValue.intValue());
		var implicitNull:java.lang.Integer = null;
		implicitNull = null;
		var implicitValue:java.lang.Integer = 5;
		implicitValue = 8;
		Blargh.Utilities.Foo(3);
		var n:java.lang.Integer = null;
    }
	def Foo(i:java.lang.Integer):java.lang.Integer =
	{
		return 4;
	}
 
}");
        }


        [TestMethod]
        public void Enums()
        {


            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, new string[] { @"
using System;

namespace Blargh
{
    public enum MostlyNumbered
    {
        One = 1,
        Two = 2,
        Three = 3,
        Unnumbered,
        SomethingElse = 50
    }
    public enum UnNumbered
    {
        One, Two, Three
    }
    static class Clazz
    {
        public static void Methodz()
        {
            var f = MostlyNumbered.One;
            var arr = new UnNumbered[] { UnNumbered.One, UnNumbered.Two, UnNumbered.Three };
            var i = (int)f;
			var e = (MostlyNumbered)Enum.Parse(typeof(MostlyNumbered), ""One"");
			var s = e.ToString();
			s = e + ""asdf"";
			s = ""asdf"" + e;
			var vals = Enum.GetValues(typeof(MostlyNumbered));
        }
    }
}" }, new string[] { @"
package Blargh;
" + WriteImports.StandardImports + @"
object MostlyNumbered
{
    val One:Int = 1;
    val Two:Int = 2;
    val Three:Int = 3;
    val Unnumbered:Int = 4;
    val SomethingElse:Int = 50;

	def ToString(e:Int):String =
	{ 
		return e match
		{ 
			case 1 => ""One""; 
			case 2 => ""Two""; 
			case 3 => ""Three""; 
			case 4 => ""Unnumbered""; 
			case 50 => ""SomethingElse""; 
		}
	} 
	
	def Parse(s:String):Int =
	{ 
		return s match
		{ 
			case ""One"" => 1; 
			case ""Two"" => 2;		
			case ""Three"" => 3; 
			case ""Unnumbered"" => 4; 
			case ""SomethingElse"" => 50; 
		} 
	}

	val Values:Array[Int] = Array(1, 2, 3, 4, 50);
}", @"
package Blargh;
" + WriteImports.StandardImports + @"
object UnNumbered
{
	val One:Int = 1; 
    val Two:Int = 2;
    val Three:Int = 3;
	def ToString(e:Int):String =
	{ 
		return e match
		{ 
			case 1 => ""One""; 
			case 2 => ""Two""; 
			case 3 => ""Three""; 
		}
	} 
	
	def Parse(s:String):Int =
	{ 
		return s match
		{ 
			case ""One"" => 1; 
			case ""Two"" => 2;		
			case ""Three"" => 3; 
		} 
	}
	val Values:Array[Int] = Array(1, 2, 3);
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Clazz
{
    def Methodz()
    {
        var f:Int = Blargh.MostlyNumbered.One;
        var arr:Array[Int] = Array[Int](Blargh.UnNumbered.One, Blargh.UnNumbered.Two, Blargh.UnNumbered.Three);
        var i:Int = f;
		var e:Int = Blargh.MostlyNumbered.Parse(""One"").asInstanceOf[Int];
		var s:String = Blargh.MostlyNumbered.ToString(e);
		s = Blargh.MostlyNumbered.ToString(e) + ""asdf"";
		s = ""asdf"" + Blargh.MostlyNumbered.ToString(e);
		var vals = Blargh.MostlyNumbered.Values();
	}
}"});
        }

        [TestMethod]
        public void NestedEnum()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, new string[] { @"
namespace Blargh
{
    class Foo
    {
		public enum TestEnum
		{
			One, Two, Three
		}

		public Foo()
		{
			var i = TestEnum.One;
			i.ToString();
		}
    }
}" }, new string[] { @"
package Blargh;
" + WriteImports.StandardImports + @"
class Foo
{
	{
		var i:Int = Blargh.Foo_TestEnum.One;
		Blargh.Foo_TestEnum.ToString(i);
	}
}", @"
package Blargh;
" + WriteImports.StandardImports + @"
object Foo_TestEnum
{
	val One:Int = 1; 
    val Two:Int = 2;
    val Three:Int = 3;

	def ToString(e:Int):String =
	{ 
		return e match
		{ 
			case 1 => ""One""; 
			case 2 => ""Two""; 
			case 3 => ""Three""; 
		}
	} 
	
	def Parse(s:String):Int =
	{ 
		return s match
		{ 
			case ""One"" => 1; 
			case ""Two"" => 2;		
			case ""Three"" => 3; 
		} 
	}
	val Values:Array[Int] = Array(1, 2, 3);
}" });
        }

        [TestMethod]
        public void SwitchStatement()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            string s = ""Blah"";
            switch (s)
            {
                case ""NotMe"": Console.WriteLine(5); break;
                case ""Box"": Console.WriteLine(4); break;
                case ""Blah"": 
				case ""Blah2"": Console.WriteLine(3); break;
                default: throw new InvalidOperationException();
            }
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var s:String = ""Blah"";
        s match
        {
            case ""NotMe"" =>
                System.Console.WriteLine(5);
            case ""Box"" =>
                System.Console.WriteLine(4); 
            case ""Blah"" | ""Blah2"" =>
                System.Console.WriteLine(3); 
            case _ => 
                throw new System.InvalidOperationException();
        }
    }
}");
        }

        [TestMethod]
        public void IsAndAs()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Collections.Generic;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            string s = ""Blah"";
            var list = new List<int>();
            if (s is string)
                Console.WriteLine(""Yes"");
            if (list is List<int>)
                Console.WriteLine(""Yes"");

            object o = s;
            string sss = o as string;
            Console.WriteLine(sss);
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var s:String = ""Blah"";
        var list:ArrayBuffer[Int] = new ArrayBuffer[Int]();
        if (s.isInstanceOf[String])
        {
            System.Console.WriteLine(""Yes"");
        }
        if (list.isInstanceOf[ArrayBuffer[Int]])
        {
            System.Console.WriteLine(""Yes"");
        }
		var o:Any = s;
		var sss:String = CsScala.As[String](o);
		System.Console.WriteLine(sss);
    }
}");
        }


        [TestMethod]
        public void AbstractAndOverrides()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    abstract class TopLevel
    {
        public abstract void AbstractMethod();
        public abstract string AbstractProperty { get; }

        public virtual void VirtualMethod()
        {
            Console.WriteLine(""TopLevel::VirtualMethod"");
        }
        public virtual string VirtualProperty
        {
            get
            {
                return ""TopLevel::VirtualProperty"";
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    class Derived : TopLevel
    {
        public override void AbstractMethod()
        {
            Console.WriteLine(""Derived::AbstractMethod"");
        }

        public override string AbstractProperty
        {
            get { return ""Derived::AbstractProperty""; }
        }

        public override void VirtualMethod()
        {
            base.VirtualMethod();
            Console.WriteLine(""Derived::VirtualMethod"");
        }

        public override string VirtualProperty
        {
            get
            {
                return base.VirtualProperty + ""Derived:VirtualProperty"";
            }
        }
        public override string ToString()
        {
            return ""DerivedToString"";
        }
    }
}", new string[] { @"
package Blargh;
" + WriteImports.StandardImports + @"

abstract class TopLevel
{
    def AbstractMethod();

    def AbstractProperty:String;
	
    def VirtualMethod()
    {
        System.Console.WriteLine(""TopLevel::VirtualMethod"");
    }

    def VirtualProperty:String = 
    {
        return ""TopLevel::VirtualProperty"";
    }

    override def toString():String =
    {
        return """";
    }	
}",
    @"
package Blargh;
" + WriteImports.StandardImports + @"

  
        
class Derived extends Blargh.TopLevel
{
    override def AbstractMethod()
    {
        System.Console.WriteLine(""Derived::AbstractMethod"");
    }
    override def AbstractProperty:String =
    {
        return ""Derived::AbstractProperty"";
    }
    override def VirtualMethod()
    {
        super.VirtualMethod();
        System.Console.WriteLine(""Derived::VirtualMethod"");
    }

    override def VirtualProperty:String =
    {
        return super.VirtualProperty + ""Derived:VirtualProperty"";
    }

    override def toString():String =
    {
        return ""DerivedToString"";
    }
}"  });
        }



        [TestMethod]
        public void Interfaces()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public interface ITesting
    {
        void Poke();
    }

    class Pokable : ITesting
    {
        public void Poke()
        {
            Console.WriteLine(""Implementation"");
        }
    }
}",
  new string[] { @"
package Blargh;
" + WriteImports.StandardImports + @"

trait ITesting
{
    def Poke();
}",
  @"
package Blargh;
" + WriteImports.StandardImports + @"

class Pokable extends Blargh.ITesting
{
    def Poke()
    {
        System.Console.WriteLine(""Implementation"");
    }
}"});
        }

        [TestMethod]
        public void TryCatchThrow()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.IO;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            Console.WriteLine(""Before try"");
            try
            {
                Console.WriteLine(""In try"");
            }
            catch (IOException ex)
            {
                Console.WriteLine(""In catch 1"");
            }
            catch (Exception ex)
            {
                Console.WriteLine(""In catch 2"");
            }
			finally
			{
                Console.WriteLine(""In finally"");
			}

            try
            {
                Console.WriteLine(""Try in parameterless catch"");
            }
            catch
            {
                Console.WriteLine(""In parameterless catch"");
            }

            throw new InvalidOperationException(""err"");
        }

    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        System.Console.WriteLine(""Before try"");
        try
        {
            System.Console.WriteLine(""In try"");
        }
        catch
        {
          case ex: System.IO.IOException => 
            System.Console.WriteLine(""In catch 1"");
          case ex: java.lang.Exception =>
            System.Console.WriteLine(""In catch 2"");
        }
        finally
        {
            System.Console.WriteLine(""In finally"");
        }
        try
        {
            System.Console.WriteLine(""Try in parameterless catch"");
        }
        catch
        {
          case __ex: java.lang.Exception =>
            System.Console.WriteLine(""In parameterless catch"");
        }

        throw new System.InvalidOperationException(""err"");    		
    }
}");
        }



        [TestMethod]
        public void Generics()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Collections.Generic;

namespace Blargh
{
    public static class Utilities
    {
        public static Queue<T> ToQueue<T>(this IEnumerable<T> array)
        {
            var queue = new Queue<T>();
            foreach (T a in array)
                queue.Enqueue(a);

            queue.Dequeue();
            return queue;
        }

        public static IEnumerable<T> SideEffect<T>(this IEnumerable<T> array, Action<T> effect)
        {
            foreach(var i in array)
                effect(i);
            return array;
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def ToQueue[T](array:Traversable[T]):Queue[T] =
    {
        var queue:Queue[T] = new Queue[T]();
        for (a <- array)
        {
            queue.enqueue(a);
        }
        queue.dequeue();
        return queue;
    }
    def SideEffect[T](array:Traversable[T], effect:(T) => Unit):Traversable[T] =
    {
        for (i <- array)
        {
            effect(i);
        }
        return array;
    }
}");
        }

        [TestMethod]
        public void Objects()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Collections.Generic;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            var queue = new Queue<int>(10);
            queue.Enqueue(4);
            queue.Enqueue(2);
            Console.WriteLine(queue.Dequeue());
            queue.Clear();
    
            var list = new List<string>(3);
            list.Add(""Three"");
            list.RemoveAt(0);
            list.Insert(4, ""Seven"");

            var stack = new Stack<int>();
            stack.Push(9);
            stack.Push(3);
            Math.Max(stack.Pop(), stack.Pop());
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var queue:Queue[Int] = new Queue[Int]();
        queue.enqueue(4);
        queue.enqueue(2);
        System.Console.WriteLine(queue.dequeue());
        queue.clear();

        var list:ArrayBuffer[String] = new ArrayBuffer[String](3);
        list.append(""Three"");
        list.remove(0);
        list.insert(4, ""Seven"");

		
        var stack:Stack[Int] = new Stack[Int]();
        stack.push(9);
        stack.push(3);
        Math.max(stack.pop(), stack.pop());
    }
}");
        }

        [TestMethod]
        public void ReplaceTypeWithAttribute()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using Shared;

#if !CSSCALA
namespace Shared
{
	public class CsScalaAttribute : Attribute
    {
        public string ReplaceWithType { get; set; }
    }
}
#endif

namespace Blargh
{

    public class Foo
    {
        [CsScala(ReplaceWithType = ""bar.Baz"")]
        public object Obj;
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class Foo
{
    var Obj:bar.Baz = null;
}");
        }

        [TestMethod]
        public void Casts()
        {
            var cs = @"
using System;

namespace Blargh
{
#if !CSSCALA
	public static class Utilities
	{
		public static T As<T>(this object o)
		{
			return (T)o;
		}
	}
#endif

    public static class Test
    {
        public static void SomeFunction()
        {
			var a = DateTime.Now.As<String>();
			object o = 4;
			var b = (byte)(short)o;
        }
    }
}";

            var scala = @"
package Blargh;
" + WriteImports.StandardImports + @"

object Test
{
    def SomeFunction()
    {
        var a:String = System.DateTime.Now.asInstanceOf[String];
		var o:Any = 4;
		var b:Byte = o.asInstanceOf[Short].toByte;
    }
}";

            var transform = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Translations>
  <Method SourceObject=""*"" Match=""As"">
	<ReplaceWith>
	  <Expression />
	  <String>.asInstanceOf[</String>
	  <TypeParameter Index=""0"" />
	  <String>]</String>
	</ReplaceWith>
  </Method>
</Translations>";

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, new[] { cs }, new[] { scala }, transform);
        }


        [TestMethod]
        public void ArrayAndForEach()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Collections.Generic;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            var ar = new int[] { 1, 2, 3 };

            foreach(var i in ar)
                Console.WriteLine(i);

            Console.WriteLine(ar[1]);
            Console.WriteLine(ar.Length);
			Console.WriteLine(new List<string>().Count);
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var ar:Array[Int] = Array[Int](1, 2, 3);
        for (i <- ar)
        {
        	System.Console.WriteLine(i);
        }
        System.Console.WriteLine(ar(1));
        System.Console.WriteLine(ar.length);
		System.Console.WriteLine(new ArrayBuffer[String]().length);
    }
}");
        }

        [TestMethod]
        public void PartialClasses()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name,

new string[] { @"
using System;

namespace Blargh
{
    public partial class Utilities
    {
        public void FunFromOne()
        {
            Console.WriteLine(""I'm in one!"");
        }
    }
}",
            
  @"
using System;

namespace Blargh
{
    public partial class Utilities
    {
        public void FunFromTwo()
        {
            Console.WriteLine(""I'm in Two!"");
        }
    }
}"
}, @"
package Blargh;
" + WriteImports.StandardImports + @"

class Utilities
{
    def FunFromOne()
    {
        System.Console.WriteLine(""I'm in one!"");
    }
    def FunFromTwo()
    {
        System.Console.WriteLine(""I'm in Two!"");
    }
}");
        }

        [TestMethod]
        public void StringMethods()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction(string s2)
        {
            string s = @""50\0"";
            Console.WriteLine(s.IndexOf(""0""));
            Console.WriteLine(s2.IndexOf(""0""));

            foreach(string s3 in new string[] { ""Hello"" })
                s3.Substring(4, 5);

            int i = 4;
            string si = i.ToString();
			if (si.StartsWith(""asdf""))
				Console.WriteLine(4);
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"
object Utilities
{
    def SomeFunction(s2:String)
    {
        var s:String = ""50\\0"";
        System.Console.WriteLine(s.indexOf(""0""));
        System.Console.WriteLine(s2.indexOf(""0""));

        for (s3 <- Array[String](""Hello""))
        {
            s3.substring(4, 5);
        }
        var i:Int = 4;
        var si:String = i.toString();
        if (si.startsWith(""asdf""))
		{
			System.Console.WriteLine(4);
		}
    }
}");
        }

        [TestMethod]
        public void ExtensionMethod()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction()
        {
            int i = -3;
            Console.WriteLine(""false "" + i.IsFour());
            i += 6;
            var b = i.IsFour();
            Console.WriteLine(""true "" + b);
            Utilities.IsFour(5);
        }

        public static bool IsFour(this int i)
        {
            return i == 4;
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction()
    {
        var i:Int = -3;
        System.Console.WriteLine(""false "" + Blargh.Utilities.IsFour(i));
        i += 6;
        var b:Boolean = Blargh.Utilities.IsFour(i);
        System.Console.WriteLine(""true "" + b);
        Blargh.Utilities.IsFour(5);
    }

    def IsFour(i:Int):Boolean =
    {
        return i == 4;
    }
}");
        }

        [TestMethod]
        public void StringJoin()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public class Foo
    {
        public Foo()
        {
            var s = string.Join("";"", new[] { ""one"", ""two"" });
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class Foo
{
    {
        var s:String = System.CsScala.Join("";"", Array(""one"", ""two""));
    }
}");
        }


        [TestMethod]
        public void RefAndOut()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Text;
		
namespace Blargh
{
	public class Foo
	{
		public Foo()
		{
			int x;
			TestOut(out x);
			x = 3;
			var s = x.ToString();
			int i = 1;
			TestRef(ref i);
			i = 5;
			new StringBuilder(i);
			Func<int> fun = () => x;
		}
		
		public void TestRef(ref int i)
		{
			var sb = new StringBuilder(i);
			i = 4;
		}
		public void TestOut(out int i)
		{
			i = 4;
			var sb = new StringBuilder(i);
		}
		
	}
}", @"
package Blargh;
" + WriteImports.StandardImports + @"
		
class Foo
{
	def TestRef(i:CsRef[Int])
	{
		var sb:System.Text.StringBuilder = new System.Text.StringBuilder(i.Value);
		i.Value = 4;
	}
		
	def TestOut(i:CsRef[Int])
	{
		i.Value = 4;
		var sb:System.Text.StringBuilder = new System.Text.StringBuilder(i.Value);
	}
	
	{
		var x:CsRef[Int] = new CsRef[Int](0);
		TestOut(x);
		x.Value = 3;
		var s:String = x.Value.toString();
		var i:CsRef[Int] = new CsRef[Int](1);
		TestRef(i);
		i.Value = 5;
		new System.Text.StringBuilder(i.Value);
		var fun:() => Int = () => { x.Value; }:Int;
	}
		
}");
        }


        [TestMethod]
        public void PartialMethods()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public partial class Foo
    {
        partial void NoOther();
		partial void Other();
    }

	partial class Foo
	{
		partial void Other()
		{
			Console.WriteLine();
		}
	}
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class Foo
{
	def NoOther()
	{
	}
	def Other()
	{
		System.Console.WriteLine();
	}
}");
        }


        [TestMethod]
        public void TypeConstraints()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static void SomeFunction<T>() where T : class, IComparable<T>
        {
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction[T >: Null <% System.IComparable[T]]()
    {
    }
}");
        }

        [TestMethod]
        public void ExplicitCastOperators()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Foo
{
    public class Bar
    {
        public static explicit operator string(Bar value)
		{
			return ""blah"";
		}

		public static void Foo()
		{
			var b = new Bar();
			var s = (string)b;
	
		}
    }
}", @"
package Foo;
" + WriteImports.StandardImports + @"

object Bar
{
    def op_Explicit_String(value:Foo.Bar):String =
    {
        return ""blah"";
    }
	def Foo()
	{
		var b:Foo.Bar = new Foo.Bar();
		var s:String = Foo.Bar.op_Explicit_String(b);
	}
}
class Bar
{
}");
        }



        [TestMethod]
        public void ParamsArguments()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Foo
{
    public static class Bar
    {
        public static void Method1(params int[] p)
		{
		}
        public static void Method2(int i, params int[] p)
		{
		}
        public static void Method3(int i, int z, params int[] p)
		{
		}

		public static void Foo()
		{
			Method1(1);
			Method1(1, 2);
			Method1(1, 2, 3);
			Method1(1, 2, 3, 4);
			Method2(1);
			Method2(1, 2);
			Method2(1, 2, 3);
			Method2(1, 2, 3, 4);
			Method3(1, 2);
			Method3(1, 2, 3);
			Method3(1, 2, 3, 4);

		}
    }
}", @"
package Foo;
" + WriteImports.StandardImports + @"

object Bar
{
    def Method1(p:Array[Int])
	{
	}
    def Method2(i:Int, p:Array[Int])
	{
	}
    def Method3(i:Int, z:Int, p:Array[Int])
	{
	}
	def Foo()
	{
		Foo.Bar.Method1(Array(1));
		Foo.Bar.Method1(Array(1, 2));
		Foo.Bar.Method1(Array(1, 2, 3));
		Foo.Bar.Method1(Array(1, 2, 3, 4));
		Foo.Bar.Method2(1);
		Foo.Bar.Method2(1, Array(2));
		Foo.Bar.Method2(1, Array(2, 3));
		Foo.Bar.Method2(1, Array(2, 3, 4));
		Foo.Bar.Method3(1, 2);
		Foo.Bar.Method3(1, 2, Array(3));
		Foo.Bar.Method3(1, 2, Array(3, 4));
	}
}");
        }

        [TestMethod]
        public void AccessStaticFromInstance()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public class Foo
    {
		static int StaticField = 1;
		static void StaticMethod() { }
        public void Method()
		{
			Console.WriteLine(StaticField);
			StaticMethod();
		}
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Foo
{
	var StaticField:Int = 1;
	def StaticMethod()
	{
	}
}

class Foo
{
	def Method()
	{
		System.Console.WriteLine(Blargh.Foo.StaticField);
		Blargh.Foo.StaticMethod();
	}
}
");
        }


        [TestMethod]
        public void ClassTag()
        {

            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;
using System.Linq;
using System.Collections.Generic;

namespace Blargh
{
    public class Foo
    {
        public void Method1<T1>(IEnumerable<T1> t)
		{
			t.ToList();
		}
        public void Method2<T2>(IEnumerable<T2> t)
		{
			
		}
        public void Method3<T3>(IEnumerable<T3> t)
		{
			Method1(t);
		}
        public void Method4<A4,B4,C4>(IEnumerable<A4> a, IEnumerable<B4> b, IEnumerable<C4> c)
		{
			b.OfType<C4>();
		}
        public void Method5<T5>(IEnumerable<T5> t)
		{
			Method4(t, new int[] { }, new int[] { });
		}
        public void Method6<T6>(IEnumerable<T6> t)
		{
			Method4(new int[] { }, t, new int[] { });
		}
        public void Method7<T7>(IEnumerable<T7> t)
		{
			Method4(new int[] { }, new int[] { }, t);
		}

    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

class Foo
{
	def Method1[T1:ClassTag](t:Traversable[T1])
	{
		System.Linq.Enumerable.ToList(t);
	}
	def Method2[T2](t:Traversable[T2])
	{
	}
	def Method3[T3:ClassTag](t:Traversable[T3])
	{
		Method1(t);
	}
	def Method4[A4, B4, C4:ClassTag](a:Traversable[A4], b:Traversable[B4], c:Traversable[C4])
	{
		System.Linq.Enumerable.OfType[B4, C4](b);
	}
	def Method5[T5](t:Traversable[T5])
	{
		Method4(t, Array[Int](), Array[Int]());
	}
	def Method6[T6](t:Traversable[T6])
	{
		Method4(Array[Int](), t, Array[Int]());
	}
	def Method7[T7:ClassTag](t:Traversable[T7])
	{
		Method4(Array[Int](), Array[Int](), t);
	}
}
");
        }

        [TestMethod]
        public void ByteConstants()
        {
            TestFramework.TestCode(MethodInfo.GetCurrentMethod().Name, @"
using System;

namespace Blargh
{
    public static class Utilities
    {
        public static Byte SomeFunction()
        {
			return 200;
        }
    }
}", @"
package Blargh;
" + WriteImports.StandardImports + @"

object Utilities
{
    def SomeFunction():Byte =
    {
        return 200.toByte;
    }
}");
        }
    }


}
