using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.BrowserConsole()
    .CreateLogger();

try {
    WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

    builder.Services.AddApiAuthorization();
    builder.Logging.ClearProviders()
        .AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.BrowserConsole()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger(), true);

    builder.Services.AddSingleton<HubConnection>(sp => {
        NavigationManager navigationManager = sp.GetRequiredService<NavigationManager>();
        return new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/gamehub"))
            .WithAutomaticReconnect()
            .Build();
    });
    
    await builder.Build().RunAsync();
    return 0;
}
catch (Exception e) {
    Console.WriteLine(e);
    return e.HResult;
}
finally {
    await Log.CloseAndFlushAsync();
}