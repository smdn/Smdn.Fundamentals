// SPDX-FileCopyrightText: 2025 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_DIAGNOSTICS_CODEANALYSIS_DYNAMICALLYACCESSEDMEMBERSATTRIBUTE
using System.Diagnostics.CodeAnalysis;
#endif

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Polly.DependencyInjection;
using Polly.Registry;
using Polly.Registry.KeyedRegistry;

namespace Polly;

public static class KeyedResiliencePipelineProviderServiceCollectionExtensions {
  [CLSCompliant(false)] // ResiliencePipelineBuilder is not CLS compliant
#pragma warning disable IDE0055
  public static
  IServiceCollection AddResiliencePipeline<
#if SYSTEM_DIAGNOSTICS_CODEANALYSIS_DYNAMICALLYACCESSEDMEMBERSATTRIBUTE
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
    TResiliencePipelineKeyPair,
    TServiceKey,
    TPipelineKey
  >(
    this IServiceCollection services,
    TResiliencePipelineKeyPair resiliencePipelineKeyPair,
    Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure
  )
    where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
    where TPipelineKey : notnull
#pragma warning restore IDE0055
    => AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
      services: services ?? throw new ArgumentNullException(nameof(services)),
      resiliencePipelineKeyPair: resiliencePipelineKeyPair,
      createResiliencePipelineKeyPair: CreateResiliencePipelineKeyPair<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>,
      configureRegistryOptions: null,
      configurePipeline: configure
    );

#pragma warning disable IDE0055
  private static
  TResiliencePipelineKeyPair CreateResiliencePipelineKeyPair<
#if SYSTEM_DIAGNOSTICS_CODEANALYSIS_DYNAMICALLYACCESSEDMEMBERSATTRIBUTE
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
    TResiliencePipelineKeyPair,
    TServiceKey,
    TPipelineKey
  >(TServiceKey serviceKey, TPipelineKey pipelineKey)
    where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
    where TPipelineKey : notnull
#pragma warning restore IDE0055
    => (TResiliencePipelineKeyPair)(Activator.CreateInstance(
      type: typeof(TResiliencePipelineKeyPair),
      args: new object?[] { serviceKey, pipelineKey }
    ) ?? throw new InvalidOperationException($"could not create instance of {typeof(TResiliencePipelineKeyPair)}"));

  [CLSCompliant(false)] // ResiliencePipelineBuilder is not CLS compliant
#pragma warning disable IDE0055
  public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
    this IServiceCollection services,
    TServiceKey serviceKey,
    TPipelineKey pipelineKey,
    Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair,
    Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure
  )
    where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
    where TPipelineKey : notnull
#pragma warning restore IDE0055
    => AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
      services: services ?? throw new ArgumentNullException(nameof(services)),
      resiliencePipelineKeyPair: (createResiliencePipelineKeyPair ?? throw new ArgumentNullException(nameof(createResiliencePipelineKeyPair)))
        .Invoke(serviceKey, pipelineKey),
      createResiliencePipelineKeyPair: createResiliencePipelineKeyPair,
      configureRegistryOptions: null,
      configurePipeline: configure
    );

  [CLSCompliant(false)] // ResiliencePipelineBuilder is not CLS compliant
#pragma warning disable IDE0055
  public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
    this IServiceCollection services,
    TServiceKey serviceKey,
    TPipelineKey pipelineKey,
    Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair,
    Action<ResiliencePipelineRegistryOptions<TResiliencePipelineKeyPair>>? configureRegistryOptions,
    Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configurePipeline
  )
    where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
    where TPipelineKey : notnull
#pragma warning restore IDE0055
    => AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
      services: services ?? throw new ArgumentNullException(nameof(services)),
      resiliencePipelineKeyPair: (createResiliencePipelineKeyPair ?? throw new ArgumentNullException(nameof(createResiliencePipelineKeyPair)))
        .Invoke(serviceKey, pipelineKey),
      createResiliencePipelineKeyPair: createResiliencePipelineKeyPair,
      configureRegistryOptions: configureRegistryOptions,
      configurePipeline: configurePipeline
    );

  [CLSCompliant(false)] // ResiliencePipelineBuilder is not CLS compliant
#pragma warning disable IDE0055
  public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
    this IServiceCollection services,
    TResiliencePipelineKeyPair resiliencePipelineKeyPair,
    Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair,
    Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure
  )
    where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
    where TPipelineKey : notnull
#pragma warning restore IDE0055
    => AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
      services: services ?? throw new ArgumentNullException(nameof(services)),
      resiliencePipelineKeyPair: resiliencePipelineKeyPair,
      createResiliencePipelineKeyPair: createResiliencePipelineKeyPair,
      configureRegistryOptions: null,
      configurePipeline: configure
    );

  /// <summary>
  /// Registers the <see cref="ResiliencePipeline"/> using the key <typeparamref name="TResiliencePipelineKeyPair"/>,
  /// which consists of the service key <typeparamref name="TServiceKey"/> and the pipeline key <typeparamref name="TPipelineKey"/>.
  /// </summary>
  /// <remarks>
  /// The <see cref="ResiliencePipeline"/> registered by this method can be retrieved from the
  /// <see cref="ResiliencePipelineProvider{TPipelineKey}"/> registered with the <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}.ServiceKey"/>.
  /// When retrieving <see cref="ResiliencePipeline"/> from <see cref="ResiliencePipelineProvider{TPipelineKey}"/>,
  /// specify only the pipeline key <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}.PipelineKey"/>.
  /// </remarks>
  /// <typeparam name="TResiliencePipelineKeyPair">The type of key that implements <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}"/>.</typeparam>
  /// <typeparam name="TServiceKey">The type for the service key.</typeparam>
  /// <typeparam name="TPipelineKey">The type for the pipeline key.</typeparam>
  [CLSCompliant(false)] // ResiliencePipelineBuilder is not CLS compliant
  public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
    this IServiceCollection services,
    TResiliencePipelineKeyPair resiliencePipelineKeyPair,
    Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair,
    Action<ResiliencePipelineRegistryOptions<TResiliencePipelineKeyPair>>? configureRegistryOptions,
    Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configurePipeline
  )
    where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
    where TPipelineKey : notnull
