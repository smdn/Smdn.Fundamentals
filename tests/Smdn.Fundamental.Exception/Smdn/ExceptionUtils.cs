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

      Assert.That(ex.Message, Does.StartWith("ゼロまたは正の値を指定してください"));
    }

    [Test, SetUICulture("en-US")]
    public void TestCreate_EN_US()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

      Assert.That(ex.Message, Does.StartWith("must be zero or positive value"));
    }

    [Test, SetUICulture("")]
    public void TestCreate_InvariantCulture()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

      Assert.That(ex.Message, Does.StartWith("must be zero or positive value"));
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonZeroPositive()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeNonZeroPositive("arg", 0);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be non-zero positive value"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.Zero);
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeZeroOrPositive()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeZeroOrPositive("arg", -1);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be zero or positive value"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(-1));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThan()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeLessThan(2, "arg", 2);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be less than 2"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(2));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanNullMaxValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeLessThan(null, "arg", 2);

      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(2));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanOrEqualTo()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(2, "arg", 3);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be less than or equal to 2"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(3));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeLessThanOrEqualToNullMaxValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(null, "arg", 2);

      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(2));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThan()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeGreaterThan(2, "arg", 2);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be greater than 2"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(2));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanNullMaxValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeGreaterThan(null, "arg", 2);

      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(2));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanOrEqualTo()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(2, "arg", 1);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be greater than or equal to 2"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(1));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeGreaterThanOrEqualToNullMaxValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(null, "arg", 2);

      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(2));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRange()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeInRange(0, 3, "arg", -1);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be in range 0 to 3"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(-1));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRangeNullFromValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeInRange(null, 3, "arg", 2);

      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(2));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeInRangeNullToValue()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeInRange(0, null, "arg", 2);

      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex.ActualValue, Is.EqualTo(2));
      Assert.That(ex, Is.InstanceOf<ArgumentOutOfRangeException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeMultipleOf()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeMultipleOf(2, "arg");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be multiple of 2"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentXMustBeLessThanY()
    {
      var ex = ExceptionUtils.CreateArgumentXMustBeLessThanY(0, "min", 3, "max");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("`min` must be less than `max` (min=0, max=3)"));
      Assert.That(ex.ParamName, Is.EqualTo("min"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentXMustBeLessThanOrEqualToY()
    {
      var ex = ExceptionUtils.CreateArgumentXMustBeLessThanOrEqualToY(0, "min", 3, "max");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("`min` must be less than or equal to `max` (min=0, max=3)"));
      Assert.That(ex.ParamName, Is.EqualTo("min"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonEmptyArray()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyArray("arg");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be a non-empty array"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfArray()
    {
      var array = new[] {0, 1, 2, 3};
      var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", array, 2, 4);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("attempt to access beyond the end of an array (length=4, offset=2, count=4)"));
      Assert.That(ex.ParamName, Is.EqualTo("index"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfArrayNullArray()
    {
      var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", null, 2, 4);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.ParamName, Is.EqualTo("index"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonEmptyCollection()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyCollection("arg");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be a non-empty collection"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfCollection()
    {
      var collection = (IReadOnlyCollection<int>)new[] {0, 1, 2, 3};
      var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfCollection("index", collection, 2, 4);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("attempt to access beyond the end of a collection (length=4, offset=2, count=4)"));
      Assert.That(ex.ParamName, Is.EqualTo("index"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentAttemptToAccessBeyondEndOfCollection_CollectionNull()
    {
      var ex = ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfCollection("index", (IReadOnlyList<int>)null, 2, 4);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.ParamName, Is.EqualTo("index"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateAllItemsOfArgumentMustBeNonNull()
    {
      var ex = ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull("list");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("all items in the collection must be non-null"));
      Assert.That(ex.ParamName, Is.EqualTo("list"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustHaveLengthExact()
    {
      var ex = ExceptionUtils.CreateArgumentMustHaveLengthExact("arg", 4);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must have length exact 4"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustHaveLengthAtLeast()
    {
      var ex = ExceptionUtils.CreateArgumentMustHaveLengthAtLeast("arg", 4);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must have length at least 4"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeNonEmptyString()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeNonEmptyString("arg");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("must be a non-empty string"));
      Assert.That(ex.ParamName, Is.EqualTo("arg"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidEnumValue1()
    {
      var origin = (SeekOrigin)(-1);
      var ex = ExceptionUtils.CreateArgumentMustBeValidEnumValue("origin", origin);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("invalid enum value ( value=-1, type=System.IO.SeekOrigin)"));
      Assert.That(ex.ParamName, Is.EqualTo("origin"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidEnumValue2()
    {
      var origin = (SeekOrigin)(-1);
      var ex = ExceptionUtils.CreateArgumentMustBeValidEnumValue("origin", origin, "invalid seek origin");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("invalid enum value (invalid seek origin value=-1, type=System.IO.SeekOrigin)"));
      Assert.That(ex.ParamName, Is.EqualTo("origin"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedEnumValue()
    {
      var comparison = StringComparison.CurrentCulture;
      var ex = ExceptionUtils.CreateNotSupportedEnumValue(comparison);

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("'CurrentCulture' (System.StringComparison) is not supported"));
      Assert.That(ex, Is.InstanceOf<NotSupportedException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeReadableStream()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeReadableStream("baseStream");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("stream does not support reading"));
      Assert.That(ex.ParamName, Is.EqualTo("baseStream"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeWritableStream()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeWritableStream("baseStream");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("stream does not support writing"));
      Assert.That(ex.ParamName, Is.EqualTo("baseStream"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeSeekableStream()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeSeekableStream("baseStream");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("stream does not support seeking"));
      Assert.That(ex.ParamName, Is.EqualTo("baseStream"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedReadingStream()
    {
      var ex = ExceptionUtils.CreateNotSupportedReadingStream();

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("stream does not support reading"));
      Assert.That(ex, Is.InstanceOf<NotSupportedException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedWritingStream()
    {
      var ex = ExceptionUtils.CreateNotSupportedWritingStream();

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("stream does not support writing"));
      Assert.That(ex, Is.InstanceOf<NotSupportedException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedSeekingStream()
    {
      var ex = ExceptionUtils.CreateNotSupportedSeekingStream();

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("stream does not support seeking"));
      Assert.That(ex, Is.InstanceOf<NotSupportedException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateNotSupportedSettingStreamLength()
    {
      var ex = ExceptionUtils.CreateNotSupportedSettingStreamLength();

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("stream does not support setting length"));
      Assert.That(ex, Is.InstanceOf<NotSupportedException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateIOAttemptToSeekBeforeStartOfStream()
    {
      var ex = ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("attempted to seek before start of stream"));
      Assert.That(ex, Is.InstanceOf<IOException>());
    }

    [Test, SetUICulture("")]
    public void TestCreateArgumentMustBeValidIAsyncResult()
    {
      var ex = ExceptionUtils.CreateArgumentMustBeValidIAsyncResult("asyncResult");

      Assert.That(ex.InnerException, Is.Null);
      Assert.That(ex.Message, Does.StartWith("invalid IAsyncResult"));
      Assert.That(ex.ParamName, Is.EqualTo("asyncResult"));
      Assert.That(ex, Is.InstanceOf<ArgumentException>());
    }
  }
}
