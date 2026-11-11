namespace CafeStockAI.Mobile;
internal static class MobileEndpoints
{
    public static void MapMobileEndpoints(this WebApplication app, bool demo)
    {
        var commands = app.MapGroup("");
        commands.AddEndpointFilter(async (context, next) =>
        {
            var gate = context.HttpContext.RequestServices.GetRequiredService<SemaphoreSlim>();
            await gate.WaitAsync(context.HttpContext.RequestAborted);
            try
            {
                return await next(context);
            }
            finally
            {
                gate.Release();
            }
        });
        app.MapAuthenticationEndpoints(commands, demo);
        app.MapWorkspaceEndpoints();
        app.MapCountsEndpoints(commands);
        app.MapManagementEndpoints(commands);
    }
}
