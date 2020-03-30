using System;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

class Client : MonoBehaviour
{
    public Uri ServerUri { get; set; }

    public Client(Uri serverUri)
    {
        ServerUri = serverUri;
    }

    public void Listen()
    {
        print("It works");
        using (var ws = new WebSocket(ServerUri.ToString()))
        {
            ws.OnOpen += (sender, e) =>
            {
                string message = @"{
A1: 1,
A2: 1,
A3: 1,
A4: 1,
AB1: 1,
AB2: 1,
B1: 1,
B2: 1,
B3: 1,
B4: 1,
B5: 1,
BB1: 1,
C1: 1,
C2: 1,
C3: 1,
D1: 1,
D2: 1,
D3: 1,
E1: 1,
E2: 1,
EV1: 1,
EV2: 1,
EV3: 1,
EV4: 1,
FF1: 1,
FF2: 1,
FV1: 1,
FV2: 1,
FV3: 1,
FV4: 1,
GF1: 1,
GF2: 1,
GV1: 1,
GV2: 1,
GV3: 1,
GV4: 1,
}";
                ws.Send(message);
            };

            ws.OnMessage += (sender, e) =>
            {
                print(e.Data);
                string message = @"{
A1: 1,
A2: 1,
A3: 1,
A4: 1,
AB1: 1,
AB2: 1,
B1: 1,
B2: 1,
B3: 1,
B4: 1,
BB1: 1,
C1: 1,
C2: 1,
C3: 1,
D1: 1,
D2: 1,
D3: 1,
E1: 1,
E2: 1,
EV1: 1,
EV2: 1,
EV3: 1,
EV4: 1,
FF1: 1,
FF2: 1,
FV1: 1,
FV2: 1,
FV3: 1,
FV4: 1,
GF1: 1,
GF2: 1,
GV1: 1,
GV2: 1,
GV3: 1,
GV4: 1,
}";
                ws.Send(message);
            };

            ws.Connect();
        }
    }
}
