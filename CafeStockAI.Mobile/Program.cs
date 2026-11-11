using CafeStockAI.Mobile;

var builder = WebApplication.CreateBuilder(args);
bool demo = args.Contains("--demo") || builder.Configuration["CAFESTOCK_DEMO"] == "1";
builder.AddMobileServices(demo);
var app = builder.Build();
app.UseMobilePipeline();
app.MapMobileEndpoints(demo);
app.Run();
