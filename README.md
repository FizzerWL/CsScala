# CsScala

CsScala uses Microsoft Roslyn to convert your C# 4.0 code to Scala 2.10 code.  It was primarilly invented to provide a way to run C# on the JVM.

Check out Test/Tests.cs for examples as well as an overview of what is supported.

C# is a large language, and there are some language features that CsScala doesn't support, such as unsafe code, goto, await, etc.

CsScala aims to support 90% of an average C# codebase.  You should expect going in to have to refactor some of your C# code to work around features that CsScala doesn't support.  See the limitations section for a full list.

# Limitations

Flat-out not supported: goto, yield, unsafe code, operator overloads, implicit cast operators, overloaded constructors, events, initializer blocks.

### Static Constructors

Scala doesn't have static constructors in the same way that C# does.  Instead, CsScala will write out your static constructor as a method named "cctor".  It is your responsibility to ensure this method gets called before you use the class.  

As a shortcut, CsScala will generate a Constructors type that contains calls to all static methods it generated. You can simply call this method at your program start-up and never have to worry about this again.

### Structs

Scala doesn't allow defining your own value-types like C# does.  CsScala will write out any structs as classes.  Any structures in you code will be a reference type in Scala, and this may cause bugs if your code assumes the structure is being copied by value.  

To fix this, change your structs to classes or make the structs immutable. (Mutable structs are a [bad idea](http://blogs.msdn.com/ericlippert/archive/2008/05/14/mutating-readonly-structs.aspx) in C# anyway.)  

### "Using" statements must reference a local variable

CsScala transforms your "using" statements to a try/finally block. Since the code in the using condition gets called beforehand in C# and afterwards in Scala, you can't put code with side-effects into the using condition. For example:


Don't do:
```
	using (SomeMethod()) { ... }
```

Instead, do:
```
	var someObject = SomeMethod();
	using (someObject) { ... }
```	


### Can't do math on char, convert to int instead
C# allows adding and subtracting from char types, whereas Scala does not.  To work around this, cast your chars to integers before doing math on them.

### Assignment, ++, and -- don't result in the assigned expression
Scala lacks the ++ and -- operators.  CsScala will re-write "i++" as a simple "i += 1".  However, that means that ++, --, and assignments can't be used as an expression.

### Can't re-assign method parameters
In Scala, all method parameters are immutable.  Therefore, they can't be re-assigned like they can in C#.

### Can't name constructor arguments the same as class fields
In Scala, all constructor arguments are visible throughout the class's implementation.  Therefore, names of constructor arguments must not conflict.

### Can't define overloaded functions with default parameters
C#'s method overloading is much more advanced than Scala's.  Scala does not allow using default parameters and overloading together.

### Base class libraries
CsScala comes with an implementation of many BCL functions.  The BCL is quite large, so obviously not all functions/classes are implemented.  You should browse through the provided code to see if everything your code references is implemented.  You may have to implement the missing functions yourself.
	
  
  


