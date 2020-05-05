using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// The main simulation script, handles communication with controller & the staet of the simulation
/// </summary>
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
        // Updates the state of the simulation to the latest data from the controller
        if (Client.ControllerData != null)
        {
            State.SetAllLights(Client.ControllerData);
        }
        // Sends new state data to controller on update
        if (State.IsUpdated)
        {
            Client.Send(State);
        }
    }
}