#pragma warning restore IDE0055
  {
    if (services is null)
      throw new ArgumentNullException(nameof(services));
    if (createResiliencePipelineKeyPair is null)
      throw new ArgumentNullException(nameof(createResiliencePipelineKeyPair));
    if (configurePipeline is null)
      throw new ArgumentNullException(nameof(configurePipeline));

    if (configureRegistryOptions is null)
      services.AddResiliencePipelineRegistry<TResiliencePipelineKeyPair>();
    else
      services.AddResiliencePipelineRegistry<TResiliencePipelineKeyPair>(configure: configureRegistryOptions);

    services.AddResiliencePipeline<TResiliencePipelineKeyPair>(
      key: resiliencePipelineKeyPair,
      configure: configurePipeline
    );

    // Add KeyedResiliencePipelineProvider<..., TPipelineKey> as a keyed ResiliencePipelineProvider<TPipelineKey>
    //
    // PollyServiceCollectionExtensions.AddResiliencePipelineRegistry does not register
    // the ResiliencePipelineProvider/Registry<TKey> alongside a service key.
    // That means that the GetKeyedService<ResiliencePipelineProvider<TKey>> never works.
    // Here, enables that requirements by registering additional ResiliencePipelineProvider that works
    // with TPipelineKey as a keyed service.
    // At this point, register the composite information, including not only the service key but also
    // the type information, as a new 'service key.'
    object? serviceKey = resiliencePipelineKeyPair.ServiceKey;
    var isGenericTypeDefKeyPairType = typeof(TResiliencePipelineKeyPair).IsGenericType && typeof(TResiliencePipelineKeyPair).IsGenericTypeDefinition;

    var serviceKeyWithTypeArguments = serviceKey is null || isGenericTypeDefKeyPairType
      // TServiceKey can not be determined at the time of GetKeyedService() is called since the serviceKey for this service is null
      ? (serviceKey, typeof(TResiliencePipelineKeyPair), null, typeof(TPipelineKey))
      : (serviceKey, typeof(TResiliencePipelineKeyPair), typeof(TServiceKey), typeof(TPipelineKey));

    services.TryAddKeyedSingleton<ResiliencePipelineProvider<TPipelineKey>>(
      serviceKey: serviceKey,
      implementationFactory: (serviceProvider, serviceKey)
        => serviceProvider.GetRequiredKeyedService<ResiliencePipelineProvider<TPipelineKey>>(
          serviceKey: serviceKeyWithTypeArguments
        )
    );

    // In cases where two ResiliencePipelineProviders are constructed with different types
    // of TResiliencePipelineKeyPair but the same ServiceKey value, it will not be possible
    // to distinguish between them.
    // In order to handle this situation, register the KeyedResiliencePipelineProvider in
    // addition using the combination of TResiliencePipelineKeyPair and ServiceKey as the key.
#pragma warning disable SA1141 // use tuple syntax
    services.TryAddKeyedSingleton<ResiliencePipelineProvider<TPipelineKey>>(
      serviceKey: serviceKeyWithTypeArguments,
      implementationFactory: (serviceProvider, serviceKey) => {
        TServiceKey deconstructedServiceKey = serviceKey is ValueTuple<object?, Type, Type, Type> serviceKeyTuple
          ? serviceKeyTuple.Item1 is null ? default! : (TServiceKey)serviceKeyTuple.Item1
          : default!;

        return new KeyedResiliencePipelineProvider<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
          serviceKey: deconstructedServiceKey,
          baseProvider: serviceProvider.GetRequiredService<ResiliencePipelineProvider<TResiliencePipelineKeyPair>>(),
          createResiliencePipelineKeyPair: createResiliencePipelineKeyPair
        );
      }
    );
#pragma warning restore SA1141

    if (typeof(TResiliencePipelineKeyPair).IsGenericType) {
      // this additional service resolves XxxKeyPair<> to XxxKeyPair<TServiceKey>
#pragma warning disable SA1141 // use tuple syntax
      services.TryAddKeyedSingleton<ResiliencePipelineProvider<TPipelineKey>>(
        serviceKey: serviceKeyWithTypeArguments with {
          // use the type definition of TResiliencePipelineKeyPair as the key element for TResiliencePipelineKeyPair
          // (e.g., XxxKeyPair<TServiceKey> to XxxKeyPair<>)
          Item2 = typeof(TResiliencePipelineKeyPair).GetGenericTypeDefinition(),
        },
        implementationFactory: static (serviceProvider, serviceKey)
          => serviceProvider.GetRequiredKeyedService<ResiliencePipelineProvider<TPipelineKey>>(
            serviceKey: (
              (ValueTuple<object?, Type, Type, Type>)(serviceKey ?? throw new ArgumentNullException(nameof(serviceKey)))
            ) with {
              // replace the key element from type definition to constructed type to get the service
              // (e.g., XxxKeyPair<> to XxxKeyPair<TServiceKey>)
              Item2 = typeof(TResiliencePipelineKeyPair),
            }
          )
      );
#pragma warning restore SA1141
    }

    return services;
  }
}
