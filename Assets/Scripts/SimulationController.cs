using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    // Start is called before the first frame update
    Client Client { get; set; }
    void Start()
    {
        Client = new Client(new Uri("ws://trafic.azurewebsites.net/simulation"));
        Client.Listen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
