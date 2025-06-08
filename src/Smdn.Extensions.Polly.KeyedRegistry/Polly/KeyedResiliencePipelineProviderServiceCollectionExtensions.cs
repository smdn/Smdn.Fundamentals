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
    //   PollyServiceCollectionExtensions.AddResiliencePipelineRegistry does not register
    //   the ResiliencePipelineProvider/Registry<TKey> alongside a service key.
    //   That means that the GetKeyedService<ResiliencePipelineProvider<TKey>> never works.
    //   Here, enables that requirements by registering additional ResiliencePipelineProvider that works
    //   with TPipelineKey as a keyed service.
    services.TryAddKeyedSingleton<ResiliencePipelineProvider<TPipelineKey>>(
      serviceKey: resiliencePipelineKeyPair.ServiceKey,
      implementationFactory: (serviceProvider, serviceKey)
        => new KeyedResiliencePipelineProvider<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(
          serviceKey: serviceKey is null ? default! : (TServiceKey)serviceKey,
          baseProvider: serviceProvider.GetRequiredService<ResiliencePipelineProvider<TResiliencePipelineKeyPair>>(),
          createResiliencePipelineKeyPair: createResiliencePipelineKeyPair
        )
    );

    return services;
  }
}
