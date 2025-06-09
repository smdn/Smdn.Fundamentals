// SPDX-FileCopyrightText: 2025 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using NUnit.Framework;

using Polly.Registry;
using Polly.Registry.KeyedRegistry;

namespace Polly;

[TestFixture]
public class KeyedResiliencePipelineProviderServiceCollectionExtensionsTests {
  private record struct ResiliencePipelineKeyPair(string ServiceKey, int PipelineKey) : IResiliencePipelineKeyPair<string, int> { }

  [Test]
  public void AddResiliencePipeline()
  {
    var services = new ServiceCollection();

    const string ServiceKey = nameof(ServiceKey);
    const int PipelineKey = 1;

    ResiliencePipelineKeyPair? addedKey = default;
    IServiceProvider? addedServiceProvider = default;

    services.AddResiliencePipeline<ResiliencePipelineKeyPair, string, int>(
      resiliencePipelineKeyPair: new(ServiceKey, PipelineKey),
      configure: (builder, context) => {
        addedKey = context.PipelineKey;
        addedServiceProvider = context.ServiceProvider;
      }
    );

    var provider = services.BuildServiceProvider();
    var pipelineProvider = provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: ServiceKey);

    Assert.That(
      pipelineProvider,
      Is.TypeOf<KeyedResiliencePipelineProvider<ResiliencePipelineKeyPair, string, int>>()
    );
    Assert.That(
      pipelineProvider.GetPipeline(key: PipelineKey),
      Is.Not.Null
    );

    Assert.That(addedKey, Is.Not.Null);
    Assert.That(addedKey.Value.ServiceKey, Is.EqualTo(ServiceKey));
    Assert.That(addedKey.Value.PipelineKey, Is.EqualTo(PipelineKey));

