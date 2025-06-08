using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.Registry;
using Polly.Registry.KeyedRegistry;

var services = new ServiceCollection();

// Add ResiliencePipeline with service key '1', pipeline key "pipeline1"
services.AddResiliencePipeline<ResiliencePipelineKeyPair, int, string>(
  resiliencePipelineKeyPair: new(ServiceKey: 1, PipelineKey: "pipeline1"),
  configure: static (builder, context) => {
    var (serviceKey, pipelineKey) = (context.PipelineKey.ServiceKey, context.PipelineKey.PipelineKey);
    Console.WriteLine($"Add ResiliencePipeline: ServiceKey={serviceKey}, PipelineKey={pipelineKey}");
    // Add ResiliencePipeline: ServiceKey=1, PipelineKey=pipeline1

    builder.AddTimeout(TimeSpan.FromSeconds(1));
  }
);

// Add ResiliencePipeline with service key '1', pipeline key "pipeline2"
services.AddResiliencePipeline<ResiliencePipelineKeyPair, int, string>(
  resiliencePipelineKeyPair: new(ServiceKey: 1, PipelineKey: "pipeline2"),
  configure: (builder, context) => {
    var (serviceKey, pipelineKey) = (context.PipelineKey.ServiceKey, context.PipelineKey.PipelineKey);
    Console.WriteLine($"Add ResiliencePipeline: ServiceKey={serviceKey}, PipelineKey={pipelineKey}");
    // Add ResiliencePipeline: ServiceKey=1, PipelineKey=pipeline2

    builder.AddRetry(new());
  }
);

// Add ResiliencePipeline with service key '2', pipeline key "pipeline3"
services.AddResiliencePipeline<ResiliencePipelineKeyPair, int, string>(
  resiliencePipelineKeyPair: new(ServiceKey: 2, PipelineKey: "pipeline3"),
  configure: (builder, context) => {
    var (serviceKey, pipelineKey) = (context.PipelineKey.ServiceKey, context.PipelineKey.PipelineKey);
    Console.WriteLine($"Add ResiliencePipeline: ServiceKey={serviceKey}, PipelineKey={pipelineKey}");
    // Add ResiliencePipeline: ServiceKey=2, PipelineKey=pipeline3

    builder.AddCircuitBreaker(new());
  }
);

var serviceProvider = services.BuildServiceProvider();

// Get the ResiliencePipelineProvider registered with the service key '1'
var pipelineProvider1 = serviceProvider.GetRequiredKeyedService<ResiliencePipelineProvider<string>>(serviceKey: 1);

var pipeline1 = pipelineProvider1.GetPipeline("pipeline1");
var pipeline2 = pipelineProvider1.GetPipeline("pipeline2");

// Get the ResiliencePipelineProvider registered with the service key '2'
var pipelineProvider2 = serviceProvider.GetRequiredKeyedService<ResiliencePipelineProvider<string>>(serviceKey: 2);

var pipeline3 = pipelineProvider2.GetPipeline("pipeline3");

// The key type used to register ResiliencePipeline and ResiliencePipelineProvider
record struct ResiliencePipelineKeyPair(int ServiceKey, string PipelineKey) : IResiliencePipelineKeyPair<int, string> { }
