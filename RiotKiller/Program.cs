using RiotKiller;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseWindowsService(o => o.ServiceName = "RiotKillerService")
    .Build();

await host.RunAsync();
