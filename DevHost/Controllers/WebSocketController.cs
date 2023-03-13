using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using MonacoRoslynCompletionProvider;
using MonacoRoslynCompletionProvider.Api;
using System.Text.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevHost.Controllers;

[ApiController]
// [Route("[controller]")]
public class WebSocketController : ControllerBase
{
    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }
        else 
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        string result = System.Text.Encoding.UTF8.GetString(buffer);
        result = result.Trim('\0');
        var codeCheckRequest = JsonSerializer.Deserialize<CodeCheckRequest>(result);
        var codeCheckResults = await CompletitionRequestHandler.Handle(codeCheckRequest);
        var ms = new MemoryStream();
        JsonSerializer.Serialize<CodeCheckResult[]>(ms, codeCheckResults);
        var resultBingArr = ms.ToArray();
        while(!receiveResult.CloseStatus.HasValue)
        {
            await webSocket.SendAsync(
                // new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                new ArraySegment<byte>(resultBingArr),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);
            receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
        
    }
}