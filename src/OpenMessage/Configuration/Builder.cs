using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenMessage.Configuration
{
    public abstract class Builder : IBuilder
    {
        public IMessagingBuilder HostBuilder { get; }
        public string ConsumerId { get; } = Guid.NewGuid().ToString("N");

        public Builder(IMessagingBuilder hostBuilder)
        {
            HostBuilder = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));
        }


        protected void ConfigureOptions<T>(Action<T> configurator)
            where T :class
        {
            if (configurator == null)
                return;

            HostBuilder.Services.Configure<T>(ConsumerId, configurator);
        }

        protected void ConfigureOptions<T>(Action<HostBuilderContext, T> configurator)
            where T :class
        {
            if (configurator == null)
                return;

            HostBuilder.Services.Configure<T>(ConsumerId, options => configurator(HostBuilder.Context, options));
        }

        public abstract void Build();
    }
}