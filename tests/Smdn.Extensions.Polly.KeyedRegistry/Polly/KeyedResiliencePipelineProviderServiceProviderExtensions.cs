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
public class KeyedResiliencePipelineProviderServiceProviderExtensionsTests {
  private record struct PipelineKeyPair1(string ServiceKey, int PipelineKey) : IResiliencePipelineKeyPair<string, int> { }
  private record struct PipelineKeyPair2(string ServiceKey, int PipelineKey) : IResiliencePipelineKeyPair<string, int> { }

  [Test]
  public void GetKeyedResiliencePipelineProvider()
  {
    var services = new ServiceCollection();

    const string CommonServiceKey = nameof(CommonServiceKey);
    const string OtherServiceKey = nameof(OtherServiceKey);
    const int PipelineKey1 = 1;
    const int PipelineKey2 = 2;
    const int PipelineKey3 = 3;
    const int PipelineKey4 = 4;

    services.AddResiliencePipeline<PipelineKeyPair1, string, int>(
      resiliencePipelineKeyPair: new PipelineKeyPair1(CommonServiceKey, PipelineKey1),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<PipelineKeyPair2, string, int>(
      resiliencePipelineKeyPair: new PipelineKeyPair2(CommonServiceKey, PipelineKey2),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<PipelineKeyPair2, string, int>(
      resiliencePipelineKeyPair: new PipelineKeyPair2(CommonServiceKey, PipelineKey3),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<PipelineKeyPair2, string, int>(
      resiliencePipelineKeyPair: new PipelineKeyPair2(OtherServiceKey, PipelineKey4),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    // get KeyedResiliencePipelineProvider<PipelineKeyPair1, string, int> with CommonServiceKey
    var pipelineProviderOfKeyPair1 = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: CommonServiceKey,
      typeOfKeyPair: typeof(PipelineKeyPair1)
    );

    Assert.That(pipelineProviderOfKeyPair1, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair1,
      Is.TypeOf<KeyedResiliencePipelineProvider<PipelineKeyPair1, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey1, out _), Is.True);
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey2, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey3, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey4, out _), Is.False);

    // get KeyedResiliencePipelineProvider<PipelineKeyPair2, string, int> with CommonServiceKey
    var pipelineProviderOfKeyPair2 = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: CommonServiceKey,
      typeOfKeyPair: typeof(PipelineKeyPair2)
    );

