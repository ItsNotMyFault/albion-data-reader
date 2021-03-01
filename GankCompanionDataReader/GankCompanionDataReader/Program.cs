using GankCompanionDataReader.eventHandler;
using GankCompanionDataReader.infrastructure;
using GankCompanionDataReader.packethandler;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GankCompanionDataReader
{
    public class Program
    {

        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection().AddSingleton<IPartyRepository, InMemoryPartyRepository>().BuildServiceProvider();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var endpointURL = Environment.GetEnvironmentVariable("ENDPOINT_URL");
            PacketReader gg = new PacketReader(serviceProvider.GetService<IPartyRepository>());
            gg.Start(endpointURL);
        }

    }
}
