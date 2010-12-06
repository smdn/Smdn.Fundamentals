using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using NUnit.Framework;

namespace Smdn {
  [TestFixture]
  public class ExceptionUtilsTests {
    // NUnit 2.5
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    private class SetUICultureAttribute : Attribute {
      public string Name;

      public SetUICultureAttribute(string name)
      {
        this.Name = name;
      }
    }

    private void TestOnSpecificUICulture(Action test)
    {
      CultureInfo temporaryUICulture = null;

      var callerStackFrame = new StackFrame(1);
      var caller = callerStackFrame.GetMethod();

      foreach (var attr in caller.GetCustomAttributes(false)) {
        var uiCultureAttr = attr as SetUICultureAttribute;

        if (uiCultureAttr != null)
          temporaryUICulture = CultureInfo.GetCultureInfo(uiCultureAttr.Name);
      }

      var previousUICulture = Thread.CurrentThread.CurrentUICulture;

      try {
        if (temporaryUICulture != null)
          Thread.CurrentThread.CurrentUICulture = temporaryUICulture;

        test();
      }
      finally {
        Thread.CurrentThread.CurrentUICulture = previousUICulture;
      }
    }

    [Test, SetUICulture("ja-JP")]
    public void TestCreate_JA_JP()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

        StringAssert.StartsWith("ゼロまたは正の値を指定してください", ex.Message);
      });
    }

    [Test, SetUICulture("en-US")]
    public void TestCreate_EN_US()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

        StringAssert.StartsWith("must be zero or positive value", ex.Message);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreate_InvariantCulture()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

        StringAssert.StartsWith("must be zero or positive value", ex.Message);
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeNonZeroPositive()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeNonZeroPositive("arg", 0);

        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be non-zero positive value", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(0, ex.ActualValue);

        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeZeroOrPositive()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be zero or positive value", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(-1, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeLessThan()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeLessThan(2, "arg", 2);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be less than 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeLessThanNullMaxValue()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeLessThan(null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeLessThanOrEqualTo()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(2, "arg", 3);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be less than or equal to 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(3, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeLessThanOrEqualToNullMaxValue()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeGreaterThan()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeGreaterThan(2, "arg", 2);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be greater than 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeGreaterThanNullMaxValue()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeGreaterThan(null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeGreaterThanOrEqualTo()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(2, "arg", 1);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be greater than or equal to 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(1, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeGreaterThanOrEqualToNullMaxValue()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeInRange()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeInRange(0, 3, "arg", -1);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be in range 0 to 3", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(-1, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeInRangeNullFromValue()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeInRange(null, 3, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestCreateArgumentMustBeInRangeNullToValue()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeInRange(0, null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeMultipleOf()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeMultipleOf(2, "arg");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be multiple of 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeNonEmptyArray()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyArray("arg");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be a non-empty array", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfArray()
    {
      TestOnSpecificUICulture(delegate {
        var array = new[] {0, 1, 2, 3};
        var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", array, 2, 4);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("attempt to access beyond the end of an array (length=4, offset=2, count=4)", ex.Message);
        Assert.AreEqual("index", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfArrayNullArray()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", null, 2, 4);
  
        Assert.IsNull(ex.InnerException);
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("index", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeNonEmptyString()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyString("arg");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be a non-empty string", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeValidEnumValue1()
    {
      TestOnSpecificUICulture(delegate {
        var origin = (SeekOrigin)(-1);
        var ex = ExceptionUtils.CreateArgumentMustBeValidEnumValue("origin", origin);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("invalid enum value ( value=-1, type=System.IO.SeekOrigin)", ex.Message);
        Assert.AreEqual("origin", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeValidEnumValue2()
    {
      TestOnSpecificUICulture(delegate {
        var origin = (SeekOrigin)(-1);
        var ex = ExceptionUtils.CreateArgumentMustBeValidEnumValue("origin", origin, "invalid seek origin");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("invalid enum value (invalid seek origin value=-1, type=System.IO.SeekOrigin)", ex.Message);
        Assert.AreEqual("origin", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(NotSupportedException))]
    public void TestCreateNotSupportedEnumValue()
    {
      TestOnSpecificUICulture(delegate {
        var endian = Endianness.Unknown;
        var ex = ExceptionUtils.CreateNotSupportedEnumValue(endian);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("'Unknown' (Smdn.Endianness) is not supported", ex.Message);
  
        throw ex;
      });
    }
    
    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeReadableStream()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeReadableStream("baseStream");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support reading", ex.Message);
        Assert.AreEqual("baseStream", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeWritableStream()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeWritableStream("baseStream");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support writing", ex.Message);
        Assert.AreEqual("baseStream", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeSeekableStream()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeSeekableStream("baseStream");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support seeking", ex.Message);
        Assert.AreEqual("baseStream", ex.ParamName);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(NotSupportedException))]
    public void TestCreateNotSupportedReadingStream()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateNotSupportedReadingStream();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support reading", ex.Message);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(NotSupportedException))]
    public void TestCreateNotSupportedWritingStream()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateNotSupportedWritingStream();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support writing", ex.Message);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(NotSupportedException))]
    public void TestCreateNotSupportedSeekingStream()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateNotSupportedSeekingStream();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support seeking", ex.Message);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(NotSupportedException))]
    public void TestCreateNotSupportedSettingStreamLength()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateNotSupportedSettingStreamLength();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support setting length", ex.Message);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(IOException))]
    public void TestCreateIOAttemptToSeekBeforeStartOfStream()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("attempted to seek before start of stream", ex.Message);
  
        throw ex;
      });
    }

    [Test, SetUICulture(""), ExpectedException(typeof(ArgumentException))]
    public void TestCreateArgumentMustBeValidIAsyncResult()
    {
      TestOnSpecificUICulture(delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeValidIAsyncResult("asyncResult");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("invalid IAsyncResult", ex.Message);
        Assert.AreEqual("asyncResult", ex.ParamName);
  
        throw ex;
      });
    }
  }
}