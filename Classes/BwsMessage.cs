using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorWebSocketHelper.Classes.BwsEnums;

namespace BlazorWebSocketHelper.Classes
{
    public class BwsMessage
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public byte[] MessageBinary { get; set; }
        public BwsMessageType MessageType { get; set; }
    }
}
