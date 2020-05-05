using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class WayPoint : MonoBehaviour
{
    [SerializeField]
    public WayPoint Next;

    // Waypoints with CheckLight = true force all trafic users to look at the light color before continuing
    [SerializeField]
    public bool CheckLight = false;

    // Sets the index for the waypoint, allowing multiple waypoints to be chained together. Cars start at waypoint 0 and onward
    [SerializeField]
    public int LightIndex = 0;
}
