using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, l) =>
        l.ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .CreateLogger());
    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment()) {
        app.UseMigrationsEndPoint();
        app.UseWebAssemblyDebugging();
    } else {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    
    app.UseHttpsRedirection();

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();
    app.MapGet("/", () => "Hello World!");

    await app.RunAsync();
    return 0;
}
catch (Exception e) {
    Log.Fatal(e, "Error in webapp run");
    return e.HResult;
}
finally {
    await Log.CloseAndFlushAsync();
}