    Assert.That(pipelineProviderOfKeyPair2, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair2,
      Is.TypeOf<KeyedResiliencePipelineProvider<PipelineKeyPair2, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey1, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey2, out _), Is.True);
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey3, out _), Is.True);
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey4, out _), Is.False);

    // get KeyedResiliencePipelineProvider<PipelineKeyPair2, string, int> with OtherServiceKey
    var pipelineProviderOfKeyPair2ForOtherServiceKey = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: OtherServiceKey,
      typeOfKeyPair: typeof(PipelineKeyPair2)
    );

    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair2ForOtherServiceKey,
      Is.TypeOf<KeyedResiliencePipelineProvider<PipelineKeyPair2, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey.TryGetPipeline(PipelineKey1, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey.TryGetPipeline(PipelineKey2, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey.TryGetPipeline(PipelineKey3, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey.TryGetPipeline(PipelineKey4, out _), Is.True);

    // get any ResiliencePipelineProvider<int> with CommonServiceKey
    Assert.That(
      provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: CommonServiceKey),
      Is.SameAs(pipelineProviderOfKeyPair1).Or.SameAs(pipelineProviderOfKeyPair2)
    );

    // get any ResiliencePipelineProvider<int> with OtherServiceKey
    Assert.That(
      provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: OtherServiceKey),
      Is.SameAs(pipelineProviderOfKeyPair2ForOtherServiceKey)
    );
  }

  [Test]
  public void GetKeyedResiliencePipelineProvider_NullServiceKey()
  {
    var services = new ServiceCollection();

    const string NullServiceKey = null!;
    const int PipelineKey1 = 1;
    const int PipelineKey2 = 2;

    services.AddResiliencePipeline<PipelineKeyPair1, string, int>(
      resiliencePipelineKeyPair: new PipelineKeyPair1(NullServiceKey, PipelineKey1),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<PipelineKeyPair2, string, int>(
      resiliencePipelineKeyPair: new PipelineKeyPair2(NullServiceKey, PipelineKey2),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    // get KeyedResiliencePipelineProvider<PipelineKeyPair1, string, int> with NullServiceKey
    var pipelineProviderOfKeyPair1 = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: NullServiceKey,
      typeOfKeyPair: typeof(PipelineKeyPair1)
    );

    Assert.That(pipelineProviderOfKeyPair1, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair1,
      Is.TypeOf<KeyedResiliencePipelineProvider<PipelineKeyPair1, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey1, out _), Is.True);
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey2, out _), Is.False);

    // get KeyedResiliencePipelineProvider<PipelineKeyPair2, string, int> with NullServiceKey
    var pipelineProviderOfKeyPair2 = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: NullServiceKey,
      typeOfKeyPair: typeof(PipelineKeyPair2)
    );

    Assert.That(pipelineProviderOfKeyPair2, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair2,
      Is.TypeOf<KeyedResiliencePipelineProvider<PipelineKeyPair2, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey1, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey2, out _), Is.True);

    // get any ResiliencePipelineProvider<int> with NullServiceKey
    Assert.That(
      provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: NullServiceKey),
      Is.SameAs(pipelineProviderOfKeyPair1).Or.SameAs(pipelineProviderOfKeyPair2)
    );
  }

  private record struct OpenGenericPipelineKeyPair1<TServiceKey>(TServiceKey ServiceKey, int PipelineKey) : IResiliencePipelineKeyPair<TServiceKey, int> { }
  private record struct OpenGenericPipelineKeyPair2<TServiceKey>(TServiceKey ServiceKey, int PipelineKey) : IResiliencePipelineKeyPair<TServiceKey, int> { }

  [Test]
  public void GetKeyedResiliencePipelineProvider_OpenGenericKeyPairType()
  {
    var services = new ServiceCollection();

    const string CommonServiceKey = nameof(CommonServiceKey);
    const string OtherServiceKey = nameof(OtherServiceKey);
    const int PipelineKey1 = 1;
    const int PipelineKey2 = 2;
    const int PipelineKey3 = 3;
    const int PipelineKey4 = 4;

    services.AddResiliencePipeline<OpenGenericPipelineKeyPair1<string>, string, int>(
      resiliencePipelineKeyPair: new OpenGenericPipelineKeyPair1<string>(CommonServiceKey, PipelineKey1),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<OpenGenericPipelineKeyPair2<string>, string, int>(
      resiliencePipelineKeyPair: new OpenGenericPipelineKeyPair2<string>(CommonServiceKey, PipelineKey2),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<OpenGenericPipelineKeyPair2<string>, string, int>(
      resiliencePipelineKeyPair: new OpenGenericPipelineKeyPair2<string>(CommonServiceKey, PipelineKey3),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<OpenGenericPipelineKeyPair2<string>, string, int>(
      resiliencePipelineKeyPair: new OpenGenericPipelineKeyPair2<string>(OtherServiceKey, PipelineKey4),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    // get KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair1<string>, string, int> with CommonServiceKey
    var pipelineProviderOfKeyPair1 = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: CommonServiceKey,
      typeOfKeyPair: typeof(OpenGenericPipelineKeyPair1<>)
    );

    Assert.That(pipelineProviderOfKeyPair1, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair1,
      Is.TypeOf<KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair1<string>, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey1, out _), Is.True);
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey2, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey3, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey4, out _), Is.False);

    // get KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair2<string>, string, int> with CommonServiceKey
    var pipelineProviderOfKeyPair2 = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: CommonServiceKey,
      typeOfKeyPair: typeof(OpenGenericPipelineKeyPair2<>)
    );

    Assert.That(pipelineProviderOfKeyPair2, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair2,
      Is.TypeOf<KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair2<string>, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey1, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey2, out _), Is.True);
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey3, out _), Is.True);
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey4, out _), Is.False);

    // get KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair2<string>, string, int> with OtherServiceKey
    var pipelineProviderOfKeyPair2ForOtherServiceKey = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: OtherServiceKey,
      typeOfKeyPair: typeof(OpenGenericPipelineKeyPair2<>)
    );

    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair2ForOtherServiceKey,
      Is.TypeOf<KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair2<string>, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey.TryGetPipeline(PipelineKey1, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey.TryGetPipeline(PipelineKey2, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey.TryGetPipeline(PipelineKey3, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2ForOtherServiceKey.TryGetPipeline(PipelineKey4, out _), Is.True);

    // get any ResiliencePipelineProvider<int> with CommonServiceKey
    Assert.That(
      provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: CommonServiceKey),
      Is.SameAs(pipelineProviderOfKeyPair1).Or.SameAs(pipelineProviderOfKeyPair2)
    );

    // get any ResiliencePipelineProvider<int> with OtherServiceKey
    Assert.That(
      provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: OtherServiceKey),
      Is.SameAs(pipelineProviderOfKeyPair2ForOtherServiceKey)
    );
  }

  [Test]
  public void GetKeyedResiliencePipelineProvider_OpenGenericKeyPairType_NullServiceKey()
  {
    var services = new ServiceCollection();

    const string NullServiceKey = null!;
    const int PipelineKey1 = 1;
    const int PipelineKey2 = 2;

    services.AddResiliencePipeline<OpenGenericPipelineKeyPair1<string>, string, int>(
      resiliencePipelineKeyPair: new OpenGenericPipelineKeyPair1<string>(NullServiceKey, PipelineKey1),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<OpenGenericPipelineKeyPair2<string>, string, int>(
      resiliencePipelineKeyPair: new OpenGenericPipelineKeyPair2<string>(NullServiceKey, PipelineKey2),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    // get KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair1<string>, string, int> with NullServiceKey
    var pipelineProviderOfKeyPair1 = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: NullServiceKey,
      typeOfKeyPair: typeof(OpenGenericPipelineKeyPair1<>)
    );

    Assert.That(pipelineProviderOfKeyPair1, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair1,
      Is.TypeOf<KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair1<string>, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey1, out _), Is.True);
    Assert.That(pipelineProviderOfKeyPair1.TryGetPipeline(PipelineKey2, out _), Is.False);

    // get KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair2<string>, string, int> with NullServiceKey
    var pipelineProviderOfKeyPair2 = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: NullServiceKey,
      typeOfKeyPair: typeof(OpenGenericPipelineKeyPair2<>)
    );

    Assert.That(pipelineProviderOfKeyPair2, Is.Not.Null);
    Assert.That(
      pipelineProviderOfKeyPair2,
      Is.TypeOf<KeyedResiliencePipelineProvider<OpenGenericPipelineKeyPair2<string>, string, int>>()
    );
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey1, out _), Is.False);
    Assert.That(pipelineProviderOfKeyPair2.TryGetPipeline(PipelineKey2, out _), Is.True);

    // get any ResiliencePipelineProvider<int> with NullServiceKey
    Assert.That(
      provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: NullServiceKey),
      Is.SameAs(pipelineProviderOfKeyPair1).Or.SameAs(pipelineProviderOfKeyPair2)
    );
  }

  private record struct GenericTypeDefPipelineKeyPair<TServiceKey, TPipelineKey>(TServiceKey ServiceKey, TPipelineKey PipelineKey)
    : IResiliencePipelineKeyPair<TServiceKey, TPipelineKey>
    where TPipelineKey : notnull
  { }

  [Test]
  public void GetKeyedResiliencePipelineProvider_GenericTypeDefKeyPairType()
  {
    var services = new ServiceCollection();

    const string CommonServiceKey = nameof(CommonServiceKey);
    const string OtherServiceKey = nameof(OtherServiceKey);
    const int PipelineKey1 = 1;
    const int PipelineKey2 = 2;
    const int PipelineKey3 = 3;

    services.AddResiliencePipeline<GenericTypeDefPipelineKeyPair<string, int>, string, int>(
      resiliencePipelineKeyPair: new GenericTypeDefPipelineKeyPair<string, int>(CommonServiceKey, PipelineKey1),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<GenericTypeDefPipelineKeyPair<string, int>, string, int>(
      resiliencePipelineKeyPair: new GenericTypeDefPipelineKeyPair<string, int>(OtherServiceKey, PipelineKey2),
      configure: (builder, context) => { }
    );
    services.AddResiliencePipeline<GenericTypeDefPipelineKeyPair<string, int>, string, int>(
      resiliencePipelineKeyPair: new GenericTypeDefPipelineKeyPair<string, int>(OtherServiceKey, PipelineKey3),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    // get KeyedResiliencePipelineProvider<GenericTypeDefPipelineKeyPair<string, int>, string, int> with CommonServiceKey
    var pipelineProviderForCommonServiceKey = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: CommonServiceKey,
      typeOfKeyPair: typeof(GenericTypeDefPipelineKeyPair<string, int>)
    );

    Assert.That(pipelineProviderForCommonServiceKey, Is.Not.Null);
    Assert.That(
      pipelineProviderForCommonServiceKey,
      Is.TypeOf<KeyedResiliencePipelineProvider<GenericTypeDefPipelineKeyPair<string, int>, string, int>>()
    );
    Assert.That(pipelineProviderForCommonServiceKey.TryGetPipeline(PipelineKey1, out _), Is.True);
    Assert.That(pipelineProviderForCommonServiceKey.TryGetPipeline(PipelineKey2, out _), Is.False);
    Assert.That(pipelineProviderForCommonServiceKey.TryGetPipeline(PipelineKey3, out _), Is.False);

    // get KeyedResiliencePipelineProvider<GenericTypeDefPipelineKeyPair<string, int>, string, int> with OtherServiceKey
    var pipelineProviderForOtherServiceKey = provider.GetKeyedResiliencePipelineProvider<int>(
      serviceKey: OtherServiceKey,
      typeOfKeyPair: typeof(GenericTypeDefPipelineKeyPair<string, int>)
    );

    Assert.That(pipelineProviderForOtherServiceKey, Is.Not.Null);
    Assert.That(
      pipelineProviderForOtherServiceKey,
      Is.TypeOf<KeyedResiliencePipelineProvider<GenericTypeDefPipelineKeyPair<string, int>, string, int>>()
    );
    Assert.That(pipelineProviderForOtherServiceKey.TryGetPipeline(PipelineKey1, out _), Is.False);
    Assert.That(pipelineProviderForOtherServiceKey.TryGetPipeline(PipelineKey2, out _), Is.True);
    Assert.That(pipelineProviderForOtherServiceKey.TryGetPipeline(PipelineKey3, out _), Is.True);

    // get any ResiliencePipelineProvider<int> with CommonServiceKey
    Assert.That(
      provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: CommonServiceKey),
      Is.SameAs(pipelineProviderForCommonServiceKey)
    );

    // get any ResiliencePipelineProvider<int> with OtherServiceKey
    Assert.That(
      provider.GetRequiredKeyedService<ResiliencePipelineProvider<int>>(serviceKey: OtherServiceKey),
      Is.SameAs(pipelineProviderForOtherServiceKey)
    );
  }

  [Test]
  public void GetKeyedResiliencePipelineProvider_OpenGenericKeyPairType_TypeOfPipelineKeyDoesNotMatch()
  {
    const string ServiceKey = nameof(ServiceKey);
    const int PipelineKey = 1;

    var services = new ServiceCollection();

    services.AddResiliencePipeline<OpenGenericPipelineKeyPair1<string>, string, int>(
      resiliencePipelineKeyPair: new OpenGenericPipelineKeyPair1<string>(ServiceKey, PipelineKey),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    // specify double for TPipelineKey instead of int
    Assert.That(
      () => provider.GetKeyedResiliencePipelineProvider<double>(
        serviceKey: ServiceKey,
        typeOfKeyPair: typeof(OpenGenericPipelineKeyPair1<>)
      ),
      Throws.InvalidOperationException
    );
  }

  [Test]
  public void GetKeyedResiliencePipelineProvider_GenericTypeDefKeyPairType_TypeOfPipelineKeyIsNotConstructedType()
  {
    const string ServiceKey = nameof(ServiceKey);
    const int PipelineKey = 1;

    var services = new ServiceCollection();

    services.AddResiliencePipeline<GenericTypeDefPipelineKeyPair<string, int>, string, int>(
      resiliencePipelineKeyPair: new GenericTypeDefPipelineKeyPair<string, int>(ServiceKey, PipelineKey),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    Assert.That(
      () => provider.GetKeyedResiliencePipelineProvider<int>(
        serviceKey: ServiceKey,
        typeOfKeyPair: typeof(GenericTypeDefPipelineKeyPair<,>)
      ),
      Throws.InvalidOperationException
    );
  }

  [Test]
  public void GetKeyedResiliencePipelineProvider_TypeOServiceKeyDoesNotMatch()
  {
    const string StringServiceKey = "0";
    const double DoubleServiceKey = 0;
    const int PipelineKey = 1;

    var services = new ServiceCollection();

    services.AddResiliencePipeline<PipelineKeyPair1, string, int>(
      resiliencePipelineKeyPair: new PipelineKeyPair1(StringServiceKey, PipelineKey),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    Assert.That(
      () => provider.GetKeyedResiliencePipelineProvider<int>(
        serviceKey: DoubleServiceKey,
        typeOfKeyPair: typeof(PipelineKeyPair1)
      ),
      Is.Null
    );
  }

  [Test]
  public void GetKeyedResiliencePipelineProvider_OpenGenericKeyPairType_TypeOServiceKeyDoesNotMatch()
  {
    const string StringServiceKey = "0";
    const double DoubleServiceKey = 0;
    const int PipelineKey = 1;

    var services = new ServiceCollection();

    services.AddResiliencePipeline<OpenGenericPipelineKeyPair1<string>, string, int>(
      resiliencePipelineKeyPair: new OpenGenericPipelineKeyPair1<string>(StringServiceKey, PipelineKey),
      configure: (builder, context) => { }
    );

    var provider = services.BuildServiceProvider();

    Assert.That(
      () => provider.GetKeyedResiliencePipelineProvider<int>(
        serviceKey: DoubleServiceKey,
        typeOfKeyPair: typeof(OpenGenericPipelineKeyPair1<>)
      ),
      Is.Null
    );
  }
}
