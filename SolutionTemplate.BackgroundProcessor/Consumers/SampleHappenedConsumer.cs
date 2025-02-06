using MassTransit;
using SolutionTemplate.Domain.Events;

namespace SolutionTemplate.BackgroundProcessor.Consumers
{
    internal class SampleHappenedConsumer : IConsumer<SampleHappened>
    {
        private readonly ILogger<SampleHappenedConsumer> _logger;

        public SampleHappenedConsumer(ILogger<SampleHappenedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SampleHappened> context)
        {
            _logger.LogInformation("Received sample event {id}", context.Message.Id);
            return Task.CompletedTask;
        }
    }
}
