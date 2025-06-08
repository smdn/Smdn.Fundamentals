// SPDX-FileCopyrightText: 2025 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif

using Microsoft.Extensions.DependencyInjection;

namespace Polly.Registry.KeyedRegistry;

/// <summary>
/// Provides an implementation of <see cref="ResiliencePipelineProvider{TKey}"/> for registering
/// with <see cref="ServiceCollection"/> along with the any service key of type <typeparamref name="TServiceKey"/>.
/// This implementation also provides functionality that unwraps and handles
/// the pipeline keys of <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}"/> only,
/// enabling it to work as a <see cref="ResiliencePipelineProvider{TPipelineKey}"/>.
/// </summary>
/// <typeparam name="TResiliencePipelineKeyPair">The type of composite key that implements <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}"/>.</typeparam>
/// <typeparam name="TServiceKey">The type for the service key.</typeparam>
/// <typeparam name="TPipelineKey">The type for the pipeline key.</typeparam>
#pragma warning disable IDE0055
[CLSCompliant(false)] // ResiliencePipelineProvider is not CLS compliant
public class KeyedResiliencePipelineProvider<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>
  : ResiliencePipelineProvider<TPipelineKey>
  where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
  where TPipelineKey : notnull
{
#pragma warning restore IDE0055
  private readonly Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair;

  /// <summary>
  /// Gets the service key associated with the current <see cref="ResiliencePipelineProvider{TKey}"/>.
  /// </summary>
  public TServiceKey ServiceKey { get; }

  /// <summary>
  /// Gets the <see cref="ResiliencePipelineProvider{TKey}"/> that is the base
  /// for the current <see cref="ResiliencePipelineProvider{TKey}"/>.
  /// </summary>
  public ResiliencePipelineProvider<TResiliencePipelineKeyPair> BaseProvider { get; }

  public KeyedResiliencePipelineProvider(
    TServiceKey serviceKey,
    ResiliencePipelineProvider<TResiliencePipelineKeyPair> baseProvider,
    Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair
  )
  {
    ServiceKey = serviceKey;
    BaseProvider = baseProvider ?? throw new ArgumentNullException(nameof(baseProvider));
    this.createResiliencePipelineKeyPair = createResiliencePipelineKeyPair ?? throw new ArgumentNullException(nameof(createResiliencePipelineKeyPair));
  }

  /// <summary>
  /// Attempt to get the <see cref="ResiliencePipeline"/> from the <see cref="BaseProvider"/>.
  /// </summary>
  /// <remarks>
  /// Ths method uses <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}"/>, which consists of
  /// <see cref="ServiceKey"/> and <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}.PipelineKey"/>,
  /// as the key to specify the target <see cref="ResiliencePipeline"/> to be retrieved.
  /// </remarks>
  public override bool TryGetPipeline(
    TPipelineKey key,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)] out ResiliencePipeline? pipeline
#else
    out ResiliencePipeline pipeline
#endif
  )
    => BaseProvider.TryGetPipeline(
      key: createResiliencePipelineKeyPair(ServiceKey, key),
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
      out pipeline
#else
      out pipeline!
#endif
    );

  /// <summary>
  /// Attempt to get the <see cref="ResiliencePipeline{TResult}"/> from the <see cref="BaseProvider"/>.
  /// </summary>
  /// <remarks>
  /// Ths method uses <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}"/>, which consists of
  /// <see cref="ServiceKey"/> and <see cref="IResiliencePipelineKeyPair{TServiceKey,TPipelineKey}.PipelineKey"/>,
  /// as the key to specify the target <see cref="ResiliencePipeline"/> to be retrieved.
  /// </remarks>
  public override bool TryGetPipeline<TResult>(
    TPipelineKey key,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)] out ResiliencePipeline<TResult>? pipeline
#else
    out ResiliencePipeline<TResult> pipeline
#endif
  )
    => BaseProvider.TryGetPipeline<TResult>(
      key: createResiliencePipelineKeyPair(ServiceKey, key),
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
      out pipeline
#else
      out pipeline!
#endif
    );
}
