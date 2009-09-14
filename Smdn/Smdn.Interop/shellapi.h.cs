// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Runtime.InteropServices;

namespace Smdn.Interop {
  public enum SEE_MASK : uint {
    DEFAULT             = 0x00000000,
    INVOKEIDLIST        = 0x0000000c,
    NOCLOSEPROCESS      = 0x00000040,
    DOENVSUBST          = 0x00000200,
    FLAG_NO_UI          = 0x00000400,
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct SHELLEXECUTEINFO {
    public int cbSize;
    public SEE_MASK fMask;
    public IntPtr hwnd;
    public string lpVerb;
    public string lpFile;
    public string lpParameters;
    public string lpDirectory;
    public int nShow;
    public IntPtr hInstApp;
    public IntPtr lpIDList;
    public string lpClass;
    public IntPtr hkeyClass;
    public uint dwHotKey;
    public IntPtr hIcon;
    public IntPtr hProcess;

    public static readonly int Size = Marshal.SizeOf(typeof(SHELLEXECUTEINFO));
  }
}