# C# Coding Standards and Naming Conventions


| Object Name               | Notation   | Plural | Prefix | Suffix | Abbreviation | Char Mask          | Underscores |
|:--------------------------|:-----------|:-------|:-------|:-------|:-------------|:-------------------|:------------|
| Namespace name            | PascalCase | Yes    | Yes    | No     | No           | [A-z][0-9]         | No          |
| Class name                | PascalCase | No     | No     | Yes    | No           | [A-z][0-9]         | No          |
| Constructor name          | PascalCase | No     | No     | Yes    | No           | [A-z][0-9]         | No          |
| Method name               | PascalCase | Yes    | No     | No     | No           | [A-z][0-9]         | No          |
| Method arguments          | camelCase  | Yes    | No     | No     | Yes          | [A-z][0-9]         | No          |
| Local variables           | camelCase  | Yes    | No     | No     | Yes          | [A-z][0-9]         | No          |
| Constants name            | PascalCase | No     | No     | No     | No           | [A-z][0-9]         | No          |
| Field name                | camelCase  | Yes    | No     | No     | Yes          | [A-z][0-9]         | Yes         |
| Properties name           | PascalCase | Yes    | No     | No     | Yes          | [A-z][0-9]         | No          |
| Delegate name             | PascalCase | No     | No     | Yes    | Yes          | [A-z]              | No          |
| Enum type name            | PascalCase | Yes    | No     | No     | No           | [A-z]              | No          |

**Use PascalCasing for class names and method names.**<br/>
**Use camelCasing for method arguments and local variables.**<br/>
**Use noun to name a class.** <br/>
**Use VerbObject or Verb to name a method.**
```csharp
public class ClientActivity
{
  public void ClearStatistics(LogEvent logEvent)
  {
    int itemCount = logEvent.Items.Count;
  }
  public void CalculateStatistics()
  {
    //...
  }
}
```

**Do not use All Caps for constants or readonly variables**
```csharp
// Correct
public const string ShippingType = "DropShip";
// Avoid
public const string SHIPPINGTYPE = "DropShip";
```

**Do not use Underscores in identifiers. Exception: you can prefix private fields with an underscore**
```csharp 
// Correct
public DateTime clientAppointment;  
// Avoid
public DateTime client_Appointment;
// Exception (Class field)
private DateTime _registrationDate;
```

**Use implicit type var for local variable declarations. Exception: primitive types (int, string, double, etc).**
```csharp 
var customers = new Dictionary();
// Exceptions
int index = 100;
string timeSheet;
bool isCompleted;
```

**Prefix interfaces with the letter I. Interface names are noun (phrases) or adjectives.**
```csharp     
public interface IShape  //n.
{
}
public interface ICollectable  //adj.
{
}
```

**Use singular names for enums. Exception: bit field enums**

```csharp 
// Correct
public enum Color
{
  Red,
  Green,
} 
// Exception
[Flags]
public enum Dockings
{
  None = 0,
  Top = 1,
  Right = 2, 
  Bottom = 4,
  Left = 8
}
```

**Use suffix EventArgs for classes containing event info**<br/>
**Use suffix EventHandler for event handlers (delegates)**<br/>
**Use suffix Exception for classes containing exception info**

```csharp 
// Correct
public class BarcodeReadEventArgs : System.EventArgs
{
}
public delegate void ReadBarcodeEventHandler(object sender, ReadBarcodeEventArgs e);
public class BarcodeReadException : System.Exception
{
}
```

**Use prefix Any, Is, Have or similar keywords for boolean identifier**

```csharp 
// Correct
public static bool IsNullOrEmpty(string value) {
    return (value == null || value.Length == 0);
}
```

**Use Named Arguments in method calls**
```csharp
// Method
public void DoSomething(string foo, int bar) {}
// Avoid
DoSomething("someString", 1);
// Correct
DoSomething(foo: "someString", bar: 1);
```


## Offical Reference

1. [MSDN General Naming Conventions](http://msdn.microsoft.com/en-us/library/ms229045(v=vs.110).aspx)
2. [DoFactory C# Coding Standards and Naming Conventions](http://www.dofactory.com/reference/csharp-coding-standards) 
3. [MSDN Naming Guidelines](http://msdn.microsoft.com/en-us/library/xzf533w0%28v=vs.71%29.aspx)
4. [MSDN Framework Design Guidelines](http://msdn.microsoft.com/en-us/library/ms229042.aspx)