    Assert.That(addedServiceProvider, Is.Not.Null);
  }

  [Test]
  public void AddResiliencePipeline_WithDiscreteServiceAndPipelineKey(
    [Values] bool withConfigureRegistryOptions
  )
  {
    var services = new ServiceCollection();

    const string ServiceKey = nameof(ServiceKey);
    const int PipelineKey = 1;

    ResiliencePipelineKeyPair? addedKey = default;
    IServiceProvider? addedServiceProvider = default;

    static ResiliencePipelineKeyPair CreateResiliencePipelineKeyPair(string serviceKey, int pipelineKey)
      => new(serviceKey, pipelineKey);

    if (withConfigureRegistryOptions) {
      services.AddResiliencePipeline(
        serviceKey: ServiceKey,
        pipelineKey: PipelineKey,
        createResiliencePipelineKeyPair: CreateResiliencePipelineKeyPair,
        configureRegistryOptions: null,
        configurePipeline: (builder, context) => {
          addedKey = context.PipelineKey;
          addedServiceProvider = context.ServiceProvider;
        }
      );
    }
    else {
      services.AddResiliencePipeline(
        serviceKey: ServiceKey,
        pipelineKey: PipelineKey,
        createResiliencePipelineKeyPair: CreateResiliencePipelineKeyPair,
        configure: (builder, context) => {
          addedKey = context.PipelineKey;
          addedServiceProvider = context.ServiceProvider;
        }
      );
    }

    var provider = services.BuildServiceProvider();
    var pipelineProvider = provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: ServiceKey);

    Assert.That(
      pipelineProvider,
      Is.TypeOf<KeyedResiliencePipelineProvider<ResiliencePipelineKeyPair, string, int>>()
    );
    Assert.That(
      pipelineProvider.GetPipeline(key: PipelineKey),
      Is.Not.Null
    );

    Assert.That(addedKey, Is.Not.Null);
    Assert.That(addedKey.Value.ServiceKey, Is.EqualTo(ServiceKey));
    Assert.That(addedKey.Value.PipelineKey, Is.EqualTo(PipelineKey));

    Assert.That(addedServiceProvider, Is.Not.Null);
  }

  [Test]
  public void AddResiliencePipeline_CreateResiliencePipelineKeyPair()
  {
    var services = new ServiceCollection();

    const string ServiceKey = nameof(ServiceKey);
    const int PipelineKey = 1;

    string? createdServiceKey = default;
    int? createdPipelineKey = default;
    string? addedServiceKey = default;
    int? addedPipelineKey = default;

    services.AddResiliencePipeline<ResiliencePipelineKeyPair, string, int>(
      resiliencePipelineKeyPair: new(ServiceKey, PipelineKey),
      createResiliencePipelineKeyPair: (serviceKey, pipelineKey) => {
        createdServiceKey = serviceKey;
        createdPipelineKey = pipelineKey;

        return new(serviceKey, pipelineKey);
      },
      configure: (builder, context) => {
        addedServiceKey = context.PipelineKey.ServiceKey;
        addedPipelineKey = context.PipelineKey.PipelineKey;
      }
    );

    var provider = services.BuildServiceProvider();
    var pipelineProvider = provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: ServiceKey);

    Assert.That(
      pipelineProvider,
      Is.TypeOf<KeyedResiliencePipelineProvider<ResiliencePipelineKeyPair, string, int>>()
    );
    Assert.That(
      pipelineProvider.GetPipeline(key: PipelineKey),
      Is.Not.Null
    );

    Assert.That(createdServiceKey, Is.Not.Null);
    Assert.That(createdServiceKey, Is.EqualTo(ServiceKey));
    Assert.That(createdPipelineKey, Is.Not.Null);
    Assert.That(createdPipelineKey, Is.EqualTo(PipelineKey));

    Assert.That(addedServiceKey, Is.Not.Null);
    Assert.That(addedServiceKey, Is.EqualTo(ServiceKey));
    Assert.That(addedPipelineKey, Is.Not.Null);
    Assert.That(addedPipelineKey, Is.EqualTo(PipelineKey));
  }

  [Test]
  public void AddResiliencePipeline_AddMultiplePipelineKeys()
  {
    var services = new ServiceCollection();

    const string ServiceKey = nameof(ServiceKey);
    const int PipelineKey1 = 1;
    const int PipelineKey2 = 2;

    services.AddResiliencePipeline<ResiliencePipelineKeyPair, string, int>(
      resiliencePipelineKeyPair: new(ServiceKey, PipelineKey1),
      configure: (builder, context) => { }
    );

    services.AddResiliencePipeline<ResiliencePipelineKeyPair, string, int>(
      resiliencePipelineKeyPair: new(ServiceKey, PipelineKey2),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();
    var pipelineProvider = provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: ServiceKey);

    Assert.That(pipelineProvider.GetPipeline(key: PipelineKey1), Is.Not.Null);
    Assert.That(pipelineProvider.GetPipeline(key: PipelineKey2), Is.Not.Null);
  }

  [Test]
  public void AddResiliencePipeline_AddMultipleServiceKeys()
  {
    var services = new ServiceCollection();

    const string ServiceKey1 = nameof(ServiceKey1);
    const string ServiceKey2 = nameof(ServiceKey2);
    const int PipelineKey1 = 1;
    const int PipelineKey2 = 2;

    services.AddResiliencePipeline<ResiliencePipelineKeyPair, string, int>(
      resiliencePipelineKeyPair: new(ServiceKey1, PipelineKey1),
      configure: (builder, context) => { }
    );

    services.AddResiliencePipeline<ResiliencePipelineKeyPair, string, int>(
      resiliencePipelineKeyPair: new(ServiceKey2, PipelineKey2),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();
    var pipelineProvider1 = provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: ServiceKey1);
    var pipelineProvider2 = provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: ServiceKey2);

    Assert.That(pipelineProvider1.GetPipeline(key: PipelineKey1), Is.Not.Null);
    Assert.That(() => pipelineProvider1.GetPipeline(key: PipelineKey2), Throws.TypeOf<KeyNotFoundException>());

    Assert.That(pipelineProvider2.GetPipeline(key: PipelineKey2), Is.Not.Null);
    Assert.That(() => pipelineProvider2.GetPipeline(key: PipelineKey1), Throws.TypeOf<KeyNotFoundException>());
  }
}


