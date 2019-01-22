using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebSocketHelper
{
    public class BwsJsInterop
    {
        public static Task<string> Alert(string message)
        {
            return JSRuntime.Current.InvokeAsync<string>(
                "BwsJsFunctions.alert",
                message);
        }

        public static Task<string> Prompt(string message)
        {
            return JSRuntime.Current.InvokeAsync<string>(
                "BwsJsFunctions.showPrompt",
                message);
        }

        public static Task<bool> WsAdd(string WsID, string WsUrl, string WsTransportType, DotNetObjectRef dotnethelper)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsFunctions.WsAdd", new { WsID, WsUrl, WsTransportType, dotnethelper });
        }


        public static Task<bool> WsSend(string WsID, string WsMessage)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsFunctions.WsSend", new { WsID, WsMessage });
        }

        public static Task<bool> WsSend(string WsID, byte[] WsMessage)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsFunctions.WsSendBinary", new { WsID, WsMessage });
        }

        public static Task<bool> WsClose(string WsID)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsFunctions.WsClose", WsID);
        }

        public static Task<bool> WsRemove(string WsID)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsFunctions.WsRemove", WsID);
        }


        public static Task<short> WsGetStatus(string WsID)
        {
            return JSRuntime.Current.InvokeAsync<short>("BwsJsFunctions.WsGetStatus", WsID);
        }

    }
}
