// SPDX-FileCopyrightText: 2025 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_DIAGNOSTICS_CODEANALYSIS_DYNAMICALLYACCESSEDMEMBERSATTRIBUTE
using System.Diagnostics.CodeAnalysis;
#endif
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Polly.Registry;
using Polly.Registry.KeyedRegistry;

namespace Polly;

public static class KeyedResiliencePipelineProviderServiceProviderExtensions {
  [CLSCompliant(false)] // ResiliencePipelineProvider is CLS incompliant
  public static ResiliencePipelineProvider<TPipelineKey>? GetKeyedResiliencePipelineProvider<TPipelineKey>(
    this IServiceProvider serviceProvider,
    object? serviceKey,
#if SYSTEM_DIAGNOSTICS_CODEANALYSIS_DYNAMICALLYACCESSEDMEMBERSATTRIBUTE
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
    Type typeOfKeyPair
  )
    where TPipelineKey : notnull
  {
    if (serviceProvider is null)
      throw new ArgumentNullException(nameof(serviceProvider));
    if (typeOfKeyPair is null)
      throw new ArgumentNullException(nameof(typeOfKeyPair));

    var serviceKeyWithTypeArguments = (
      serviceKey,
      typeOfKeyPair,
      serviceKey?.GetType(), // determine TServiceKey from type of serviceKey
      typeof(TPipelineKey)
    );

    if (typeOfKeyPair.IsGenericTypeDefinition) {
      var ifaceResiliencePipelineKeyPair = typeOfKeyPair
        .GetInterfaces()
        .FirstOrDefault(static iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IResiliencePipelineKeyPair<,>));

      if (ifaceResiliencePipelineKeyPair is not null) {
        // validate TPipelineKey of IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
        var typeOfPipelineKey = ifaceResiliencePipelineKeyPair.GetGenericArguments()[1];

        if (typeOfPipelineKey != typeof(TPipelineKey)) {
          throw new InvalidOperationException(
            message: typeOfPipelineKey.IsGenericTypeParameter
              ? $"{nameof(TPipelineKey)} must be an constructed type. ({nameof(typeOfKeyPair)}={typeOfKeyPair})"
              : $"The type of {nameof(TPipelineKey)} does not match. (expected {typeof(TPipelineKey)} but was {typeOfPipelineKey})"
          );
        }

        var provider = serviceProvider.GetKeyedService<ResiliencePipelineProvider<TPipelineKey>>(
          serviceKey: serviceKeyWithTypeArguments with {
            // attempt to retrieve a provider without determining the TServiceKey from the serviceKey type
            Item3 = null,
          }
        );

        if (provider is not null)
          return provider;
      }
    }

    return serviceProvider.GetKeyedService<ResiliencePipelineProvider<TPipelineKey>>(
      serviceKey: serviceKeyWithTypeArguments
    );
  }
}
