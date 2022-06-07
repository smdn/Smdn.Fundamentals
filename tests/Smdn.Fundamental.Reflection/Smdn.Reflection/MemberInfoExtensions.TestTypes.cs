// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable IDE0044 // Make field readonly

using System;

namespace MemberInfoExtensionsTestTypes;

public class C1 { }
internal class C2 { }

class C {
  public class C3 { }
  internal class C4 { }
  protected class C5 { }
  protected internal class C6 { }
  protected private class C7 { }
  private class C8 {}

  public void M1() => throw new NotImplementedException();
  internal void M2() => throw new NotImplementedException();
  protected void M3() => throw new NotImplementedException();
  protected internal void M4() => throw new NotImplementedException();
  protected private void M5() => throw new NotImplementedException();
  private void M6() => throw new NotImplementedException();

#pragma warning disable 0649, 0169, 0410
  public int F1;
  internal int F2;
  protected int F3;
  protected internal int F4;
  protected private int F5;
  private int F6;
#pragma warning restore 0649, 0169, 0410

  public int P1 { get; set; }

#pragma warning disable 0067
  public event EventHandler E1;
#pragma warning restore 0067
}
