using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    Client Client { get; set; }
    State State { get; set; }

    void Start()
    {
        Client = gameObject.GetComponent<Client>();
        State = gameObject.GetComponentInChildren<State>();
    }

    void Update()
    {
        if (Client.ControllerData != null)
        {
            State.SetAllLights(Client.ControllerData);
        }
        if (State.IsUpdated)
        {
            Client.Send(State);
        }
    }
}
