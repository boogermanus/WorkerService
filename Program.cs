using WorkerService;

var builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<MonitorLoop>();
builder.Services.AddHostedService<QueueHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue>(_ => {return new DefaultBackgroundTaskQueue(10);});

var host = builder.Build();
MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
monitorLoop.Start();
host.Run();
