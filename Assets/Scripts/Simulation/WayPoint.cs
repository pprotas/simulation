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

    [SerializeField]
    public bool CheckLight = false;

    [SerializeField]
    public int LightIndex = 0;

    void Start()
    {
        
    }
}
