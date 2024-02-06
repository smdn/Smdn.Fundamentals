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

#if NET7_0_OR_GREATER
public interface IStaticMembers {
  static abstract void MStaticAbstract();
  static virtual void MStaticVirtual() => throw new NotImplementedException();
}

public class CImplicitlyImplementedStaticInterfaceMembers : IStaticMembers {
  static CImplicitlyImplementedStaticInterfaceMembers() { }

  public static void M() => throw new NotImplementedException();
  public static void MStaticAbstract() => throw new NotImplementedException();
  public static void MStaticVirtual() => throw new NotImplementedException();
}

public class CExplicitlyImplementedStaticInterfaceMembers : IStaticMembers {
  public static void M() => throw new NotImplementedException();
  static void IStaticMembers.MStaticAbstract() => throw new NotImplementedException();
  static void IStaticMembers.MStaticVirtual() => throw new NotImplementedException();
}

public interface IExplicitlyImplementedStaticInterfaceMembers : IStaticMembers {
  static void M() => throw new NotImplementedException();

  static void IStaticMembers.MStaticAbstract() => throw new NotImplementedException();
  static void IStaticMembers.MStaticVirtual() => throw new NotImplementedException();
}

public interface IExplicitlyImplementedStaticInterfaceMembers<TSelf> : IStaticMembers where TSelf : IExplicitlyImplementedStaticInterfaceMembers<TSelf> {
  static void M() => throw new NotImplementedException();

  static void IStaticMembers.MStaticAbstract() => throw new NotImplementedException();
  static void IStaticMembers.MStaticVirtual() => throw new NotImplementedException();
}

public interface IStaticMembersPublic {
  static abstract void M();
}

internal interface IStaticMembersInternal {
  static abstract void M();
}

public class CStaticMembers :
  IStaticMembersPublic,
  IStaticMembersInternal,
  CStaticMembers.IPublic,
  CStaticMembers.IInternal,
  CStaticMembers.IProtected,
  CStaticMembers.IProtectedInternal,
  CStaticMembers.IPrivateProtected,
  CStaticMembers.IPrivate
{
  static void IStaticMembersPublic.M() => throw new NotImplementedException();
  static void IStaticMembersInternal.M() => throw new NotImplementedException();
  static void IPublic.M() => throw new NotImplementedException();
  static void IInternal.M() => throw new NotImplementedException();
  static void IProtected.M() => throw new NotImplementedException();
  static void IProtectedInternal.M() => throw new NotImplementedException();
  static void IPrivateProtected.M() => throw new NotImplementedException();
  static void IPrivate.M() => throw new NotImplementedException();

  public interface IPublic {
    static abstract void M();
  }

  internal interface IInternal {
    static abstract void M();
  }

  protected interface IProtected {
    static abstract void M();
  }

  protected internal interface IProtectedInternal {
    static abstract void M();
  }

  private protected interface IPrivateProtected {
    static abstract void M();
  }

  private interface IPrivate {
    static abstract void M();
  }
}
#endif
