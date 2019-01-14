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
                "BwsJsInterop.alert",
                message);
        }

        public static Task<string> Prompt(string message)
        {
            return JSRuntime.Current.InvokeAsync<string>(
                "BwsJsInterop.showPrompt",
                message);
        }

        public static Task<bool> WsAdd(string WsID, string WsUrl, DotNetObjectRef dotnethelper)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsInterop.WsAdd", new { WsID, WsUrl, dotnethelper });
        }


        public static Task<bool> WsSend(string WsID, string WsMessage)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsInterop.WsSend", new { WsID, WsMessage });
        }

        public static Task<bool> WsClose(string WsID)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsInterop.WsClose", WsID);
        }

        public static Task<bool> WsRemove(string WsID)
        {
            return JSRuntime.Current.InvokeAsync<bool>("BwsJsInterop.WsRemove", WsID);
        }


        public static Task<short> WsGetStatus(string WsID)
        {
            return JSRuntime.Current.InvokeAsync<short>("BwsJsInterop.WsGetStatus", WsID);
        }

    }
}
