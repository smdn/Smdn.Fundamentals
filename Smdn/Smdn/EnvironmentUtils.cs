// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
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
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Smdn {
  [Obsolete("use Smdn.OperatingSystem.EnvironmentUtils instead")]
  public static class EnvironmentUtils {
    public static Dictionary<string, string> ParseEnvironmentVariables(string variables)
    {
      return Smdn.OperatingSystem.EnvironmentUtils.ParseEnvironmentVariables(variables);
    }

    public static Dictionary<string, string> ParseEnvironmentVariables(string variables, bool throwIfInvalid)
    {
      return Smdn.OperatingSystem.EnvironmentUtils.ParseEnvironmentVariables(variables, throwIfInvalid);
    }

    public static string CombineEnvironmentVariables(IDictionary<string, string> variables)
    {
      return Smdn.OperatingSystem.EnvironmentUtils.CombineEnvironmentVariables(variables);
    }
  }
}