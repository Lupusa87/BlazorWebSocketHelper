using Microsoft.JSInterop;
using Mono.WebAssembly.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorWebSocketHelper
{
    public static class StaticClass
    {
        public static List<WebSocketHelper> webSocketHelpers_List = new List<WebSocketHelper>();

        public static MonoWebAssemblyJSRuntime monoWebAssemblyJSRuntime = new MonoWebAssemblyJSRuntime();

        //1/21/2019 after suchiman's recomendation solved problem how to get byte array from js in dotnet
        //https://github.com/aspnet/AspNetCore/blob/master/src/Components/Blazor/Blazor/src/Http/WebAssemblyHttpMessageHandler.cs
        //https://github.com/aspnet/AspNetCore/blob/master/src/Components/Browser.JS/src/Services/Http.ts
        //https://github.com/aspnet/AspNetCore/blob/master/src/Components/Browser.JS/src/Platform/Mono/MonoPlatform.ts


        //1/23/2019 mono unmarshaled fixed fast transfer from  dotnet to js 

        //test websocket server
        //http://demos.kaazing.com/echo/
        //https://www.websocket.org/echo.html
        [JSInvokable]
        public static void AllocateArray(int length, string a)
        {
           
            byte[] b = new byte[length];

            monoWebAssemblyJSRuntime.InvokeUnmarshalled<byte[], bool>("BwsJsFunctions.GetBinaryData", b);

            HandleMessageBinary(b, a);

        }


        [JSInvokable]
        public static void HandleMessageBinary(byte[] par_message, string wsID)
        {
           
            if (webSocketHelpers_List.Any())
            {
                if (webSocketHelpers_List.Any(x => x._id.Equals(wsID, StringComparison.InvariantCultureIgnoreCase)))
                {
                    webSocketHelpers_List.Single(x => x._id.Equals(wsID, StringComparison.InvariantCultureIgnoreCase)).InvokeOnMessageBinary(par_message);
                }
            }
        }


       
    }


}
