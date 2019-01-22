var WebSockets_array = [];

function WsOnOpen(e, dotnethelper) {
    dotnethelper.invokeMethodAsync('InvokeStateChanged', 1);
}

function WsOnClose(e, dotnethelper) {
    dotnethelper.invokeMethodAsync('InvokeStateChanged', 3);
}

function WsOnError(e, dotnethelper) {
    console.log("invoked WsOnError - " + e.message);
    dotnethelper.invokeMethodAsync('InvokeOnError', "Connection Error!");
}


function WsOnMessage(e, dotnethelper) {

    if (e.data instanceof ArrayBuffer) {
        //console.log("js invoked WsOnMessage - arraybuffer");
        var allocateArrayMethod = Blazor.platform.findMethod(
            'BlazorWebSocketHelper',
            'BlazorWebSocketHelper',
            'StaticClass',
            'AllocateArray'
        );


        var dotNetArray = Blazor.platform.callMethod(allocateArrayMethod, null, [Blazor.platform.toDotNetString(e.data.byteLength.toString())]);
        
        var arr = Blazor.platform.toUint8Array(dotNetArray);

        arr.set(new Uint8Array(e.data));

        var receiveResponseMethod = Blazor.platform.findMethod(
            'BlazorWebSocketHelper',
            'BlazorWebSocketHelper',
            'StaticClass',
            'HandleMessageBinary'
        );

        Blazor.platform.callMethod(receiveResponseMethod, null, [dotNetArray]);

    }
    else {
        //console.log("js invoked WsOnMessage - text");
        dotnethelper.invokeMethodAsync('InvokeOnMessage', e.data);
    }

}

window.BwsJsFunctions = {
    alert: function (message) {
        return alert(message);
    },
    showPrompt: function (message) {
        return prompt(message, 'Type anything here');
    },
    WsAdd: function (obj) {
      
        var b = {
            id: obj.wsID,
            ws: new WebSocket(obj.wsUrl)
        };

        obj.dotnethelper.invokeMethodAsync('InvokeStateChanged', 0);

        b.ws.onopen = function (e) { WsOnOpen(e, obj.dotnethelper); };

        b.ws.onclose = function (e) { WsOnClose(e, obj.dotnethelper); };
        b.ws.onerror = function (e) { WsOnError(e, obj.dotnethelper); };
        b.ws.onmessage = function (e) { WsOnMessage(e, obj.dotnethelper); };
        WebSockets_array.push(b);


        return true;
    },
    WsSetBinaryType: function (obj) {
        var result = true;
        var index = WebSockets_array.findIndex(x => x.id === obj.wsID);

        if (index > -1) {
            WebSockets_array[index].ws.binaryType = obj.wsBinaryType;
            //console.log("js invoked WsSetBinaryType - " + obj.wsBinaryType);
        }
        else {
            result = false;
        }

        return result;
    },
    WsClose: function (WsID) {
        var result = true;
        var index = WebSockets_array.findIndex(x => x.id === WsID);

        if (index > -1) {
            WebSockets_array[index].ws.close(); 
        }
        else {
            result = false;
        }

        return result;
    },
    WsRemove: function (WsID) {
        var result = true;
        var index = WebSockets_array.findIndex(x => x.id === WsID);

        if (index > -1) {
            WebSockets_array.splice(index, 1);
        }
        else {
            result = false;
        }

        return result;
    },
    WsSend: function (obj) {
        var result = false;

        var index = WebSockets_array.findIndex(x => x.id === obj.wsID);

        if (index > -1) {

            WebSockets_array[index].ws.send(obj.wsMessage);
            result = true;
            
        }

        return result;
    },
    WsSendBinary: function (obj) {
        var result = false;

        var index = WebSockets_array.findIndex(x => x.id === obj.wsID);
     
        if (index > -1) {
    
            var dataLen = obj.wsMessage.length;

            var bytearray = new Uint8Array(dataLen);
            bytearray.set(obj.wsMessage);

            WebSockets_array[index].ws.send(bytearray.buffer);

            result = true;

        }

        return result;
    },
    WsGetStatus: function (WsID) {

        var result = -1;

        var index = WebSockets_array.findIndex(x => x.id === WsID);

        if (index > -1) {
            result = WebSockets_array[index].ws.readyState;
        }
       
        return result;
    },
};
