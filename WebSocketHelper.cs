using BlazorWebSocketHelper.Classes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorWebSocketHelper.Classes.BwsEnums;

namespace BlazorWebSocketHelper
{
    public class WebSocketHelper : IDisposable
    {

        public BwsState bwsState = BwsState.Undefined;

        public bool IsDisposed = false;

        public Action<short> OnStateChange { get; set; }

        public Action<string> OnMessage { get; set; }

        public Action<string> OnError { get; set; }


        public List<BwsMessage> Log = new List<BwsMessage>();


        private string _id = BwsFunctions.Cmd_Get_UniqueID();

        private string _url = string.Empty;

        public List<BwsError> BwsError = new List<BwsError>();



        public async Task<string> get_WsStatus()
        {
            
            short a = await BwsJsInterop.WsGetStatus(_id);

            return BwsFunctions.ConvertStatus(a).ToString();
            
        }

        public WebSocketHelper(string Par_URL)
        {
            _initialize(Par_URL);
        }

        public void Connect(string Par_URL)
        {
            _initialize(Par_URL);
        }



        private void _initialize(string Par_URL)
        {
            if (!string.IsNullOrEmpty(Par_URL))
            {
                _url = Par_URL;
                _connect();
            }
            else
            {
                BwsError.Add(new BwsError { Message = "Url is not provided!", Description = string.Empty });
            }
        }

        private void _connect()
        {
            BwsJsInterop.WsAdd(_id, _url, new DotNetObjectRef(this));
        }

        public void send(string Par_Message)
        {
            if (!string.IsNullOrEmpty(Par_Message))
            {
                BwsJsInterop.WsSend(_id, Par_Message);
                Log.Add(new BwsMessage { ID = Log.Count + 1, Date = DateTime.Now, Message = Par_Message, MessageType = BwsMessageType.send });
            }
        }


        [JSInvokable]
        public void InvokeStateChanged(short par_state)
        {
            bwsState = BwsFunctions.ConvertStatus(par_state);
            OnStateChange?.Invoke(par_state);
        }


        [JSInvokable]
        public void InvokeOnError(string par_error)
        {
            OnError?.Invoke(par_error);
        }


        [JSInvokable]
        public void InvokeOnMessage(string par_message)
        {
            Log.Add(new BwsMessage { ID = Log.Count + 1, Date = DateTime.Now, Message = par_message, MessageType = BwsMessageType.received });

            OnMessage?.Invoke(par_message);
        }

        public void Close()
        {
            Log = new List<BwsMessage>();
            BwsJsInterop.WsClose(_id);
        }

        public void Dispose()
        {
            Log = new List<BwsMessage>();
            BwsJsInterop.WsRemove(_id);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }




    }
}
