using Microsoft.JSInterop;
using Mono.WebAssembly.Interop;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorWebSocketHelper
{
    public class BwsJsInterop
    {
        private IJSRuntime _JSRuntime;
        public BwsJsInterop(IJSRuntime jsRuntime) => _JSRuntime = jsRuntime;

        public Task<string> Alert(string message)
        {
            return _JSRuntime.InvokeAsync<string>(
                "BwsJsFunctions.alert",
                message);
        }

        public Task<string> Prompt(string message)
        {
            return _JSRuntime.InvokeAsync<string>(
                "BwsJsFunctions.showPrompt",
                message);
        }

        public Task<bool> WsAdd(string WsID, string WsUrl, string WsTransportType, DotNetObjectRef dotnethelper)
        {
            return _JSRuntime.InvokeAsync<bool>("BwsJsFunctions.WsAdd", new { WsID, WsUrl, WsTransportType, dotnethelper });
        }


        public Task<bool> WsSend(string WsID, string WsMessage)
        {
            return _JSRuntime.InvokeAsync<bool>("BwsJsFunctions.WsSend", new { WsID, WsMessage});
        }

        public bool WsSend(string WsID, byte[] WsMessage)
        {
            if (_JSRuntime is MonoWebAssemblyJSRuntime mono)
            {
                return mono.InvokeUnmarshalled<string, byte[], bool>(
                    "BwsJsFunctions.WsSendBinary",
                    WsID,
                    WsMessage);
            }

            return false;
        }

        public Task<bool> WsSetBinaryType(string WsID, string WsBinaryType)
        {
            return _JSRuntime.InvokeAsync<bool>("BwsJsFunctions.WsSetBinaryType", new { WsID, WsBinaryType });
        }

        public Task<bool> WsClose(string WsID)
        {
            return _JSRuntime.InvokeAsync<bool>("BwsJsFunctions.WsClose", WsID);
        }

        public Task<bool> WsRemove(string WsID)
        {
            return _JSRuntime.InvokeAsync<bool>("BwsJsFunctions.WsRemove", WsID);
        }


        public Task<short> WsGetStatus(string WsID)
        {
            return _JSRuntime.InvokeAsync<short>("BwsJsFunctions.WsGetStatus", WsID);
        }


    }
}
