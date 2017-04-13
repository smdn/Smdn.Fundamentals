using System;
using System.Globalization;
using System.IO;
using System.Reflection;
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

    private void TestOnSpecificUICulture(string cultureName, Action test)
    {
      CultureInfo temporaryUICulture = CultureInfo.InvariantCulture;

      if (!string.IsNullOrEmpty(cultureName))
        temporaryUICulture = new CultureInfo(cultureName);

      var previousUICulture = CultureInfo.CurrentUICulture;

      try {
        if (temporaryUICulture != null)
          CultureInfo.CurrentUICulture = temporaryUICulture;

        test();
      }
      finally {
        CultureInfo.CurrentUICulture = previousUICulture;
      }
    }

    [Test, SetUICulture("ja-JP")]
    public void TestCreate_JA_JP()
    {
      TestOnSpecificUICulture("ja-JP", delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

        StringAssert.StartsWith("ゼロまたは正の値を指定してください", ex.Message);
      });
    }

    [Test, SetUICulture("en-US")]
    public void TestCreate_EN_US()
    {
      TestOnSpecificUICulture("en-US", delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

        StringAssert.StartsWith("must be zero or positive value", ex.Message);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreate_InvariantCulture()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

        StringAssert.StartsWith("must be zero or positive value", ex.Message);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonZeroPositive()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeNonZeroPositive("arg", 0);

        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be non-zero positive value", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(0, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeZeroOrPositive()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be zero or positive value", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(-1, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThan()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeLessThan(2, "arg", 2);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be less than 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanNullMaxValue()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeLessThan(null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanOrEqualTo()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(2, "arg", 3);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be less than or equal to 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(3, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanOrEqualToNullMaxValue()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThan()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeGreaterThan(2, "arg", 2);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be greater than 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanNullMaxValue()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeGreaterThan(null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanOrEqualTo()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(2, "arg", 1);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be greater than or equal to 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(1, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanOrEqualToNullMaxValue()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRange()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeInRange(0, 3, "arg", -1);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be in range 0 to 3", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(-1, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRangeNullFromValue()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeInRange(null, 3, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRangeNullToValue()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeInRange(0, null, "arg", 2);
  
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.AreEqual(2, ex.ActualValue);
        Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeMultipleOf()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeMultipleOf(2, "arg");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be multiple of 2", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonEmptyArray()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyArray("arg");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be a non-empty array", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfArray()
    {
      TestOnSpecificUICulture(null, delegate {
        var array = new[] {0, 1, 2, 3};
        var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", array, 2, 4);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("attempt to access beyond the end of an array (length=4, offset=2, count=4)", ex.Message);
        Assert.AreEqual("index", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfArrayNullArray()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", null, 2, 4);
  
        Assert.IsNull(ex.InnerException);
        Assert.IsNotEmpty(ex.Message);
        Assert.AreEqual("index", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonEmptyString()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyString("arg");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("must be a non-empty string", ex.Message);
        Assert.AreEqual("arg", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidEnumValue1()
    {
      TestOnSpecificUICulture(null, delegate {
        var origin = (SeekOrigin)(-1);
        var ex = ExceptionUtils.CreateArgumentMustBeValidEnumValue("origin", origin);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("invalid enum value ( value=-1, type=System.IO.SeekOrigin)", ex.Message);
        Assert.AreEqual("origin", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidEnumValue2()
    {
      TestOnSpecificUICulture(null, delegate {
        var origin = (SeekOrigin)(-1);
        var ex = ExceptionUtils.CreateArgumentMustBeValidEnumValue("origin", origin, "invalid seek origin");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("invalid enum value (invalid seek origin value=-1, type=System.IO.SeekOrigin)", ex.Message);
        Assert.AreEqual("origin", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedEnumValue()
    {
      TestOnSpecificUICulture(null, delegate {
        var endian = Endianness.Unknown;
        var ex = ExceptionUtils.CreateNotSupportedEnumValue(endian);
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("'Unknown' (Smdn.Endianness) is not supported", ex.Message);
        Assert.IsInstanceOf<NotSupportedException>(ex);
      });
    }
    
    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeReadableStream()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeReadableStream("baseStream");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support reading", ex.Message);
        Assert.AreEqual("baseStream", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeWritableStream()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeWritableStream("baseStream");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support writing", ex.Message);
        Assert.AreEqual("baseStream", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeSeekableStream()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeSeekableStream("baseStream");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support seeking", ex.Message);
        Assert.AreEqual("baseStream", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedReadingStream()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateNotSupportedReadingStream();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support reading", ex.Message);
        Assert.IsInstanceOf<NotSupportedException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedWritingStream()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateNotSupportedWritingStream();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support writing", ex.Message);
        Assert.IsInstanceOf<NotSupportedException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedSeekingStream()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateNotSupportedSeekingStream();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support seeking", ex.Message);
        Assert.IsInstanceOf<NotSupportedException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedSettingStreamLength()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateNotSupportedSettingStreamLength();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("stream does not support setting length", ex.Message);
        Assert.IsInstanceOf<NotSupportedException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateIOAttemptToSeekBeforeStartOfStream()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("attempted to seek before start of stream", ex.Message);
        Assert.IsInstanceOf<IOException>(ex);
      });
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidIAsyncResult()
    {
      TestOnSpecificUICulture(null, delegate {
        var ex = ExceptionUtils.CreateArgumentMustBeValidIAsyncResult("asyncResult");
  
        Assert.IsNull(ex.InnerException);
        StringAssert.StartsWith("invalid IAsyncResult", ex.Message);
        Assert.AreEqual("asyncResult", ex.ParamName);
        Assert.IsInstanceOf<ArgumentException>(ex);
      });
    }
  }
}