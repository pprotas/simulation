using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.Collections.Specialized;
using Boo.Lang;

/// <summary>
/// Class representing the total state of all traffic lights in the simulation
/// </summary>
internal class State : MonoBehaviour
{
    [SerializeField]
    public List<Lane> lanes;
    private Light[] Lights;

    private bool _isUpdated;
    public bool IsUpdated
    {
        get
        {
            var tmp = _isUpdated;
            // State is not updated anymore if someone accesses this property
            // ("can only be read once" mechanism)
            _isUpdated = false;
            return tmp;
        }
        set
        {
            _isUpdated = value;
        }
    }

    void Start()
    {
        lanes = new List<Lane>(gameObject.GetComponentsInChildren<Lane>());
        Lights = gameObject.GetComponentsInChildren<Light>();
    }

    void Update()
    {
        SpawnCarAtRandom();
    }

    Lane GetLaneById(string id)
    {
        Lane result;

        if (!string.IsNullOrEmpty(id))
        {
            result = lanes.SingleOrDefault(lane => lane.id == id);
        }
        else
        {
            print($"Empty id for Lane specified");
            return null;
        }

        if (result)
        {
            return result;
        }
        print($"No lane found with the id of {id}");
        return null;
    }

    Light GetLightById(string id)
    {
        Light result;

        if (!string.IsNullOrEmpty(id))
        {
            result = Lights.SingleOrDefault(light => light.id == id);
        }
        else
        {
            print($"Empty id for light specified");
            return null;
        }

        if (result)
        {
            return result;
        }
        print($"No light found with the id of {id}");
        return null;
    }

    private void SetLightForId(string laneId, LightColor color)
    {
        Light lane = GetLightById(laneId);
        if (lane != null)
        {
            lane.color = color;
        }
    }

    public void SetAllLights(LightColor color)
    {
        foreach (Light light in Lights)
        {
            light.color = color;
        }
    }

    public void SetAllLights(ControllerData data)
    {
        foreach (FieldInfo field in data.GetType().GetFields(BindingFlags.Instance |
                       BindingFlags.Static |
                       BindingFlags.NonPublic |
                       BindingFlags.Public))
        {
            LightColor color = GetLightColor(data, field);
            string id = field.Name;
            SetLightForId(id, color);
        }
    }

    private LightColor GetLightColor(ControllerData data, FieldInfo field)
    {
        return (LightColor)field.GetValue(data);
    }

    private void SpawnCarAtRandom()
    {
        lanes[Random.Range(lanes.IndexOf(lanes.First()), lanes.Count)].TrySpawnCar();
    }

    // Returns a JSON formatted string that represents the state, according to protocol.
    public string ToJson()
    {
        string result = "{";
        foreach(Light light in Lights)
        {
            result += $"\"{light.id}\": {light.lane.cars.Count},";
        }
        result = result.Substring(0, result.Length - 1);
        result += "}";
        return result;
    }
}