using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
class Light : MonoBehaviour
{
    [SerializeField]
    public LightColor color;

    public Light()
    {

    }
}
