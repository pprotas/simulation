using System.IO;
using UnityEngine;
using WebSocketSharp;

/// <summary>
/// Websocket client meant for connecting to an external controller. 
/// </summary>
class Client : MonoBehaviour
{
    // Default url, can be changed in the simulation settings
    [SerializeField]
    private string webSocketUrl = "ws://localhost:8080";

    // Latest data retrieved from the controller
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
        // File that represents an empty state according to the protocol
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

            // Updates the controller data on every message
            SetLatestData(data);
        };

        ws.Connect();
    }

    private void SetLatestData(string data)
    {
        // Converts the data from JSON to a ControllerData class
        ControllerData = JsonUtility.FromJson<ControllerData>(data);
    }

    public void Send(State state)
    {
        var x = state.ToJson();
        WebSocket.Send(x);
    }
}
