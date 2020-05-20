using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using WebSocketSharp;

/// <summary>
/// Class representing a light in the simulation
/// </summary>
[Serializable]
class Light : MonoBehaviour
{
    [SerializeField]
    public LightColor color;
    public Material material;
    Renderer rend;

    [SerializeField]
    public string id;

    public Lane lane;

    Color orange = new Color(1.0f, 0.64f, 0.0f);

    private void Start()
    {
        rend = GetComponent<Renderer>();
        material = GetComponent<Renderer>().material;
        rend.enabled = true;
        
        if (id.IsNullOrEmpty())
        {
            id = gameObject.GetComponentInParent<Lane>().id;
        }
        lane = gameObject.GetComponentInParent<Lane>();
        
    }

    private void Update()
    {
        switch (color)
        {
            case LightColor.Red:
                rend.sharedMaterial.color = Color.red;
                break;
            case LightColor.Orange:
                rend.sharedMaterial.color = orange;
                break;
            case LightColor.Green:
                rend.sharedMaterial.color = Color.green;
                break;
        }

    }
}
