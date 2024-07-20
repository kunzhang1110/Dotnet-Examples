
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using System.Text;

namespace mvc_example.Controllers
{
    public class SocketController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Connect()
        {
            var buffer = new byte[1024 * 4];
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync(); // Accept the WebSocket connection
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                
                while (!result.CloseStatus.HasValue)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var serverMsg = Encoding.UTF8.GetBytes($"Server: {message}");
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                        result.MessageType,
                        result.EndOfMessage,
                        CancellationToken.None);

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }

            return Content("WebSocket Connected");
        }

    }
}
