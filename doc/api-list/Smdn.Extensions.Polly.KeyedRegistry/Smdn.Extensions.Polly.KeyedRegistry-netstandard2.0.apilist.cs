// Smdn.Extensions.Polly.KeyedRegistry.dll (Smdn.Extensions.Polly.KeyedRegistry-1.2.0)
//   Name: Smdn.Extensions.Polly.KeyedRegistry
//   AssemblyVersion: 1.2.0.0
//   InformationalVersion: 1.2.0+ab57871a61fa98809fd1f62940e326073110502c
//   TargetFramework: .NETStandard,Version=v2.0
//   Configuration: Release
//   Referenced assemblies:
//     Microsoft.Extensions.DependencyInjection.Abstractions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
//     Polly.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc
//     Polly.Extensions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc
//     netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
#nullable enable annotations

using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using Polly.Registry.KeyedRegistry;

namespace Polly {
  public static class KeyedResiliencePipelineProviderServiceCollectionExtensions {
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TResiliencePipelineKeyPair resiliencePipelineKeyPair, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TResiliencePipelineKeyPair resiliencePipelineKeyPair, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TResiliencePipelineKeyPair resiliencePipelineKeyPair, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair, Action<ResiliencePipelineRegistryOptions<TResiliencePipelineKeyPair>>? configureRegistryOptions, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configurePipeline) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TServiceKey serviceKey, TPipelineKey pipelineKey, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TServiceKey serviceKey, TPipelineKey pipelineKey, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair, Action<ResiliencePipelineRegistryOptions<TResiliencePipelineKeyPair>>? configureRegistryOptions, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configurePipeline) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
  }

  public static class KeyedResiliencePipelineProviderServiceProviderExtensions {
    public static ResiliencePipelineProvider<TPipelineKey>? GetKeyedResiliencePipelineProvider<TPipelineKey>(this IServiceProvider serviceProvider, object? serviceKey, Type typeOfKeyPair) where TPipelineKey : notnull {}
  }
}

namespace Polly.Registry.KeyedRegistry {
  public interface IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {
    TPipelineKey PipelineKey { get; }
    TServiceKey ServiceKey { get; }
  }

  public class KeyedResiliencePipelineProvider<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey> : ResiliencePipelineProvider<TPipelineKey> where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {
    public KeyedResiliencePipelineProvider(TServiceKey serviceKey, ResiliencePipelineProvider<TResiliencePipelineKeyPair> baseProvider, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair) {}

    public ResiliencePipelineProvider<TResiliencePipelineKeyPair> BaseProvider { get; }
    public TServiceKey ServiceKey { get; }

    public override bool TryGetPipeline(TPipelineKey key, out ResiliencePipeline pipeline) {}
    public override bool TryGetPipeline<TResult>(TPipelineKey key, out ResiliencePipeline<TResult> pipeline) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.6.0.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.4.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
