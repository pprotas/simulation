using System.IO;
using UnityEngine;
using WebSocketSharp;

class Client : MonoBehaviour
{
    [SerializeField]
    private string webSocketUrl = "ws://localhost:8080";
    public ControllerData ControllerData { get; set; }
    private WebSocket WebSocket { get; set; }

    void Start()
    {
        print("Client started");
        WebSocket = new WebSocket(webSocketUrl);
        Listen(WebSocket);
    }


    private void Listen(WebSocket ws)
    {
        var x = File.ReadAllText(@".\Assets\Resources\Json\init.json");

        ws.OnOpen += (sender, e) =>
        {
            print($"Client >> Listening to {webSocketUrl}");
            WebSocket.Send(x);
        };

        ws.OnClose += (sender, e) => {
            print($"Client >> Connection to {webSocketUrl} ended");
        };

        ws.OnMessage += (sender, e) =>
        {
            string data = e.Data;
            print($"Client >> Data received:\n{data}");
            SetLatestData(data);
        };

        ws.Connect();
    }

    private void SetLatestData(string data)
    {
        ControllerData = JsonUtility.FromJson<ControllerData>(data);
    }

    public void Send(State state)
    {
        WebSocket.Send(state.ToJson()); // todo Send only the lights
    }
}
