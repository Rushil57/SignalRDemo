using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRDemo
{
    public class EmployeeHub : Hub
    {
        [HubMethodName("NotifyClients")]
        public static void NotifyCurrentEmployeeInformationToAllClients()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<EmployeeHub>();

            // the update client method will update the connected client about any recent changes in the server data
            context.Clients.All.updatedClients();
        }
    }
}