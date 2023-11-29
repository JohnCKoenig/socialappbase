namespace MobileAppAPI.RTS
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using MobileAppAPI.RTS.Chat;

    public static class HubRouteBuilder
    {
        public static string RTSRoute = "rts";
        public static void MapHubs(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<DirectMessageService>($"{RTSRoute}/dm");
        }
    }
}
