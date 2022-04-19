using Microsoft.AspNetCore.SignalR;

namespace Pdf2Epub.API.Hubs
{
    public class MessageHub : Hub
    {
        public delegate void MessageHandler(string message);
        MessageHandler? message_handler = null;

        public async Task DistributeWorkerMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveWorkerMessage", message);
        }

        public async Task DistributeFrontendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveFrontendMessage", message);
        }

        public void HandleMessage(string message)
        {
            message_handler?.Invoke(message);
        }
    }
}
