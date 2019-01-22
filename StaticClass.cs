using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebSocketHelper
{
    public static class StaticClass
    {
        public static WebSocketHelper webSocketHelper = null;
        //1/21/2019 after suchiman's recomendation solved problem how to get byte array from js in dotnet
        //https://github.com/aspnet/AspNetCore/blob/master/src/Components/Blazor/Blazor/src/Http/WebAssemblyHttpMessageHandler.cs
        //https://github.com/aspnet/AspNetCore/blob/master/src/Components/Browser.JS/src/Services/Http.ts
        //https://github.com/aspnet/AspNetCore/blob/master/src/Components/Browser.JS/src/Platform/Mono/MonoPlatform.ts

        //test websocket server
        //http://demos.kaazing.com/echo/
        //https://www.websocket.org/echo.html
        [JSInvokable]
        public static byte[] AllocateArray(string length)
        {
            return new byte[int.Parse(length)];
        }


        [JSInvokable]
        public static void HandleMessageBinary(byte[] par_message)
        {
            webSocketHelper.InvokeOnMessageBinary(par_message);
        }
        }
}
