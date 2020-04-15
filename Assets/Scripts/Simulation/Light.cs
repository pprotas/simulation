using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

[Serializable]
class Light : MonoBehaviour
{
    [SerializeField]
    public LightColor color;

    [SerializeField]
    public string id;

    private void Start()
    {
        if (id.IsNullOrEmpty())
        {
            id = gameObject.GetComponentInParent<Lane>().id;
        }
    }
}
