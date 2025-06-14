// Smdn.Extensions.Polly.KeyedRegistry.dll (Smdn.Extensions.Polly.KeyedRegistry-1.2.0)
//   Name: Smdn.Extensions.Polly.KeyedRegistry
//   AssemblyVersion: 1.2.0.0
//   InformationalVersion: 1.2.0+ab57871a61fa98809fd1f62940e326073110502c
//   TargetFramework: .NETCoreApp,Version=v8.0
//   Configuration: Release
//   Referenced assemblies:
//     Microsoft.Extensions.DependencyInjection.Abstractions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
//     Polly.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc
//     Polly.Extensions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=c8a3ffc3f8f825cc
//     System.ComponentModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Linq, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using Polly.Registry.KeyedRegistry;

namespace Polly {
  public static class KeyedResiliencePipelineProviderServiceCollectionExtensions {
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TResiliencePipelineKeyPair resiliencePipelineKeyPair, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TResiliencePipelineKeyPair resiliencePipelineKeyPair, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair, Action<ResiliencePipelineRegistryOptions<TResiliencePipelineKeyPair>>? configureRegistryOptions, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configurePipeline) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TServiceKey serviceKey, TPipelineKey pipelineKey, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
    public static IServiceCollection AddResiliencePipeline<TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TServiceKey serviceKey, TPipelineKey pipelineKey, Func<TServiceKey, TPipelineKey, TResiliencePipelineKeyPair> createResiliencePipelineKeyPair, Action<ResiliencePipelineRegistryOptions<TResiliencePipelineKeyPair>>? configureRegistryOptions, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configurePipeline) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
    public static IServiceCollection AddResiliencePipeline<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TResiliencePipelineKeyPair, TServiceKey, TPipelineKey>(this IServiceCollection services, TResiliencePipelineKeyPair resiliencePipelineKeyPair, Action<ResiliencePipelineBuilder, AddResiliencePipelineContext<TResiliencePipelineKeyPair>> configure) where TResiliencePipelineKeyPair : notnull, IResiliencePipelineKeyPair<TServiceKey, TPipelineKey> where TPipelineKey : notnull {}
  }

  public static class KeyedResiliencePipelineProviderServiceProviderExtensions {
    public static ResiliencePipelineProvider<TPipelineKey>? GetKeyedResiliencePipelineProvider<TPipelineKey>(this IServiceProvider serviceProvider, object? serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type typeOfKeyPair) where TPipelineKey : notnull {}
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

    public override bool TryGetPipeline(TPipelineKey key, [NotNullWhen(true)] out ResiliencePipeline? pipeline) {}
    public override bool TryGetPipeline<TResult>(TPipelineKey key, [NotNullWhen(true)] out ResiliencePipeline<TResult>? pipeline) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.4.1.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.3.1.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
