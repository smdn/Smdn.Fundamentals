// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Test.NUnit.Assertion;

public partial class Assert : global::NUnit.Framework.Assert {
  public static TException ThrowsOrAggregates<TException>(TestDelegate code)
    where TException : Exception
  {
    try {
      code();

      Fail("expected exception {0} not thrown", typeof(TException).FullName);

      return null;
    }
    catch (AssertionException) {
      throw;
    }
    catch (AggregateException ex) {
      IsInstanceOf<TException>(ex.Flatten().InnerException);

      return ex.InnerException as TException;
    }
    catch (Exception ex) {
      IsInstanceOf<TException>(ex);

      return ex as TException;
    }
  }
}
