// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace MethodBaseExtensionsTestTypes;

public interface I1 {
  void M();
}

internal interface I2 {
  void M();
}

public class C1 : I1, I2, C1.I3, C1.I4, C1.I5, C1.I6, C1.I7, C1.I8 {
  void I1.M() => throw new NotImplementedException();
  void I2.M() => throw new NotImplementedException();
  void I3.M() => throw new NotImplementedException();
  void I4.M() => throw new NotImplementedException();
  void I5.M() => throw new NotImplementedException();
  void I6.M() => throw new NotImplementedException();
  void I7.M() => throw new NotImplementedException();
  void I8.M() => throw new NotImplementedException();

  public interface I3 {
    void M();
  }

  internal interface I4 {
    void M();
  }

  protected interface I5 {
    void M();
  }

  internal protected interface I6 {
    void M();
  }

  private protected interface I7 {
    void M();
  }

  private interface I8 {
    void M();
  }
}
