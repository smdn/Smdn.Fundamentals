// SPDX-FileCopyrightText: 2025 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Polly.Registry.KeyedRegistry;

/// <summary>
/// Provides an interface for composing the pair of service key and pipeline key into a single type and
/// constructing them as keys used by <see cref="Polly.Registry.ResiliencePipelineProvider{TKey}"/>.
/// </summary>
/// <typeparam name="TServiceKey">The type for the service key.</typeparam>
/// <typeparam name="TPipelineKey">The type for the pipeline key.</typeparam>
public interface IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {
  /// <summary>
  /// Gets the service key associated with the specific <see cref="ResiliencePipelineProvider{TKey}"/>.
  /// </summary>
  TServiceKey ServiceKey { get; }

  /// <summary>
  /// Gets the pipeline key associated with the specific <see cref="ResiliencePipeline"/>
  /// registered with the <see cref="ResiliencePipelineProvider{TKey}"/>.
  /// </summary>
  TPipelineKey PipelineKey { get; }
}
