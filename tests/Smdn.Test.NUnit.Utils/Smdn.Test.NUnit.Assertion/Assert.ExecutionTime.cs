// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.Test.NUnit.Assertion;

[TestFixture]
public class AssertExecutionTimeTests {
  [SetUp]
  public void SetUp()
  {
    /*
     * warm up
     */
    Assert.Elapses(TimeSpan.FromMilliseconds(0), static () => Thread.Sleep(0));
    Assert.ElapsesAsync(TimeSpan.FromMilliseconds(0), static () => Task.Delay(0));

    Assert.NotElapse(TimeSpan.FromMilliseconds(100), static () => Thread.Sleep(0));
    Assert.NotElapseAsync(TimeSpan.FromMilliseconds(100), static () => Task.Delay(0));

    Assert.ElapsesInRange(TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(100), static () => Thread.Sleep(10));
    Assert.ElapsesInRangeAsync(TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(100), static () => Task.Delay(10));
  }

  [Test]
  [Retry(2)]
  public void Elapses(
    [Random(min: 30, max: 40, count: 10)] int milliseconds
  )
    => Assert.Elapses(
      expected: TimeSpan.FromMilliseconds(20),
      code: () => Thread.Sleep(milliseconds)
    );

  [Test]
  [Retry(2)]
  public void Elapses_Fail(
    [Random(min: 1, max: 10, count: 10)] int milliseconds
  )
    => Assert.Throws<AssertionException>(() =>
      Assert.Elapses(
        expected: TimeSpan.FromMilliseconds(20),
        code: () => Thread.Sleep(milliseconds)
      )
    );

  [Test]
  [Retry(2)]
  public void ElapsesAsync(
    [Random(min: 30, max: 40, count: 10)] int milliseconds
  )
    => Assert.ElapsesAsync(
      expected: TimeSpan.FromMilliseconds(20),
      code: () => Task.Delay(milliseconds)
    );

  [Test]
  [Retry(2)]
  public void ElapsesAsync_Fail(
    [Random(min: 1, max: 10, count: 10)] int milliseconds
  )
    => Assert.Throws<AssertionException>(() =>
      Assert.ElapsesAsync(
        expected: TimeSpan.FromMilliseconds(20),
        code: () => Task.Delay(milliseconds)
      )
    );

  [Test]
  [Retry(2)]
  public void NotElapse(
    [Random(min: 1, max: 10, count: 10)] int milliseconds
  )
    => Assert.NotElapse(
      expected: TimeSpan.FromMilliseconds(20),
      code: () => Thread.Sleep(milliseconds)
    );

  [Test]
  [Retry(2)]
  public void NotElapse_Fail(
    [Random(min: 30, max: 40, count: 10)] int milliseconds
  )
    => Assert.Throws<AssertionException>(() =>
      Assert.NotElapse(
        expected: TimeSpan.FromMilliseconds(20),
        code: () => Thread.Sleep(milliseconds)
      )
    );

  [Test]
  [Retry(2)]
  public void NotElapseAsync(
    [Random(min: 1, max: 10, count: 10)] int milliseconds
  )
    => Assert.NotElapseAsync(
      expected: TimeSpan.FromMilliseconds(20),
      code: () => Task.Delay(milliseconds)
    );

  [Test]
  [Retry(2)]
  public void NotElapseAsync_Fail(
    [Random(min: 30, max: 40, count: 10)] int milliseconds
  )
    => Assert.Throws<AssertionException>(() =>
      Assert.NotElapseAsync(
        expected: TimeSpan.FromMilliseconds(20),
        code: () => Task.Delay(milliseconds)
      )
    );

  [Test]
  [Retry(2)]
  public void ElapsesInRange(
    [Random(min: 20, max: 30, count: 10)] int milliseconds
  )
    => Assert.ElapsesInRange(
      expectedMin: TimeSpan.FromMilliseconds(10),
      expectedMax: TimeSpan.FromMilliseconds(40),
      code: () => Thread.Sleep(milliseconds)
    );

  [Test]
  [Retry(2)]
  public void ElapsesInRange_Fail_LessThanMin(
    [Random(min: 0, max: 10, count: 10)] int milliseconds
  )
    => Assert.Throws<AssertionException>(() =>
      Assert.ElapsesInRange(
        expectedMin: TimeSpan.FromMilliseconds(20),
        expectedMax: TimeSpan.FromMilliseconds(21),
        code: () => Thread.Sleep(milliseconds)
      )
    );

  [Test]
  [Retry(2)]
  public void ElapsesInRange_Fail_GreaterThanMax(
    [Random(min: 30, max: 40, count: 10)] int milliseconds
  )
    => Assert.Throws<AssertionException>(() =>
      Assert.ElapsesInRange(
        expectedMin: TimeSpan.FromMilliseconds(20),
        expectedMax: TimeSpan.FromMilliseconds(21),
        code: () => Thread.Sleep(milliseconds)
      )
    );

  [Test]
  [Retry(2)]
  public void ElapsesInRangeAsync(
    [Random(min: 20, max: 30, count: 10)] int milliseconds
  )
    => Assert.ElapsesInRangeAsync(
      expectedMin: TimeSpan.FromMilliseconds(10),
      expectedMax: TimeSpan.FromMilliseconds(40),
      code: () => Task.Delay(milliseconds)
    );

  [Test]
  [Retry(2)]
  public void ElapsesInRangeAsync_Fail_LessThanMin(
    [Random(min: 0, max: 10, count: 10)] int milliseconds
  )
    => Assert.Throws<AssertionException>(() =>
      Assert.ElapsesInRangeAsync(
        expectedMin: TimeSpan.FromMilliseconds(20),
        expectedMax: TimeSpan.FromMilliseconds(21),
        code: () => Task.Delay(milliseconds)
      )
    );

  [Test]
  [Retry(2)]
  public void ElapsesInRangeAsync_Fail_GreaterThanMax(
    [Random(min: 30, max: 40, count: 10)] int milliseconds
  )
    => Assert.Throws<AssertionException>(() =>
      Assert.ElapsesInRangeAsync(
        expectedMin: TimeSpan.FromMilliseconds(20),
        expectedMax: TimeSpan.FromMilliseconds(21),
        code: () => Task.Delay(milliseconds)
      )
    );
}
