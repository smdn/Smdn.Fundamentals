// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Smdn.Reflection.Attributes {
  public static class ICustomAttributeProviderExtensions {
    public static IList<CustomAttributeData> GetCustomAttributeDataList(this ICustomAttributeProvider attributeProvider)
      => attributeProvider switch {
        Assembly assm => CustomAttributeData.GetCustomAttributes(assm),
        Module module => CustomAttributeData.GetCustomAttributes(module),
        MemberInfo member => CustomAttributeData.GetCustomAttributes(member),
        ParameterInfo param => CustomAttributeData.GetCustomAttributes(param),

        null => throw new ArgumentNullException(nameof(attributeProvider)),
        _ => throw new ArgumentException($"unsupported type of {nameof(ICustomAttributeProvider)}", nameof(attributeProvider)),
      };
  }
}
