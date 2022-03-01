// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Smdn {
  [TestFixture]
  public class ExceptionUtilsTests {
    [Test, SetUICulture("ja-JP")]
    public void TestCreate_JA_JP()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

      StringAssert.StartsWith("ゼロまたは正の値を指定してください", ex.Message);
    }

    [Test, SetUICulture("en-US")]
    public void TestCreate_EN_US()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

      StringAssert.StartsWith("must be zero or positive value", ex.Message);
    }

    [Test, SetUICulture("")]
    public void TestCreate_InvariantCulture()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

      StringAssert.StartsWith("must be zero or positive value", ex.Message);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonZeroPositive()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeNonZeroPositive("arg", 0);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be non-zero positive value", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(0, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeZeroOrPositive()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be zero or positive value", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(-1, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThan()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeLessThan(2, "arg", 2);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be less than 2", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(2, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanNullMaxValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeLessThan(null, "arg", 2);

      Assert.IsNotEmpty(ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(2, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanOrEqualTo()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(2, "arg", 3);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be less than or equal to 2", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(3, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanOrEqualToNullMaxValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(null, "arg", 2);

      Assert.IsNotEmpty(ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(2, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThan()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeGreaterThan(2, "arg", 2);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be greater than 2", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(2, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanNullMaxValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeGreaterThan(null, "arg", 2);

      Assert.IsNotEmpty(ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(2, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanOrEqualTo()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(2, "arg", 1);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be greater than or equal to 2", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(1, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanOrEqualToNullMaxValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(null, "arg", 2);

      Assert.IsNotEmpty(ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(2, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRange()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeInRange(0, 3, "arg", -1);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be in range 0 to 3", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(-1, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRangeNullFromValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeInRange(null, 3, "arg", 2);

      Assert.IsNotEmpty(ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(2, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRangeNullToValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeInRange(0, null, "arg", 2);

      Assert.IsNotEmpty(ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.AreEqual(2, ex.ActualValue);
      Assert.IsInstanceOf<ArgumentOutOfRangeException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeMultipleOf()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeMultipleOf(2, "arg");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be multiple of 2", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonEmptyArray()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyArray("arg");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be a non-empty array", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfArray()
    {
      var array = new[] {0, 1, 2, 3};
      var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", array, 2, 4);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("attempt to access beyond the end of an array (length=4, offset=2, count=4)", ex.Message);
      Assert.AreEqual("index", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfArrayNullArray()
    {
      var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", null, 2, 4);

      Assert.IsNull(ex.InnerException);
      Assert.IsNotEmpty(ex.Message);
      Assert.AreEqual("index", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonEmptyCollection()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyCollection("arg");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be a non-empty collection", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfCollection()
    {
      var collection = (IReadOnlyCollection<int>)new[] {0, 1, 2, 3};
      var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfCollection("index", collection, 2, 4);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("attempt to access beyond the end of a collection (length=4, offset=2, count=4)", ex.Message);
      Assert.AreEqual("index", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfCollection_CollectionNull()
    {
      var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfCollection("index", (IReadOnlyList<int>)null, 2, 4);

      Assert.IsNull(ex.InnerException);
      Assert.IsNotEmpty(ex.Message);
      Assert.AreEqual("index", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateAllItemsOfArgumentMustBeNonNull()
    {
      var ex = ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull("list");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("all items in the collection must be non-null", ex.Message);
      Assert.AreEqual("list", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonEmptyString()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyString("arg");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("must be a non-empty string", ex.Message);
      Assert.AreEqual("arg", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidEnumValue1()
    {
      var origin = (SeekOrigin)(-1);
      var ex = ExceptionUtils.CreateArgumentMustBeValidEnumValue("origin", origin);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("invalid enum value ( value=-1, type=System.IO.SeekOrigin)", ex.Message);
      Assert.AreEqual("origin", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidEnumValue2()
    {
      var origin = (SeekOrigin)(-1);
      var ex = ExceptionUtils.CreateArgumentMustBeValidEnumValue("origin", origin, "invalid seek origin");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("invalid enum value (invalid seek origin value=-1, type=System.IO.SeekOrigin)", ex.Message);
      Assert.AreEqual("origin", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedEnumValue()
    {
      var comparison = StringComparison.CurrentCulture;
      var ex = ExceptionUtils.CreateNotSupportedEnumValue(comparison);

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("'CurrentCulture' (System.StringComparison) is not supported", ex.Message);
      Assert.IsInstanceOf<NotSupportedException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeReadableStream()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeReadableStream("baseStream");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("stream does not support reading", ex.Message);
      Assert.AreEqual("baseStream", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeWritableStream()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeWritableStream("baseStream");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("stream does not support writing", ex.Message);
      Assert.AreEqual("baseStream", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeSeekableStream()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeSeekableStream("baseStream");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("stream does not support seeking", ex.Message);
      Assert.AreEqual("baseStream", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedReadingStream()
    {
      var ex = ExceptionUtils.CreateNotSupportedReadingStream();

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("stream does not support reading", ex.Message);
      Assert.IsInstanceOf<NotSupportedException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedWritingStream()
    {
      var ex = ExceptionUtils.CreateNotSupportedWritingStream();

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("stream does not support writing", ex.Message);
      Assert.IsInstanceOf<NotSupportedException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedSeekingStream()
    {
      var ex = ExceptionUtils.CreateNotSupportedSeekingStream();

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("stream does not support seeking", ex.Message);
      Assert.IsInstanceOf<NotSupportedException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedSettingStreamLength()
    {
      var ex = ExceptionUtils.CreateNotSupportedSettingStreamLength();

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("stream does not support setting length", ex.Message);
      Assert.IsInstanceOf<NotSupportedException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateIOAttemptToSeekBeforeStartOfStream()
    {
      var ex = ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("attempted to seek before start of stream", ex.Message);
      Assert.IsInstanceOf<IOException>(ex);
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidIAsyncResult()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeValidIAsyncResult("asyncResult");

      Assert.IsNull(ex.InnerException);
      StringAssert.StartsWith("invalid IAsyncResult", ex.Message);
      Assert.AreEqual("asyncResult", ex.ParamName);
      Assert.IsInstanceOf<ArgumentException>(ex);
    }
  }
}
