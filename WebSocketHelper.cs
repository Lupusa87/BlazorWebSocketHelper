using BlazorWebSocketHelper.Classes;
using BlazorWindowHelper;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static BlazorWebSocketHelper.Classes.BwsEnums;

namespace BlazorWebSocketHelper
{
    public class WebSocketHelper : IDisposable
    {

        public BwsState bwsState = BwsState.Undefined;


        public BwsTransportType bwsTransportType { get; private set; } = BwsTransportType.Text;

        public bool IsDisposed = false;

        public Action<short> OnStateChange { get; set; }

        public Action<BwsMessage> OnMessage { get; set; }

        public Action<string> OnError { get; set; }


        public List<BwsMessage> Log = new List<BwsMessage>();
        public bool DoLog { get; set; } = true;
        public int LogMaxCount { get; set; } = 100;

        private string _id = BwsFunctions.Cmd_Get_UniqueID();

        private string _url = string.Empty;

        public List<BwsError> BwsError = new List<BwsError>();

        private byte[] buffer;

        public async Task<string> get_WsStatus()
        {
            
            short a = await BwsJsInterop.WsGetStatus(_id);

            return BwsFunctions.ConvertStatus(a).ToString();
            
        }

        public WebSocketHelper(string Par_URL, BwsTransportType Par_TransportType)
        {
            _initialize(Par_URL,Par_TransportType);
        }

        public void Connect(string Par_URL, BwsTransportType Par_TransportType)
        {
            _initialize(Par_URL, Par_TransportType);
        }



        private void _initialize(string Par_URL, BwsTransportType Par_TransportType)
        {
            if (!string.IsNullOrEmpty(Par_URL))
            {
                StaticClass.webSocketHelper = this;
                _url = Par_URL;
                bwsTransportType = Par_TransportType;
                _connect();
            }
            else
            {
                BwsError.Add(new BwsError { Message = "Url is not provided!", Description = string.Empty });
            }
        }

        private void _connect()
        {
            BwsJsInterop.WsAdd(_id, _url, bwsTransportType.ToString(), new DotNetObjectRef(this));
            _setTransportType();
        }


        private int GetNewIDFromLog()
        {

            if (Log.Any())
            {
                return Log.Max(x => x.ID) + 1;
            }
            else
            {
                return 1;
            }
        }

        public void send(string Par_Message)
        {
            if (!string.IsNullOrEmpty(Par_Message))
            {

                BwsJsInterop.WsSend(_id, Par_Message);

                if (DoLog)
                {
                    
                    Log.Add(new BwsMessage { ID = GetNewIDFromLog(),
                                             Date = DateTime.Now,
                                             Message = Par_Message,
                                             MessageType = BwsMessageType.send,
                                             TransportType = bwsTransportType});
                    if (Log.Count > LogMaxCount)
                    {
                        Log.RemoveAt(0);
                    }
                }
              
            }
        }


        public string send(byte[] Par_Message)
        {
            string result = string.Empty;

            if (Par_Message.Length>0)
            {

                result = BwsJsInterop.WsSend(_id, Par_Message);


                if (DoLog)
                {

                    Log.Add(new BwsMessage { ID = GetNewIDFromLog(),
                                             Date = DateTime.Now,
                                             MessageBinary = Par_Message,
                                             MessageType = BwsMessageType.send,
                                             TransportType = bwsTransportType });
                    if (Log.Count > LogMaxCount)
                    {
                        Log.RemoveAt(0);
                    }
                }

            }

            return result;
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

            BwsMessage b = new BwsMessage
            {
                ID = GetNewIDFromLog(),
                Date = DateTime.Now,
                Message = par_message,
                MessageType = BwsMessageType.received,
                TransportType = bwsTransportType
            };

            if (DoLog)
            {
                
                Log.Add(b);
                
                if (Log.Count > LogMaxCount)
                {
                    Log.RemoveAt(0);
                }
            }

            
            OnMessage?.Invoke(b);

        }



        public void InvokeOnMessageBinary(byte[] data, string par_str, string par_binaryVisual)
        {
            BwsMessage b = new BwsMessage
            {
                ID = GetNewIDFromLog(),
                Date = DateTime.Now,
                Message = par_str,
                MessageBinary = data,
                MessageBinaryVisual = par_binaryVisual,//string.Join(", ", data),
                MessageType = BwsMessageType.received,
                TransportType = bwsTransportType
            };

            if (DoLog)
            {

                Log.Add(b);

                if (Log.Count > LogMaxCount)
                {
                    Log.RemoveAt(0);
                }
            }


            OnMessage?.Invoke(b);
        }

        public void SetTransportType(BwsTransportType par_bwsTransportType)
        {
            if (bwsTransportType != par_bwsTransportType)
            {
                bwsTransportType = par_bwsTransportType;

                _setTransportType();
            }
        }

        private void _setTransportType()
        {

                switch (bwsTransportType)
                {
                    case BwsTransportType.Text:
                      //  BwsJsInterop.WsSetBinaryType(_id, "null");
                        break;
                    case BwsTransportType.ArrayBuffer:
                        BwsJsInterop.WsSetBinaryType(_id, "arraybuffer");
                        break;
                    case BwsTransportType.Blob:
                        BwsJsInterop.WsSetBinaryType(_id, "blob");
                        break;
                    default:
                        break;
                }
            
        }


            public void Close()
        {
            if (DoLog)
            {
                Log = new List<BwsMessage>();
            }
            BwsJsInterop.WsClose(_id);
        }

        public void Dispose()
        {
            if (DoLog)
            {
                Log = new List<BwsMessage>();
            }
            BwsJsInterop.WsRemove(_id);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }




    }
}
