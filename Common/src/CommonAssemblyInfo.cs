// 
// Copyright (c) 2017 smdn <smdn@smdn.jp>
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

using System.Reflection;

[assembly: AssemblyCompany("smdn:総武ソフトウェア推進所")]
[assembly: AssemblyCopyright(AssemblyInfo.Copyright)]
[assembly: AssemblyTitle(AssemblyInfo.Name)]
[assembly: AssemblyProduct(AssemblyInfo.Name + "-" + AssemblyInfo.Version + " (" + AssemblyInfo.TargetFramework + ")")]
[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.Version + AssemblyInfo.VersionSuffix)]
[assembly: AssemblyConfiguration(AssemblyInfo.TargetFramework + AssemblyInfo.Configuration)]

internal static partial class AssemblyInfo {
  internal const string TargetFramework =
#if NET471
    ".NET 4.7.1"
#elif NET461
    ".NET 4.6.1"
#elif NET46
    ".NET 4.6"
#elif NET45
    ".NET 4.5"
#elif NETSTANDARD2_0
    ".NET Standard 2.0"
#elif NETSTANDARD1_6
    ".NET Standard 1.6"
#else
    ".NET"
#endif
    ;

  internal const string TargetFrameworkSuffix = 
#if NET471
    "net4.7.1"
#elif NET461
    "net4.6.1"
#elif NET46
    "net4.6"
#elif NET45
    "net4.5"
#elif NETSTANDARD2_0
    "netstandard2.0"
#elif NETSTANDARD1_6
    "netstandard1.6"
#else
    "net"
#endif
    ;

  internal const string Configuration =
#if DEBUG
    " Debug"
#else
    ""
#endif
    ;
}

